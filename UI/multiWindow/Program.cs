using multiWindow.UI;
using multiWindow.UI.Controls;

public class multiWindowApp : SwitchApp
{
    private Desktop _desktop;

    public override void OnInit()
    {
        Graphics.Init(1280, 720);
        _desktop = new Desktop();

        _desktop.ContextMenu.AddItem("Editor oeffnen", () => OpenEditor());
        _desktop.ContextMenu.AddItem("Statistiken", () => OpenStats());
        _desktop.ContextMenu.AddItem("Einstellungen", () => OpenSettings());
        _desktop.ContextMenu.AddItem("Fenster schliessen", () => { /* optional */ });

        // ── Fenster 1: Info-Fenster ───────────────────────────────────────────
        Window winInfo = new Window();
        winInfo.Title  = "Info";
        winInfo.X      = 60;
        winInfo.Y      = 60;
        winInfo.Width  = 440;
        winInfo.Height = 320;

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

        ButtonControl btnOpenEditor = new ButtonControl();
        btnOpenEditor.X         = 10;
        btnOpenEditor.Y         = 80;
        btnOpenEditor.Width     = 200;
        btnOpenEditor.Height    = 40;
        btnOpenEditor.Text      = "Editor oeffnen";
        btnOpenEditor.BackColor = Color.RGB(0, 120, 215);
        btnOpenEditor.ForeColor = Color.White;
        btnOpenEditor.OnClick   = () => OpenEditor();
        winInfo.AddControl(btnOpenEditor);

        ButtonControl btnOpenStats = new ButtonControl();
        btnOpenStats.X         = 10;
        btnOpenStats.Y         = 132;
        btnOpenStats.Width     = 200;
        btnOpenStats.Height    = 40;
        btnOpenStats.Text      = "Statistiken";
        btnOpenStats.BackColor = Color.RGB(60, 60, 60);
        btnOpenStats.ForeColor = Color.White;
        btnOpenStats.OnClick   = () => OpenStats();
        winInfo.AddControl(btnOpenStats);

        _desktop.AddWindow(winInfo);

        // ── Startmenü-Einträge ────────────────────────────────────────────────
        _desktop.StartMenu.AddItem("Info-Fenster",  () => _desktop.AddWindow(winInfo));
        _desktop.StartMenu.AddItem("Editor",        () => OpenEditor());
        _desktop.StartMenu.AddItem("Statistiken",   () => OpenStats());
        _desktop.StartMenu.AddItem("Einstellungen", () => OpenSettings());
    }

    public override void OnFrame()
    {
        Graphics.BeginFrame();

        TouchState touch = Input.GetTouch();
        _desktop.Update(touch);
        _desktop.Draw();

        Graphics.EndFrame();
    }

    // ── Fenster-Fabriken (Inhalt hier befüllen) ───────────────────────────────

    private void OpenEditor()
    {
        Window win = new Window();
        win.Title  = "Editor";
        win.X      = 180;
        win.Y      = 80;
        win.Width  = 500;
        win.Height = 380;

        LabelControl lbl = new LabelControl();
        lbl.X        = 10;
        lbl.Y        = 12;
        lbl.Text     = "Text-Editor";
        lbl.Color    = Color.White;
        lbl.FontSize = 2;
        win.AddControl(lbl);

        // Hier eigenen Inhalt einfuegen
        LabelControl placeholder = new LabelControl();
        placeholder.X        = 10;
        placeholder.Y        = 50;
        placeholder.Text     = "[ Inhalt hier ]";
        placeholder.Color    = Color.Gray;
        placeholder.FontSize = 1;
        win.AddControl(placeholder);

        _desktop.AddWindow(win);
    }

    private void OpenStats()
    {
        Window win = new Window();
        win.Title  = "Statistiken";
        win.X      = 300;
        win.Y      = 120;
        win.Width  = 380;
        win.Height = 280;

        LabelControl lbl = new LabelControl();
        lbl.X        = 10;
        lbl.Y        = 12;
        lbl.Text     = "Statistiken";
        lbl.Color    = Color.White;
        lbl.FontSize = 2;
        win.AddControl(lbl);

        // Hier eigenen Inhalt einfuegen
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
        win.Title  = "Einstellungen";
        win.X      = 400;
        win.Y      = 100;
        win.Width  = 420;
        win.Height = 340;

        LabelControl lbl = new LabelControl();
        lbl.X        = 10;
        lbl.Y        = 12;
        lbl.Text     = "Einstellungen";
        lbl.Color    = Color.White;
        lbl.FontSize = 2;
        win.AddControl(lbl);

        // Hier eigenen Inhalt einfuegen
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
