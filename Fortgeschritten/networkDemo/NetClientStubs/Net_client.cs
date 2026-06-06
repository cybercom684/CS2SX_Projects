// Auto-generated from externLibs/NetClient/net_client.h
// DO NOT EDIT — regeneriert via cs2sx addLib
//
// HINWEIS: Diese Datei dient NUR der IDE-Unterstützung (Roslyn-Typen).
// Der Ordner 'NetClientStubs/' ist in ProjectReader.s_excludedDirNames
// gelistet und wird NICHT von CS2SX transpiliert.
// Die echten C-Funktionen werden direkt aus externLibs/ mitcompiliert.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace NetClient;

public static class NetClient
{
    public static extern int NetClient_Init();
    public static extern void NetClient_Exit();
    public static extern void NetClient_UseHttps(int enable);
    public static extern void NetClient_SetHeader(string name, string value);
    public static extern void NetClient_ClearHeaders();
    public static extern void NetClient_SetTimeout(int connect_s, int io_s);
    public static extern int NetClient_BeginGet(string host, string path);
    public static extern int NetClient_BeginPost(string host, string path, string body, string content_type);
    public static extern int NetClient_BeginPut(string host, string path, string body, string content_type);
    public static extern int NetClient_BeginDelete(string host, string path);
    public static extern int NetClient_BeginHead(string host, string path);
    public static extern int NetClient_BeginDownload(string host, string path, string savepath);
    public static extern int NetClient_IsComplete();
    public static extern int NetClient_WasSuccess();
    public static extern void NetClient_Finish();
    public static extern string NetClient_GetResponse();
    public static extern int NetClient_GetResponseLen();
    public static extern int NetClient_GetStatusCode();
    public static extern string NetClient_GetRespHeader(string name);
    public static extern string NetClient_GetError();
    public static extern void NetClient_UrlEncode(string input, ref byte output, int outsz);
    public static extern void NetClient_ParseUrl(string url, ref byte host, int hostsz, ref byte path, int pathsz, ref int use_https);
    public static extern string NetClient_JsonStr(string json, string field, ref byte @out, int outsz);
    public static extern int NetClient_JsonInt(string json, string field, int def_val);
    public static extern float NetClient_JsonFloat(string json, string field, float def_val);
    public static extern int NetClient_JsonBool(string json, string field, int def_val);
    public static extern string NetClient_GetWeatherTemp();
    public static extern string NetClient_GetWeatherWind();
    public static extern int NetClient_GetWeatherCode();
    public static extern void NetClient_StoreCityResult(int idx);
    public static extern string NetClient_GetCityDisplay(int idx);
}
