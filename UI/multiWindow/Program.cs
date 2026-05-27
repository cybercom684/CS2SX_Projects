using multiWindow.UI;
using multiWindow.UI.Controls;

public class multiWindowApp : SwitchApp
{
    private Desktop _desktop;

    // Virtual cursor state
    private int _cursorX = 640;
    private int _cursorY = 360;

    public override void OnInit()
    {
        Graphics.Init(1280, 720);
        _desktop = new Desktop();

        // ── Context menu (long-press on desktop) ──────────────────────────────
        _desktop.ContextMenu.AddItem("Editor oeffnen",  () => OpenEditor());
        _desktop.ContextMenu.AddItem("Statistiken",     () => OpenStats());
        _desktop.ContextMenu.AddItem("Einstellungen",   () => OpenSettings());
        _desktop.ContextMenu.AddItem("Kaskade",         () => _desktop.CascadeWindows());
        _desktop.ContextMenu.AddItem("Nebeneinander",   () => _desktop.TileHorizontal());
        _desktop.ContextMenu.AddItem("Stapeln",         () => _desktop.TileVertical());
        _desktop.ContextMenu.AddItem("Alle minimieren", () => _desktop.MinimizeAll());

        // ── Fenster 1: Info ───────────────────────────────────────────────────
        Window winInfo   = new Window();
        winInfo.Title    = "Info";
        winInfo.X        = 60;
        winInfo.Y        = 60;
        winInfo.Width    = 440;
        winInfo.Height   = 320;
        winInfo.TitleAccentColor = Color.RGB(0, 120, 215);

        LabelControl lblWelcome = new LabelControl();
        lblWelcome.X        = 10;
        lblWelcome.Y        = 14;
        lblWelcome.Text     = "Willkommen!";
        lblWelcome.Color    = Color.White;
        lblWelcome.FontSize = 2;
        winInfo.AddControl(lblWelcome);

        LabelControl lblHint = new LabelControl();
        lblHint.X        = 10;
        lblHint.Y        = 46;
        lblHint.Text     = "Fenster ziehen: Titelleiste halten";
        lblHint.Color    = Color.Gray;
        lblHint.FontSize = 1;
        winInfo.AddControl(lblHint);

        LabelControl lblHint2 = new LabelControl();
        lblHint2.X        = 10;
        lblHint2.Y        = 62;
        lblHint2.Text     = "Snap: an Rand ziehen  |  L: Fenster-Switcher";
        lblHint2.Color    = Color.Gray;
        lblHint2.FontSize = 1;
        winInfo.AddControl(lblHint2);

        ButtonControl btnOpenEditor = new ButtonControl();
        btnOpenEditor.X         = 10;
        btnOpenEditor.Y         = 88;
        btnOpenEditor.Width     = 200;
        btnOpenEditor.Height    = 40;
        btnOpenEditor.Text      = "Editor oeffnen";
        btnOpenEditor.BackColor = Color.RGB(0, 120, 215);
        btnOpenEditor.ForeColor = Color.White;
        btnOpenEditor.OnClick   = () => OpenEditor();
        winInfo.AddControl(btnOpenEditor);

        ButtonControl btnOpenStats = new ButtonControl();
        btnOpenStats.X         = 10;
        btnOpenStats.Y         = 140;
        btnOpenStats.Width     = 200;
        btnOpenStats.Height    = 40;
        btnOpenStats.Text      = "Statistiken";
        btnOpenStats.BackColor = Color.RGB(60, 60, 60);
        btnOpenStats.ForeColor = Color.White;
        btnOpenStats.OnClick   = () => OpenStats();
        winInfo.AddControl(btnOpenStats);

        _desktop.AddWindow(winInfo);

        // ── Startmenü ─────────────────────────────────────────────────────────
        _desktop.StartMenu.AddItem("Info-Fenster",  () => _desktop.AddWindow(winInfo));
        _desktop.StartMenu.AddItem("Editor",        () => OpenEditor());
        _desktop.StartMenu.AddItem("Statistiken",   () => OpenStats());
        _desktop.StartMenu.AddItem("Einstellungen", () => OpenSettings());

        // ── Desktop-Icons ─────────────────────────────────────────────────────
        DesktopIcon iconEditor = new DesktopIcon();
        iconEditor.X         = 20;
        iconEditor.Y         = 20;
        iconEditor.Label     = "Editor";
        iconEditor.IconColor = Color.RGB(0, 120, 215);
        iconEditor.OnTap     = () => OpenEditor();
        _desktop.AddDesktopIcon(iconEditor);

        DesktopIcon iconStats = new DesktopIcon();
        iconStats.X         = 20;
        iconStats.Y         = 94;
        iconStats.Label     = "Stats";
        iconStats.IconColor = Color.RGB(60, 160, 60);
        iconStats.OnTap     = () => OpenStats();
        _desktop.AddDesktopIcon(iconStats);

        DesktopIcon iconSettings = new DesktopIcon();
        iconSettings.X         = 20;
        iconSettings.Y         = 168;
        iconSettings.Label     = "Einstellg.";
        iconSettings.IconColor = Color.RGB(160, 100, 20);
        iconSettings.OnTap     = () => OpenSettings();
        _desktop.AddDesktopIcon(iconSettings);

        // ── Welcome toast ─────────────────────────────────────────────────────
        _desktop.ShowToast("multiWindow", "Desktop geladen");
    }

    public override void OnFrame()
    {
        Graphics.BeginFrame();

        TouchState touch = Input.GetTouch();
        if (!touch.IsTouched)
            touch = UpdateCursor();

        _desktop.Update(touch);
        _desktop.Draw();
        DrawCursor(_cursorX, _cursorY);

        Graphics.EndFrame();
    }

    private TouchState UpdateCursor()
    {
        // D-pad movement (4 px/frame)
        if (Input.IsHeld(NpadButton.Up))    _cursorY -= 4;
        if (Input.IsHeld(NpadButton.Down))  _cursorY += 4;
        if (Input.IsHeld(NpadButton.Left))  _cursorX -= 4;
        if (Input.IsHeld(NpadButton.Right)) _cursorX += 4;

        // Analog left stick (~8 px/frame at full deflection)
        StickPos stick = Input.GetStickLeft();
        _cursorX += stick.x / 4096;
        _cursorY -= stick.y / 4096;  // libnx Y+= up, screen Y+= down

        // Clamp to screen bounds
        if (_cursorX < 0)    _cursorX = 0;
        if (_cursorX > 1279) _cursorX = 1279;
        if (_cursorY < 0)    _cursorY = 0;
        if (_cursorY > 719)  _cursorY = 719;

        // B = open context menu at cursor (like long-press on desktop)
        if (Input.IsDown(NpadButton.B))
        {
            _desktop.ContextMenu.Open(_cursorX, _cursorY);
            _desktop.StartMenu.Close();
        }

        // A held = synthesize touch at cursor position
        TouchState synth = new TouchState();
        if (Input.IsHeld(NpadButton.A))
        {
            synth.count  = 1;
            synth.x[0]   = _cursorX;
            synth.y[0]   = _cursorY;
        }
        return synth;
    }

    private void DrawCursor(int cx, int cy)
    {
        // Black shadow (1px offset)
        Graphics.DrawLine(cx+1, cy+1, cx+1,  cy+13, Color.Black);
        Graphics.DrawLine(cx+1, cy+1, cx+10, cy+10, Color.Black);
        Graphics.DrawLine(cx+1, cy+13, cx+5, cy+9,  Color.Black);
        Graphics.DrawLine(cx+5, cy+9,  cx+10, cy+10, Color.Black);
        // White arrow
        Graphics.DrawLine(cx, cy, cx,  cy+12, Color.White);
        Graphics.DrawLine(cx, cy, cx+9, cy+9, Color.White);
        Graphics.DrawLine(cx, cy+12, cx+4, cy+8, Color.White);
        Graphics.DrawLine(cx+4, cy+8, cx+9, cy+9, Color.White);
    }

    // ── Fenster-Fabriken ──────────────────────────────────────────────────────

    private void OpenEditor()
    {
        Window win = new Window();
        win.Title            = "Editor";
        win.X                = 180;
        win.Y                = 80;
        win.Width            = 500;
        win.Height           = 380;
        win.TitleAccentColor = Color.RGB(0, 120, 215);

        LabelControl lbl = new LabelControl();
        lbl.X        = 10;
        lbl.Y        = 12;
        lbl.Text     = "Text-Editor";
        lbl.Color    = Color.White;
        lbl.FontSize = 2;
        win.AddControl(lbl);

        LabelControl placeholder = new LabelControl();
        placeholder.X        = 10;
        placeholder.Y        = 50;
        placeholder.Text     = "[ Inhalt hier ]";
        placeholder.Color    = Color.Gray;
        placeholder.FontSize = 1;
        win.AddControl(placeholder);

        _desktop.AddWindow(win);
        _desktop.ShowToast("Editor", "Neues Fenster geoeffnet");
    }

    private void OpenStats()
    {
        Window win = new Window();
        win.Title            = "Statistiken";
        win.X                = 300;
        win.Y                = 120;
        win.Width            = 380;
        win.Height           = 280;
        win.TitleAccentColor = Color.RGB(60, 160, 60);

        LabelControl lbl = new LabelControl();
        lbl.X        = 10;
        lbl.Y        = 12;
        lbl.Text     = "Statistiken";
        lbl.Color    = Color.White;
        lbl.FontSize = 2;
        win.AddControl(lbl);

        LabelControl placeholder = new LabelControl();
        placeholder.X        = 10;
        placeholder.Y        = 50;
        placeholder.Text     = "[ Inhalt hier ]";
        placeholder.Color    = Color.Gray;
        placeholder.FontSize = 1;
        win.AddControl(placeholder);

        _desktop.AddWindow(win);
    }

    private void OpenSettings()
    {
        Window win = new Window();
        win.Title            = "Einstellungen";
        win.X                = 400;
        win.Y                = 100;
        win.Width            = 420;
        win.Height           = 340;
        win.TitleAccentColor = Color.RGB(160, 100, 20);

        LabelControl lbl = new LabelControl();
        lbl.X        = 10;
        lbl.Y        = 12;
        lbl.Text     = "Einstellungen";
        lbl.Color    = Color.White;
        lbl.FontSize = 2;
        win.AddControl(lbl);

        LabelControl placeholder = new LabelControl();
        placeholder.X        = 10;
        placeholder.Y        = 50;
        placeholder.Text     = "[ Inhalt hier ]";
        placeholder.Color    = Color.Gray;
        placeholder.FontSize = 1;
        win.AddControl(placeholder);

        _desktop.AddWindow(win);
    }
}
