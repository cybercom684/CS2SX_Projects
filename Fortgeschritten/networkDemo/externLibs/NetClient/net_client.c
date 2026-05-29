// CS2SX NetClient v1.1.0 — HTTP/HTTPS client for Nintendo Switch (libnx)

#include "net_client.h"
#include <switch.h>
#include <switch/runtime/devices/socket.h>

#include <string.h>
#include <stdio.h>
#include <stdarg.h>
#include <stdlib.h>
#include <ctype.h>
#include <strings.h>   // strcasecmp, strncasecmp
#include <sys/socket.h>
#include <sys/select.h>
#include <fcntl.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <errno.h>
#include <unistd.h>
#include <math.h>

// ── Constants ─────────────────────────────────────────────────────────────────

#define NET_BUF_SIZE     65536
#define REQ_BUF_SIZE     4096
#define MAX_REQ_HEADERS  16
#define MAX_RESP_HEADERS 32
#define MAX_CITIES       16

// ── Types ─────────────────────────────────────────────────────────────────────

typedef enum { NET_IDLE=0, NET_RUNNING=1, NET_DONE_OK=2, NET_DONE_ERR=3 } NetState;

typedef struct { char name[64]; char value[256]; } NetHeader;

typedef enum { METHOD_GET, METHOD_POST, METHOD_PUT,
               METHOD_DELETE, METHOD_HEAD, METHOD_DOWNLOAD } NetMethod;

// ── State ─────────────────────────────────────────────────────────────────────

static char  s_error[256]                    = "";
static char  s_response[NET_BUF_SIZE + 1]    = "";
static int   s_response_len                  = 0;
static int   s_status_code                   = 0;

static NetHeader s_resp_headers[MAX_RESP_HEADERS];
static int       s_resp_header_count         = 0;

// Request config
static NetHeader s_req_headers[MAX_REQ_HEADERS];
static int       s_req_header_count          = 0;
static int       s_use_https                 = 0;  // 0=HTTP default; call UseHttps(1) for HTTPS
static int       s_connect_timeout           = 6;
static int       s_io_timeout                = 10;

// Current request parameters
static NetMethod s_method                    = METHOD_GET;
static char      s_req_host[256]             = "";
static char      s_req_path[1024]            = "";
static char      s_req_body[NET_BUF_SIZE/4]  = "";
static char      s_req_ctype[128]            = "";
static char      s_download_path[512]        = "";

// Threading
static Thread    s_thread;
static NetState  s_state                     = NET_IDLE;
static bool      s_thread_open               = false;

// SSL
static bool      s_sock_init                 = false;
static bool      s_ssl_init                  = false;
static SslContext s_ctx;

// Weather / per-city storage
static char  s_weather_temp[32]              = "";
static char  s_weather_wind[32]              = "";
static int   s_weather_code                  = -1;
static char  s_city_display[MAX_CITIES][80];

// ── Internal helpers ──────────────────────────────────────────────────────────

static void set_err(const char* fmt, ...) {
    va_list ap; va_start(ap, fmt);
    vsnprintf(s_error, sizeof(s_error)-1, fmt, ap);
    va_end(ap);
}

static void extract_json_num(const char* json, const char* field, char* dst, int dstsz) {
    dst[0] = '\0';
    char needle[128];
    snprintf(needle, sizeof(needle), "\"%s\":", field);
    const char* p = strstr(json, needle);
    if (!p) return;
    p += strlen(needle);
    while (*p == ' ') p++;
    int i = 0;
    while (*p && *p != ',' && *p != '}' && *p != '"' && *p != ']' && i < dstsz-1)
        dst[i++] = *p++;
    dst[i] = '\0';
    while (i > 0 && (dst[i-1]==' '||dst[i-1]=='\r'||dst[i-1]=='\n')) dst[--i]='\0';
}

static void parse_weather_json(void) {
    const char* block = strstr(s_response, "\"current_weather\":");
    if (!block) block = s_response;
    extract_json_num(block, "temperature", s_weather_temp, sizeof(s_weather_temp));
    extract_json_num(block, "windspeed",   s_weather_wind, sizeof(s_weather_wind));
    char cb[16]=""; extract_json_num(block, "weathercode", cb, sizeof(cb));
    s_weather_code = cb[0] ? atoi(cb) : -1;
}

static void parse_response_meta(const char* raw) {
    s_status_code = 0;
    s_resp_header_count = 0;
    if (!raw || !raw[0]) return;

    // Status line: HTTP/1.x NNN ...
    if (strncmp(raw, "HTTP/", 5) == 0) {
        const char* sp = strchr(raw, ' ');
        if (sp) s_status_code = atoi(sp + 1);
    }

    // Headers (after status line, until empty line)
    const char* p = strchr(raw, '\n');
    if (p) p++;
    while (p && s_resp_header_count < MAX_RESP_HEADERS) {
        if (*p == '\r' || *p == '\n') break;
        const char* colon = strchr(p, ':');
        const char* eol   = strchr(p, '\n');
        if (!colon || !eol || colon > eol) break;
        int nlen = (int)(colon - p);
        if (nlen > 0 && nlen < 63) {
            strncpy(s_resp_headers[s_resp_header_count].name, p, nlen);
            s_resp_headers[s_resp_header_count].name[nlen] = '\0';
            const char* val = colon + 1;
            while (*val == ' ') val++;
            int vlen = (int)(eol - val);
            if (vlen > 0 && val[vlen-1] == '\r') vlen--;
            if (vlen > 0 && vlen < 255) {
                strncpy(s_resp_headers[s_resp_header_count].value, val, vlen);
                s_resp_headers[s_resp_header_count].value[vlen] = '\0';
                s_resp_header_count++;
            }
        }
        p = eol + 1;
    }
}

static int resolve_host(const char* host, struct sockaddr_in* out, int port) {
    struct addrinfo hints, *res = NULL;
    memset(&hints, 0, sizeof(hints));
    hints.ai_family   = AF_INET;
    hints.ai_socktype = SOCK_STREAM;
    char ps[8]; snprintf(ps, sizeof(ps), "%d", port);
    int err = getaddrinfo(host, ps, &hints, &res);
    if (err || !res) { set_err("DNS failed for %s: %d", host, err); return 0; }
    memcpy(out, res->ai_addr, sizeof(*out));
    freeaddrinfo(res);
    return 1;
}

static int connect_timeout(int fd, struct sockaddr* addr, socklen_t alen, int secs) {
    int fl = fcntl(fd, F_GETFL, 0);
    fcntl(fd, F_SETFL, fl | O_NONBLOCK);
    int r = connect(fd, addr, alen);
    if (r == 0) { fcntl(fd, F_SETFL, fl); return 1; }
    if (errno != EINPROGRESS) { set_err("connect: %d", errno); return 0; }
    struct timeval tv = {.tv_sec=secs, .tv_usec=0};
    fd_set ws; FD_ZERO(&ws); FD_SET(fd, &ws);
    if (select(fd+1, NULL, &ws, NULL, &tv) <= 0) { set_err("connect timeout"); return 0; }
    int e=0; socklen_t el=sizeof(e);
    getsockopt(fd, SOL_SOCKET, SO_ERROR, &e, &el);
    if (e) { set_err("connect error: %d", e); return 0; }
    fcntl(fd, F_SETFL, fl);
    return 1;
}

// Build complete HTTP request into buf[].  Returns length.
static int build_http_request(char* buf, int bufsz,
                               const char* method_str, const char* host,
                               const char* path, const char* body,
                               const char* content_type)
{
    int pos = snprintf(buf, bufsz,
        "%s %s HTTP/1.1\r\n"
        "Host: %s\r\n"
        "User-Agent: CS2SX-NetClient/1.1\r\n"
        "Accept: */*\r\n",
        method_str, path, host);

    // Custom headers
    for (int i = 0; i < s_req_header_count && pos < bufsz-2; i++)
        pos += snprintf(buf+pos, bufsz-pos, "%s: %s\r\n",
                        s_req_headers[i].name, s_req_headers[i].value);

    // Body
    int body_len = (body && body[0]) ? (int)strlen(body) : 0;
    if (body_len > 0) {
        if (content_type && content_type[0])
            pos += snprintf(buf+pos, bufsz-pos, "Content-Type: %s\r\n", content_type);
        pos += snprintf(buf+pos, bufsz-pos, "Content-Length: %d\r\n", body_len);
    }

    pos += snprintf(buf+pos, bufsz-pos, "Connection: close\r\n\r\n");
    if (body_len > 0 && pos + body_len < bufsz) {
        memcpy(buf+pos, body, body_len);
        pos += body_len;
        buf[pos] = '\0';
    }
    return pos;
}

// ── Core request executor (runs in background thread) ─────────────────────────

static int do_request_http(const char* host, const char* path,
                           const char* method_str, const char* body,
                           const char* content_type)
{
    struct sockaddr_in addr;
    if (!resolve_host(host, &addr, 80)) return 0;

    int fd = socket(AF_INET, SOCK_STREAM, 0);
    if (fd < 0) { set_err("socket: %d", errno); return 0; }

    struct timeval tv = {.tv_sec=s_io_timeout, .tv_usec=0};
    setsockopt(fd, SOL_SOCKET, SO_RCVTIMEO, &tv, sizeof(tv));
    setsockopt(fd, SOL_SOCKET, SO_SNDTIMEO, &tv, sizeof(tv));

    if (!connect_timeout(fd, (struct sockaddr*)&addr, sizeof(addr), s_connect_timeout)) {
        close(fd); return 0;
    }

    static char req[REQ_BUF_SIZE];
    int rlen = build_http_request(req, sizeof(req), method_str, host, path, body, content_type);
    send(fd, req, rlen, 0);

    static char rbuf[NET_BUF_SIZE+1];
    int total=0, n;
    while ((n = recv(fd, rbuf+total, NET_BUF_SIZE-total, 0)) > 0) total += n;
    rbuf[total] = '\0';
    close(fd);

    parse_response_meta(rbuf);

    if (s_method == METHOD_HEAD) {
        // HEAD: no body, just store headers
        s_response[0]  = '\0';
        s_response_len = 0;
        return 1;
    }

    char* body_start = strstr(rbuf, "\r\n\r\n");
    body_start = body_start ? body_start + 4 : rbuf;
    int blen = total - (int)(body_start - rbuf);
    if (blen < 0) blen = 0;
    if (blen > NET_BUF_SIZE) blen = NET_BUF_SIZE;

    if (s_method == METHOD_DOWNLOAD && s_download_path[0]) {
        FILE* f = fopen(s_download_path, "wb");
        if (f) { fwrite(body_start, 1, blen, f); fclose(f); }
        else   { set_err("fopen: %s", s_download_path); return 0; }
        s_response[0]  = '\0';
        s_response_len = 0;
    } else {
        memcpy(s_response, body_start, blen);
        s_response[blen] = '\0';
        s_response_len   = blen;
        parse_weather_json();
    }
    return 1;
}

static int do_request_https(const char* host, const char* path,
                            const char* method_str, const char* body,
                            const char* content_type)
{
    struct sockaddr_in addr;
    if (!resolve_host(host, &addr, 443)) return 0;

    int fd = socket(AF_INET, SOCK_STREAM, 0);
    if (fd < 0) { set_err("socket: %d", errno); return 0; }

    struct timeval tv = {.tv_sec=s_io_timeout, .tv_usec=0};
    setsockopt(fd, SOL_SOCKET, SO_RCVTIMEO, &tv, sizeof(tv));
    setsockopt(fd, SOL_SOCKET, SO_SNDTIMEO, &tv, sizeof(tv));

    if (!connect_timeout(fd, (struct sockaddr*)&addr, sizeof(addr), s_connect_timeout)) {
        close(fd); return 0;
    }

    SslConnection conn;
    Result rc = sslContextCreateConnection(&s_ctx, &conn);
    if (R_FAILED(rc)) { set_err("sslContextCreateConnection: 0x%08x", rc); close(fd); return 0; }

    int ssl_fd = socketSslConnectionSetSocketDescriptor(&conn, fd);
    if (ssl_fd < 0 && errno != ENOENT) {
        set_err("socketSslConnectionSetSocketDescriptor: %d", errno);
        sslConnectionClose(&conn); return 0;
    }
    fd = (ssl_fd >= 0) ? ssl_fd : -1;

    sslConnectionSetHostName(&conn, host, (u32)(strlen(host)+1));
    rc = sslConnectionSetVerifyOption(&conn, SslVerifyOption_PeerCa | SslVerifyOption_HostName);
    if (R_FAILED(rc)) sslConnectionSetVerifyOption(&conn, SslVerifyOption_PeerCa);

    // Blocking handshake (runs in background thread — won't freeze the app)
    rc = sslConnectionDoHandshake(&conn, NULL, NULL, NULL, 0);
    if (R_FAILED(rc)) {
        set_err("TLS handshake: 0x%08x", rc);
        sslConnectionClose(&conn); if (fd>=0) close(fd); return 0;
    }

    static char req[REQ_BUF_SIZE];
    int rlen = build_http_request(req, sizeof(req), method_str, host, path, body, content_type);
    u32 sent = 0;
    rc = sslConnectionWrite(&conn, req, (u32)rlen, &sent);
    if (R_FAILED(rc)) {
        set_err("sslConnectionWrite: 0x%08x", rc);
        sslConnectionClose(&conn); if (fd>=0) close(fd); return 0;
    }

    static char rbuf[NET_BUF_SIZE+1];
    int total = 0;
    while (total < NET_BUF_SIZE) {
        u32 got = 0;
        rc = sslConnectionRead(&conn, rbuf+total, (u32)(NET_BUF_SIZE-total), &got);
        if (R_FAILED(rc) || got == 0) break;
        total += (int)got;
    }
    rbuf[total] = '\0';
    sslConnectionClose(&conn);
    if (fd >= 0) close(fd);

    parse_response_meta(rbuf);

    if (s_method == METHOD_HEAD) {
        s_response[0] = '\0'; s_response_len = 0; return 1;
    }

    char* body_start = strstr(rbuf, "\r\n\r\n");
    body_start = body_start ? body_start + 4 : rbuf;
    int blen = total - (int)(body_start - rbuf);
    if (blen < 0) blen = 0;
    if (blen > NET_BUF_SIZE) blen = NET_BUF_SIZE;

    if (s_method == METHOD_DOWNLOAD && s_download_path[0]) {
        FILE* f = fopen(s_download_path, "wb");
        if (f) { fwrite(body_start, 1, blen, f); fclose(f); }
        else   { set_err("fopen: %s", s_download_path); return 0; }
        s_response[0] = '\0'; s_response_len = 0;
    } else {
        memcpy(s_response, body_start, blen);
        s_response[blen] = '\0';
        s_response_len   = blen;
        parse_weather_json();
    }
    return 1;
}

// Thread entry
static void net_thread(void* arg) {
    (void)arg;
    s_error[0] = '\0';
    s_status_code = 0;
    s_resp_header_count = 0;

    const char* method_str = "GET";
    if      (s_method == METHOD_POST)     method_str = "POST";
    else if (s_method == METHOD_PUT)      method_str = "PUT";
    else if (s_method == METHOD_DELETE)   method_str = "DELETE";
    else if (s_method == METHOD_HEAD)     method_str = "HEAD";
    else if (s_method == METHOD_DOWNLOAD) method_str = "GET";

    int ok = s_use_https
        ? do_request_https(s_req_host, s_req_path, method_str, s_req_body, s_req_ctype)
        : do_request_http (s_req_host, s_req_path, method_str, s_req_body, s_req_ctype);

    s_state = ok ? NET_DONE_OK : NET_DONE_ERR;
}

// ── SSL + CA cert init ─────────────────────────────────────────────────────────

static void import_ca_certs(void) {
    u32 all = 0xFFFFFFFF;
    u32 sz  = 0;
    if (R_FAILED(sslGetCertificateBufSize(&all, 1, &sz)) || sz == 0) return;
    void* buf = malloc(sz);
    if (!buf) return;
    u32 n = 0;
    if (R_SUCCEEDED(sslGetCertificates(buf, sz, &all, 1, &n)) && n > 0) {
        u64 id = 0;
        sslContextImportServerPki(&s_ctx, buf, sz, SslCertificateFormat_Der, &id);
    }
    free(buf);
}

static int start_thread(void) {
    s_state = NET_RUNNING;
    Result rc = threadCreate(&s_thread, net_thread, NULL, NULL, 0x10000, 0x2C, -2);
    if (R_FAILED(rc)) { set_err("threadCreate: 0x%08x", rc); s_state = NET_IDLE; return 0; }
    rc = threadStart(&s_thread);
    if (R_FAILED(rc)) { threadClose(&s_thread); set_err("threadStart: 0x%08x", rc); s_state = NET_IDLE; return 0; }
    s_thread_open = true;
    return 1;
}

// ── Public API ────────────────────────────────────────────────────────────────

int NetClient_Init(void) {
    if (s_sock_init && s_ssl_init) return 1;

    if (!s_sock_init) {
        Result rc = socketInitializeDefault();
        if (R_FAILED(rc)) { set_err("socketInitializeDefault: 0x%08x", rc); return 0; }
        s_sock_init = true;
    }
    if (!s_ssl_init) {
        Result rc = sslInitialize(2);
        if (R_FAILED(rc)) { set_err("sslInitialize: 0x%08x", rc); return 0; }
        rc = sslCreateContext(&s_ctx, SslVersion_Auto);
        if (R_FAILED(rc)) { sslExit(); set_err("sslCreateContext: 0x%08x", rc); return 0; }
        import_ca_certs();
        s_ssl_init = true;
    }
    s_error[0] = '\0';
    return 1;
}

void NetClient_Exit(void) {
    NetClient_Finish();
    if (s_ssl_init) { sslContextClose(&s_ctx); sslExit(); s_ssl_init = false; }
    if (s_sock_init) { socketExit(); s_sock_init = false; }
}

void NetClient_UseHttps(int enable)  { s_use_https = enable; }
void NetClient_SetTimeout(int cs, int io) { s_connect_timeout = cs; s_io_timeout = io; }

void NetClient_SetHeader(const char* name, const char* value) {
    if (!name || !value) return;
    // Update existing
    for (int i = 0; i < s_req_header_count; i++) {
        if (strcasecmp(s_req_headers[i].name, name) == 0) {
            strncpy(s_req_headers[i].value, value, 255);
            return;
        }
    }
    if (s_req_header_count < MAX_REQ_HEADERS) {
        strncpy(s_req_headers[s_req_header_count].name,  name,  63);
        strncpy(s_req_headers[s_req_header_count].value, value, 255);
        s_req_header_count++;
    }
}

void NetClient_ClearHeaders(void) { s_req_header_count = 0; }

static int begin_request(NetMethod method, const char* host, const char* path,
                         const char* body, const char* ctype, const char* savepath)
{
    if (s_state == NET_RUNNING) { set_err("request already in progress"); return 0; }
    NetClient_Finish();

    strncpy(s_req_host, host, sizeof(s_req_host)-1);
    strncpy(s_req_path, path, sizeof(s_req_path)-1);
    s_req_body[0] = '\0'; s_req_ctype[0] = '\0'; s_download_path[0] = '\0';
    if (body)    strncpy(s_req_body,       body,    sizeof(s_req_body)-1);
    if (ctype)   strncpy(s_req_ctype,      ctype,   sizeof(s_req_ctype)-1);
    if (savepath)strncpy(s_download_path,  savepath,sizeof(s_download_path)-1);
    s_method = method;
    return start_thread();
}

int NetClient_BeginGet(const char* h, const char* p)
    { return begin_request(METHOD_GET, h, p, NULL, NULL, NULL); }
int NetClient_BeginPost(const char* h, const char* p, const char* b, const char* ct)
    { return begin_request(METHOD_POST, h, p, b, ct, NULL); }
int NetClient_BeginPut(const char* h, const char* p, const char* b, const char* ct)
    { return begin_request(METHOD_PUT, h, p, b, ct, NULL); }
int NetClient_BeginDelete(const char* h, const char* p)
    { return begin_request(METHOD_DELETE, h, p, NULL, NULL, NULL); }
int NetClient_BeginHead(const char* h, const char* p)
    { return begin_request(METHOD_HEAD, h, p, NULL, NULL, NULL); }
int NetClient_BeginDownload(const char* h, const char* p, const char* save)
    { return begin_request(METHOD_DOWNLOAD, h, p, NULL, NULL, save); }

// Legacy alias (keeps existing weather demo working)
int NetClient_StartRequest(const char* h, const char* p) { return NetClient_BeginGet(h, p); }

int  NetClient_IsComplete(void)  { return s_state == NET_DONE_OK || s_state == NET_DONE_ERR; }
int  NetClient_WasSuccess(void)  { return s_state == NET_DONE_OK; }

void NetClient_Finish(void) {
    if (s_thread_open) {
        threadWaitForExit(&s_thread);
        threadClose(&s_thread);
        s_thread_open = false;
    }
    s_state = NET_IDLE;
}

const char* NetClient_GetResponse(void)    { return s_response; }
int         NetClient_GetResponseLen(void) { return s_response_len; }
int         NetClient_GetStatusCode(void)  { return s_status_code; }
const char* NetClient_GetError(void)       { return s_error; }

const char* NetClient_GetRespHeader(const char* name) {
    for (int i = 0; i < s_resp_header_count; i++)
        if (strcasecmp(s_resp_headers[i].name, name) == 0)
            return s_resp_headers[i].value;
    return "";
}

// ── URL helpers ───────────────────────────────────────────────────────────────

void NetClient_UrlEncode(const char* in, char* out, int outsz) {
    int j = 0;
    for (int i = 0; in[i] && j < outsz-3; i++) {
        unsigned char c = (unsigned char)in[i];
        if (isalnum(c) || c=='-'||c=='_'||c=='.'||c=='~') {
            out[j++] = c;
        } else {
            j += snprintf(out+j, outsz-j, "%%%02X", c);
        }
    }
    out[j] = '\0';
}

void NetClient_ParseUrl(const char* url, char* host, int hostsz,
                        char* path, int pathsz, int* use_https)
{
    host[0] = '\0'; path[0] = '\0';
    if (use_https) *use_https = 1;
    int start = 0;
    if (strncmp(url, "https://", 8) == 0) { start = 8; if (use_https) *use_https = 1; }
    else if (strncmp(url, "http://", 7) == 0) { start = 7; if (use_https) *use_https = 0; }
    const char* slash = strchr(url + start, '/');
    if (slash) {
        int hlen = (int)(slash - url - start);
        if (hlen < hostsz) { strncpy(host, url+start, hlen); host[hlen]='\0'; }
        strncpy(path, slash, pathsz-1); path[pathsz-1]='\0';
    } else {
        strncpy(host, url+start, hostsz-1); host[hostsz-1]='\0';
        strncpy(path, "/", pathsz-1);
    }
}

// ── JSON helpers ──────────────────────────────────────────────────────────────

const char* NetClient_JsonStr(const char* json, const char* field, char* out, int outsz) {
    out[0] = '\0';
    if (!json || !field) return out;
    char needle[128];
    snprintf(needle, sizeof(needle), "\"%s\":\"", field);
    const char* p = strstr(json, needle);
    if (!p) return out;
    p += strlen(needle);
    int i = 0;
    while (*p && *p != '"' && i < outsz-1) {
        // Handle escape sequences
        if (*p == '\\' && *(p+1)) { p++; }
        out[i++] = *p++;
    }
    out[i] = '\0';
    return out;
}

int NetClient_JsonInt(const char* json, const char* field, int def_val) {
    char buf[32]; extract_json_num(json, field, buf, sizeof(buf));
    return buf[0] ? atoi(buf) : def_val;
}

float NetClient_JsonFloat(const char* json, const char* field, float def_val) {
    char buf[32]; extract_json_num(json, field, buf, sizeof(buf));
    return buf[0] ? (float)atof(buf) : def_val;
}

int NetClient_JsonBool(const char* json, const char* field, int def_val) {
    char buf[16]; extract_json_num(json, field, buf, sizeof(buf));
    if (!buf[0]) return def_val;
    if (strncmp(buf, "true",  4) == 0) return 1;
    if (strncmp(buf, "false", 5) == 0) return 0;
    return atoi(buf) != 0;
}

// ── Weather / per-city ────────────────────────────────────────────────────────

const char* NetClient_GetWeatherTemp(void) { return s_weather_temp; }
const char* NetClient_GetWeatherWind(void) { return s_weather_wind; }
int         NetClient_GetWeatherCode(void) { return s_weather_code; }

static const char* wmo_to_text(int code) {
    if (code == 0)          return "Klarer Himmel";
    if (code <= 2)          return "Leicht bewoelkt";
    if (code == 3)          return "Bedeckt";
    if (code <= 48)         return "Nebelig";
    if (code <= 57)         return "Nieselregen";
    if (code <= 67)         return "Regen";
    if (code <= 77)         return "Schnee";
    if (code <= 82)         return "Schauer";
    if (code <= 86)         return "Schneeschauer";
    if (code <= 99)         return "Gewitter";
    return "Unbekannt";
}

void NetClient_StoreCityResult(int idx) {
    if (idx < 0 || idx >= MAX_CITIES) return;
    snprintf(s_city_display[idx], sizeof(s_city_display[0]),
        "%s  %s Grad  Wind %s km/h",
        wmo_to_text(s_weather_code),
        s_weather_temp[0] ? s_weather_temp : "--",
        s_weather_wind[0] ? s_weather_wind : "--");
}

const char* NetClient_GetCityDisplay(int idx) {
    return (idx >= 0 && idx < MAX_CITIES) ? s_city_display[idx] : "";
}
