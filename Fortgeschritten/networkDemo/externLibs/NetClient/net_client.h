// =============================================================================
// CS2SX NetClient — General-purpose HTTP/HTTPS client for Nintendo Switch
// Version: 1.1.0
//
// Usage in CS2SX:
//   cs2sx addLib NetClient
//   Then call NetClient.Init(), BeginGet/Post/..., PollResult() each frame.
//
// Supports: GET POST PUT DELETE HEAD · HTTP + HTTPS · Custom headers
//           File download · URL helpers · JSON helpers
// =============================================================================
#pragma once
#include <stdbool.h>

// ── Init / Exit ───────────────────────────────────────────────────────────────

// Initialize socket + SSL services.  Call from OnInit().  Returns 1 on success.
int  NetClient_Init(void);
// Release all resources.  MUST be called from OnExit() to avoid Atmosphere crash.
void NetClient_Exit(void);

// ── Configuration (set before BeginXxx) ──────────────────────────────────────

// 1 = use HTTPS (default), 0 = plain HTTP.  Auto-detected if UseHttps not called:
//   port 443 → HTTPS, port 80 → HTTP.
void NetClient_UseHttps(int enable);

// Add a custom request header.  Cleared after each request unless kept manually.
void NetClient_SetHeader(const char* name, const char* value);
// Remove all custom headers.
void NetClient_ClearHeaders(void);

// TCP connect timeout and SSL I/O timeout in seconds.  Defaults: 6s / 10s.
void NetClient_SetTimeout(int connect_s, int io_s);

// ── Async request interface ───────────────────────────────────────────────────
// These start a background thread and return immediately.
// Call IsComplete() each frame, then Finish() once done.

int  NetClient_BeginGet(const char* host, const char* path);
int  NetClient_BeginPost(const char* host, const char* path,
                         const char* body, const char* content_type);
int  NetClient_BeginPut(const char* host, const char* path,
                        const char* body, const char* content_type);
int  NetClient_BeginDelete(const char* host, const char* path);
int  NetClient_BeginHead(const char* host, const char* path);

// Download response body directly to a file on SD card (async).
// savepath: absolute path, e.g. "/switch/myapp/data.zip"
int  NetClient_BeginDownload(const char* host, const char* path, const char* savepath);

// ── Poll ──────────────────────────────────────────────────────────────────────

// 1 when the background thread has finished (success or fail).
int  NetClient_IsComplete(void);
// 1 if the last completed request succeeded.
int  NetClient_WasSuccess(void);
// Wait for the thread and clean up.  MUST be called after IsComplete returns 1.
void NetClient_Finish(void);

// ── Response ──────────────────────────────────────────────────────────────────

// Response body (valid after IsComplete=1 and before next BeginXxx).
const char* NetClient_GetResponse(void);
int         NetClient_GetResponseLen(void);

// HTTP status code (e.g. 200, 404, 301).  0 if not available.
int         NetClient_GetStatusCode(void);

// Get a response header value by name (case-insensitive).  "" if not found.
const char* NetClient_GetRespHeader(const char* name);

// ── Error ─────────────────────────────────────────────────────────────────────

const char* NetClient_GetError(void);

// ── URL helpers ───────────────────────────────────────────────────────────────

// Percent-encode a string for use in URL query parameters.
void NetClient_UrlEncode(const char* input, char* output, int outsz);

// Split "https://host/path?query" into host and path+query parts.
// Sets *use_https to 1 for https://, 0 for http://.
void NetClient_ParseUrl(const char* url,
                        char* host, int hostsz,
                        char* path, int pathsz,
                        int*  use_https);

// ── JSON helpers ──────────────────────────────────────────────────────────────
// Operate on any JSON string.  Field must be a simple key at any nesting level.

// Extract a string value: "field":"value" → copies into out[], returns out.
const char* NetClient_JsonStr(const char* json, const char* field,
                              char* out, int outsz);
// Extract an integer value: "field":42 → returns 42, or def_val if not found.
int         NetClient_JsonInt(const char* json, const char* field, int def_val);
// Extract a float value: "field":3.14 → returns 3.14f.
float       NetClient_JsonFloat(const char* json, const char* field, float def_val);
// Extract a boolean value: "field":true/false → 1/0.
int         NetClient_JsonBool(const char* json, const char* field, int def_val);

// ── Pre-parsed weather storage (weather demo compat) ──────────────────────────

// Pre-parsed values from last Open-Meteo response (valid until next request).
const char* NetClient_GetWeatherTemp(void);
const char* NetClient_GetWeatherWind(void);
int         NetClient_GetWeatherCode(void);

// Per-city persistent ASCII display strings (up to 16 cities).
void        NetClient_StoreCityResult(int idx);
const char* NetClient_GetCityDisplay(int idx);
