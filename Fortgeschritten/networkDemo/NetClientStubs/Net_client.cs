// CS2SX NetClient v1.1 stubs — IDE support only, not transpiled.
// NetClient.Method() → NetClient_Method() in generated C.

#pragma warning disable CS0626, CS0649, CS0169, CS8981, CS1591

namespace NetClient;

public static class NetClient
{
    // Init / Exit
    public static extern int    Init();
    public static extern void   Exit();

    // Config
    public static extern void   UseHttps(int enable);
    public static extern void   SetHeader(string name, string value);
    public static extern void   ClearHeaders();
    public static extern void   SetTimeout(int connect_s, int io_s);

    // Async requests
    public static extern int    BeginGet(string host, string path);
    public static extern int    BeginPost(string host, string path, string body, string contentType);
    public static extern int    BeginPut(string host, string path, string body, string contentType);
    public static extern int    BeginDelete(string host, string path);
    public static extern int    BeginHead(string host, string path);
    public static extern int    BeginDownload(string host, string path, string savepath);

    // Poll
    public static extern int    IsComplete();
    public static extern int    WasSuccess();
    public static extern void   Finish();

    // Response
    public static extern string GetResponse();
    public static extern int    GetResponseLen();
    public static extern int    GetStatusCode();
    public static extern string GetRespHeader(string name);
    public static extern string GetError();

    // URL helpers
    public static extern void   UrlEncode(string input, string output, int outsz);
    public static extern void   ParseUrl(string url, string host, int hostsz, string path, int pathsz, ref int useHttps);

    // JSON helpers
    public static extern string JsonStr(string json, string field, string outbuf, int outsz);
    public static extern int    JsonInt(string json, string field, int defVal);
    public static extern float  JsonFloat(string json, string field, float defVal);
    public static extern int    JsonBool(string json, string field, int defVal);

    // Weather / per-city
    public static extern string GetWeatherTemp();
    public static extern string GetWeatherWind();
    public static extern int    GetWeatherCode();
    public static extern void   StoreCityResult(int idx);
    public static extern string GetCityDisplay(int idx);

    // Legacy
    public static extern int    StartRequest(string host, string path);
}
