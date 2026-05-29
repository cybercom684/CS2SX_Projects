using System.Collections.Generic;

// Wetter-Demo — zeigt aktuelles Wetter fuer deutsche Staedte via Open-Meteo API.
// Kein API-Key noetig, plain HTTP (Port 80) → kein SSL-Problem.

public class networkDemoApp : SwitchApp
{
    // Open-Meteo unterstuetzt auch plain HTTP
    private const string WEATHER_HOST = "api.open-meteo.com";

    private enum NetDemoState { Idle, Loading, ProcessResult, Done, Downloading, ProcessDownload, Error }

    private HttpClient   _http     = new HttpClient();
    private ScrollList   _list     = new ScrollList();
    private NetDemoState _state    = NetDemoState.Idle;
    private string       _status   = "";
    private string       _errorMsg = "";
    private int          _spinFrame  = 0;
    private int          _loadingFor = -1;
    private bool         _loadingAll = false;

    // ── Staedte ───────────────────────────────────────────────────────────────

    private void AddCity(List<RepoEntry> list, string name, string path)
    {
        RepoEntry e = new RepoEntry();
        e.Name        = name;
        e.DownloadUrl = path;       // string literal — kein dangling pointer
        e.Size        = "(nicht geladen)";
        e.Downloaded  = false;
        list.Add(e);
    }

    private void InitCities()
    {
        var cities = new List<RepoEntry>();
        AddCity(cities, "Berlin",      "/v1/forecast?latitude=52.52&longitude=13.41&current_weather=true&temperature_unit=celsius&windspeed_unit=kmh&timezone=Europe%2FBerlin");
        AddCity(cities, "Muenchen",    "/v1/forecast?latitude=48.14&longitude=11.58&current_weather=true&temperature_unit=celsius&windspeed_unit=kmh&timezone=Europe%2FBerlin");
        AddCity(cities, "Hamburg",     "/v1/forecast?latitude=53.55&longitude=9.99&current_weather=true&temperature_unit=celsius&windspeed_unit=kmh&timezone=Europe%2FBerlin");
        AddCity(cities, "Koeln",       "/v1/forecast?latitude=50.94&longitude=6.96&current_weather=true&temperature_unit=celsius&windspeed_unit=kmh&timezone=Europe%2FBerlin");
        AddCity(cities, "Frankfurt",   "/v1/forecast?latitude=50.11&longitude=8.68&current_weather=true&temperature_unit=celsius&windspeed_unit=kmh&timezone=Europe%2FBerlin");
        AddCity(cities, "Stuttgart",   "/v1/forecast?latitude=48.78&longitude=9.18&current_weather=true&temperature_unit=celsius&windspeed_unit=kmh&timezone=Europe%2FBerlin");
        AddCity(cities, "Duesseldorf", "/v1/forecast?latitude=51.22&longitude=6.77&current_weather=true&temperature_unit=celsius&windspeed_unit=kmh&timezone=Europe%2FBerlin");
        AddCity(cities, "Leipzig",     "/v1/forecast?latitude=51.34&longitude=12.37&current_weather=true&temperature_unit=celsius&windspeed_unit=kmh&timezone=Europe%2FBerlin");
        AddCity(cities, "Dresden",     "/v1/forecast?latitude=51.05&longitude=13.74&current_weather=true&temperature_unit=celsius&windspeed_unit=kmh&timezone=Europe%2FBerlin");
        AddCity(cities, "Nuernberg",   "/v1/forecast?latitude=49.45&longitude=11.08&current_weather=true&temperature_unit=celsius&windspeed_unit=kmh&timezone=Europe%2FBerlin");
        _list.SetItems(cities);
    }

    // ── Init / Exit ───────────────────────────────────────────────────────────

    public override void OnInit()
    {
        Graphics.Init(1280, 720);

        if (!_http.Init())
        {
            _state    = NetDemoState.Error;
            _errorMsg = "Netzwerk-Init: " + _http.LastError;
            return;
        }

        _list.VisibleRows = 9;
        _list.Y           = 130;
        _list.RowHeight   = 52;
        _list.Width       = 1220;

        InitCities();
        _status = "A: Wetter laden  |  B: Alle Staedte laden  |  +: Beenden";
        _state  = NetDemoState.Done; // list is already filled with cities
    }

    public override void OnExit()
    {
        _http.Exit();
    }

    // ── Frame ─────────────────────────────────────────────────────────────────

    public override void OnFrame()
    {
        Graphics.BeginFrame();
        Graphics.FillRect(0, 0, 1280, 720, Color.RGB(10, 14, 26));

        if (_state == NetDemoState.Done)          { UpdateDone();           DrawDone();    }
        if (_state == NetDemoState.Loading)        { UpdateLoading();        DrawLoading(); }
        if (_state == NetDemoState.ProcessResult)  { UpdateProcessResult();  DrawDone();    }
        if (_state == NetDemoState.Error)          { UpdateError();          DrawError();   }

        DrawStatusBar();
        Graphics.EndFrame();
    }

    // ── Done (Stadtliste) ─────────────────────────────────────────────────────

    private void UpdateDone()
    {
        _list.Update();

        // A = eine Stadt laden
        if (Input.IsDown(NpadButton.A) && _list.Count > 0)
        {
            _loadingFor = _list.SelectedIndex;
            _loadingAll = false;
            RepoEntry city = _list.SelectedItem;
            bool ok = _http.BeginGet(WEATHER_HOST, city.DownloadUrl);
            if (ok) { _state = NetDemoState.Loading; _spinFrame = 0; _status = "Lade " + city.Name + "..."; }
            else    { _status = "Fehler: " + _http.LastError; }
        }

        // B = alle Staedte nacheinander laden
        if (Input.IsDown(NpadButton.B) && _list.Count > 0)
        {
            _loadingFor = 0;
            _loadingAll = true;
            RepoEntry first = _list.GetItem(0);
            bool ok = _http.BeginGet(WEATHER_HOST, first.DownloadUrl);
            if (ok) { _state = NetDemoState.Loading; _spinFrame = 0; _status = "Lade alle... (1/" + _list.Count + ")"; }
        }
    }

    private void DrawDone()
    {
        DrawHeader("Deutsches Wetter", Color.RGB(20, 80, 160));

        // Legende
        Graphics.DrawText(40, 58, "Stadt", Color.Gray, 1);
        Graphics.DrawText(700, 58, "Wetter", Color.Gray, 1);
        Graphics.DrawLine(40, 76, 1240, 76, Color.RGB(40, 40, 70));

        _list.Draw();
        DrawHints("A: Wetter laden  |  B: Alle laden  |  Up/Down: Navigation  |  +: Beenden");
    }

    // ── Loading ───────────────────────────────────────────────────────────────

    private void UpdateLoading()
    {
        _spinFrame++;
        if (_http.PollResult())
            _state = NetDemoState.ProcessResult;
    }

    private void UpdateProcessResult()
    {
        if (!_http.WasSuccess())
        {
            _status = "Fehler bei " + (_loadingFor >= 0 ? _list.GetItem(_loadingFor).Name : "?")
                    + ": " + _http.LastError;
            _state = NetDemoState.Done;
            return;
        }

        if (_loadingFor >= 0 && _loadingFor < _list.Count)
        {
            // Freeze per-city result into persistent C buffer
            NetClient.StoreCityResult(_loadingFor);
            RepoEntry city = _list.GetItem(_loadingFor);
            // Size points to s_city_display[idx] — persistent static C array, not arena
            city.Size       = NetClient.GetCityDisplay(_loadingFor);
            city.Downloaded = true;
            _status = city.Name + ": " + city.Size;
        }

        // Continue loading remaining cities only when B ("load all") was used
        if (_loadingAll)
        {
            int nextUnloaded = -1;
            for (int i = _loadingFor + 1; i < _list.Count; i++)
            {
                if (_list.GetItem(i).Downloaded == false) { nextUnloaded = i; break; }
            }
            if (nextUnloaded >= 0)
            {
                _loadingFor = nextUnloaded;
                RepoEntry nextCity = _list.GetItem(nextUnloaded);
                bool ok = _http.BeginGet(WEATHER_HOST, nextCity.DownloadUrl);
                if (ok)
                {
                    int loaded = 0;
                    for (int i = 0; i < _list.Count; i++)
                        if (_list.GetItem(i).Downloaded) loaded++;
                    _state = NetDemoState.Loading; _spinFrame = 0;
                    _status = "Lade alle... (" + loaded + "/" + _list.Count + ")";
                    return;
                }
            }
            _loadingAll = false;
        }

        int totalLoaded = 0;
        for (int i = 0; i < _list.Count; i++)
            if (_list.GetItem(i).Downloaded) totalLoaded++;
        _status = totalLoaded + "/" + _list.Count + " Staedte geladen";
        _state  = NetDemoState.Done;
    }

    private void DrawLoading()
    {
        DrawHeader("Lade Wetterdaten...", Color.RGB(40, 100, 180));
        string city = _loadingFor >= 0 ? _list.GetItem(_loadingFor).Name : "?";
        int d = (_spinFrame / 12) % 4;
        string dots = d == 0 ? "" : d == 1 ? "." : d == 2 ? ".." : "...";
        Graphics.DrawText(40, 200, "Wetter fuer: " + city + dots, Color.White, 2);
        Graphics.DrawText(40, 240, WEATHER_HOST, Color.Gray, 1);
        DrawSpinner(640, 400, _spinFrame, Color.RGB(60, 140, 255));
    }

    // ── Error ─────────────────────────────────────────────────────────────────

    private void UpdateError()
    {
        if (Input.IsDown(NpadButton.B))
        {
            _state    = NetDemoState.Done;
            _errorMsg = "";
        }
    }

    private void DrawError()
    {
        DrawHeader("Fehler", Color.RGB(180, 40, 40));
        int y = 200;
        string msg = _errorMsg;
        while (msg.Length > 0 && y < 600)
        {
            int cut = msg.Length <= 90 ? msg.Length : 90;
            Graphics.DrawText(40, y, msg.Substring(0, cut), Color.White, 1);
            msg = msg.Length <= 90 ? "" : msg.Substring(90);
            y  += 22;
        }
        Graphics.DrawText(40, 640, "B = Zurueck", Color.Gray, 1);
    }

    // ── Shared UI ─────────────────────────────────────────────────────────────

    private void DrawHeader(string title, uint color)
    {
        Graphics.FillRect(0, 0, 1280, 52, color);
        int th = Graphics.MeasureTextHeight(2);
        Graphics.DrawText(20, (52 - th) / 2, title, Color.White, 2);
    }

    private void DrawStatusBar()
    {
        Graphics.FillRect(0, 686, 1280, 34, Color.RGB(15, 15, 28));
        Graphics.DrawLine(0, 686, 1280, 686, Color.RGB(40, 40, 65));
        int th = Graphics.MeasureTextHeight(1);
        int ty = 686 + (34 - th) / 2;
        Graphics.DrawText(12, ty, _status, Color.RGB(170, 170, 190), 1);
        string net = _http.IsLoading ? "Laedt..." : (_http.IsReady ? "NET OK" : "NET --");
        uint   nc  = _http.IsLoading ? Color.RGB(220, 180, 40) : (_http.IsReady ? Color.RGB(80, 220, 80) : Color.RGB(200, 80, 80));
        int    nw  = Graphics.MeasureTextWidth(net, 1);
        Graphics.DrawText(1280 - nw - 12, ty, net, nc, 1);
    }

    private void DrawHints(string h)
    {
        int th = Graphics.MeasureTextHeight(1);
        Graphics.DrawText(40, 686 - th - 4, h, Color.RGB(80, 80, 110), 1);
    }

    private void DrawSpinner(int cx, int cy, int f, uint color)
    {
        int r = 30;
        float a = f * 0.13f;
        Graphics.DrawCircle(cx, cy, r, Color.RGB(25, 30, 50));
        Graphics.DrawLine(cx + (int)(r * Math.Sin(a)), cy - (int)(r * Math.Cos(a)),
                          cx - (int)(r * Math.Sin(a)), cy + (int)(r * Math.Cos(a)), color);
    }
}
