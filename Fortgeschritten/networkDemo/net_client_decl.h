// Forward declarations for NetClient v1.1 — auto-included in _forward.h
#pragma once
#include <stdbool.h>

int         NetClient_Init(void);
void        NetClient_Exit(void);
void        NetClient_UseHttps(int enable);
void        NetClient_SetHeader(const char* name, const char* value);
void        NetClient_ClearHeaders(void);
void        NetClient_SetTimeout(int connect_s, int io_s);

int         NetClient_BeginGet(const char* host, const char* path);
int         NetClient_BeginPost(const char* host, const char* path, const char* body, const char* content_type);
int         NetClient_BeginPut(const char* host, const char* path, const char* body, const char* content_type);
int         NetClient_BeginDelete(const char* host, const char* path);
int         NetClient_BeginHead(const char* host, const char* path);
int         NetClient_BeginDownload(const char* host, const char* path, const char* savepath);

int         NetClient_IsComplete(void);
int         NetClient_WasSuccess(void);
void        NetClient_Finish(void);

const char* NetClient_GetResponse(void);
int         NetClient_GetResponseLen(void);
int         NetClient_GetStatusCode(void);
const char* NetClient_GetRespHeader(const char* name);
const char* NetClient_GetError(void);

void        NetClient_UrlEncode(const char* input, char* output, int outsz);
void        NetClient_ParseUrl(const char* url, char* host, int hostsz, char* path, int pathsz, int* use_https);

const char* NetClient_JsonStr(const char* json, const char* field, char* out, int outsz);
int         NetClient_JsonInt(const char* json, const char* field, int def_val);
float       NetClient_JsonFloat(const char* json, const char* field, float def_val);
int         NetClient_JsonBool(const char* json, const char* field, int def_val);

const char* NetClient_GetWeatherTemp(void);
const char* NetClient_GetWeatherWind(void);
int         NetClient_GetWeatherCode(void);
void        NetClient_StoreCityResult(int idx);
const char* NetClient_GetCityDisplay(int idx);

// Legacy alias
int         NetClient_StartRequest(const char* host, const char* path);
