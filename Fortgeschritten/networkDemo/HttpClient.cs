// CS2SX HttpClient — high-level C# wrapper around NetClient v1.1
// Copy this file + externLibs/NetClient/ + net_client_decl.h to any CS2SX project.

using System.Collections.Generic;

public class HttpClient
{
    private bool   _initialized;
    private string _lastError   = "";
    private string _response    = "";
    private bool   _lastSuccess = false;
    private int    _statusCode  = 0;

    public string LastError  => _lastError;
    public string Response   => _response;
    public int    StatusCode => _statusCode;
    public bool   IsReady    => _initialized;
    public bool   IsLoading  { get; private set; }

    // ── Init / Exit ───────────────────────────────────────────────────────────

    public bool Init()
    {
        _initialized = NetClient.Init() != 0;
        if (!_initialized) _lastError = NetClient.GetError();
        return _initialized;
    }

    public void Exit()
    {
        if (_initialized) { NetClient.Exit(); _initialized = false; }
    }

    // ── Configuration ─────────────────────────────────────────────────────────

    public void UseHttps(bool enable) => NetClient.UseHttps(enable ? 1 : 0);
    public void SetHeader(string name, string value) => NetClient.SetHeader(name, value);
    public void ClearHeaders() => NetClient.ClearHeaders();
    public void SetTimeout(int connectSecs, int ioSecs) => NetClient.SetTimeout(connectSecs, ioSecs);

    // ── Async GET / POST / PUT / DELETE / HEAD / Download ─────────────────────

    public bool BeginGet(string host, string path)
        => StartAsync(NetClient.BeginGet(host, path));

    public bool BeginPost(string host, string path, string body,
                          string contentType = "application/json")
        => StartAsync(NetClient.BeginPost(host, path, body, contentType));

    public bool BeginPut(string host, string path, string body,
                         string contentType = "application/json")
        => StartAsync(NetClient.BeginPut(host, path, body, contentType));

    public bool BeginDelete(string host, string path)
        => StartAsync(NetClient.BeginDelete(host, path));

    public bool BeginHead(string host, string path)
        => StartAsync(NetClient.BeginHead(host, path));

    public bool BeginDownload(string host, string path, string savepath)
        => StartAsync(NetClient.BeginDownload(host, path, savepath));

    private bool StartAsync(int ok)
    {
        if (ok != 0) { IsLoading = true; _lastSuccess = false; return true; }
        _lastError = NetClient.GetError();
        return false;
    }

    // ── Polling (call every frame after BeginXxx) ─────────────────────────────

    // Returns true when done (success or fail).
    public bool PollResult()
    {
        if (!IsLoading) return true;
        if (NetClient.IsComplete() == 0) return false;

        IsLoading    = false;
        _lastSuccess = NetClient.WasSuccess() != 0;
        _statusCode  = NetClient.GetStatusCode();
        if (_lastSuccess) _response  = NetClient.GetResponse();
        else              _lastError = NetClient.GetError();
        NetClient.Finish();
        return true;
    }

    public bool WasSuccess() => _lastSuccess;

    // Response header access
    public string GetRespHeader(string name) => NetClient.GetRespHeader(name);

    // ── URL helpers ───────────────────────────────────────────────────────────

    // Parse "https://host/path" → host, path, isHttps
    public void ParseUrl(string url, ref string host, ref string path, ref bool useHttps)
    {
        int start = 0;
        if (url.StartsWith("https://")) { start = 8; useHttps = true; }
        else if (url.StartsWith("http://")) { start = 7; useHttps = false; }
        else useHttps = true;
        int slash = url.IndexOf("/", start);
        if (slash < 0) { host = url.Substring(start); path = "/"; }
        else { host = url.Substring(start, slash - start); path = url.Substring(slash); }
    }

    // ── JSON helpers ──────────────────────────────────────────────────────────

    // These operate on any JSON string.  The value is returned as the matching C# type.
    // No dangling pointers — int/float are value types; JsonString uses a static C buffer.

    public int   JsonInt(string json, string field, int defVal = 0)
        => NetClient.JsonInt(json, field, defVal);

    public float JsonFloat(string json, string field, float defVal = 0)
        => NetClient.JsonFloat(json, field, defVal);

    public bool  JsonBool(string json, string field, bool defVal = false)
        => NetClient.JsonBool(json, field, defVal ? 1 : 0) != 0;

    // NOTE: returned string points to a static C buffer (8 rotating slots).
    // Use immediately — don't store in a class field across frames.
    public string JsonString(string json, string field)
    {
        // Use a fixed-size out buffer approach via the C function
        return NetClient.JsonStr(json, field, "", 256);
    }

    // ── Weather helpers ───────────────────────────────────────────────────────

    public string WmoToText(int code)
    {
        if (code == 0)        return "Klarer Himmel";
        if (code <= 2)        return "Leicht bewoelkt";
        if (code == 3)        return "Bedeckt";
        if (code <= 48)       return "Nebelig";
        if (code <= 57)       return "Nieselregen";
        if (code <= 67)       return "Regen";
        if (code <= 77)       return "Schnee";
        if (code <= 82)       return "Schauer";
        if (code <= 86)       return "Schneeschauer";
        if (code <= 99)       return "Gewitter";
        return "Unbekannt";
    }

    // ── GitHub releases parser ────────────────────────────────────────────────

    public List<RepoEntry> ParseGithubReleases(string json)
    {
        var entries = new List<RepoEntry>();
        int pos = 0;
        while (pos < json.Length)
        {
            int ts = json.IndexOf("\"tag_name\":\"", pos);
            if (ts < 0) break;
            ts += 12;
            int te = json.IndexOf("\"", ts);
            if (te < 0) break;
            string tag = json.Substring(ts, te - ts);

            int us = json.IndexOf("\"zipball_url\":\"", te);
            if (us < 0) break;
            us += 15;
            int ue = json.IndexOf("\"", us);
            if (ue < 0) break;
            string url = json.Substring(us, ue - us);

            var e = new RepoEntry();
            e.Name        = tag;
            e.DownloadUrl = url;
            e.Size        = "GitHub Release";
            entries.Add(e);
            pos = ue + 1;
        }
        return entries;
    }
}
