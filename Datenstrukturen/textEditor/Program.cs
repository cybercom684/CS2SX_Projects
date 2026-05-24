using System.Collections.Generic;
using textEditor.Core.Controller;
using textEditor.Core.UI.Components;
using textEditor.Core.UI.Components.Layout;
using static textEditor.Core.Controller.EditorController;

public class TextEditorApp : SwitchApp
{
    private Keyboard _keyboard;
    private ButtonWidget _addLineButton;
    private InfoPanel _infoPanel;
    private StatusBar _statusBar;

    private EditorController _editorController = new EditorController();
    private List<string> _lines;

    private const string AppTitle = "TextEditor v1.0.0";
    private const int HeaderHeight = 50;
    private const int StatusBarY = 690;
    private const int GutterWidth = 36;
    private const int ContentX = GutterWidth + 10;
    private const int LineHeight = 28;
    private const int MaxVisibleLines = 22; // (688 - 60) / 28

    private int _selectedLine = 0;
    private int _scrollOffset = 0;
    private bool _editMode = false;

    public override void OnInit()
    {
        Graphics.Init(1280, 720);
        _editorController.SetTheme(Themes.Dark);

        _lines = new List<string>
        {
            "Hello World",
            "This is a text editor app",
            "You can add more lines by modifying the code"
        };

        u64 currentTime = 0;
        LibNX.Services.Time.timeGetCurrentTime(
            LibNX.Services.TimeType.TimeType_LocalSystemClock,
            ref currentTime);
        _lines.Add("Current time: " + currentTime);

        _addLineButton = new ButtonWidget(
            1080, 11,
            "+ Add Line (A)",
            _editorController.CurrentTheme.SecondaryColor,
            _editorController.CurrentTheme.PrimaryColor,
            2);

        _infoPanel = new InfoPanel(
            AppTitle,
            "Use D-Pad/Touch to select · A to edit · X to add line",
            _editorController.CurrentTheme.PrimaryColor,
            _editorController.CurrentTheme.BackgroundColor,
            _editorController.CurrentTheme.ForegroundColor);

        _statusBar = new StatusBar(
            StatusBarY,
            _editorController.CurrentTheme.HighlightColor,
            _editorController.CurrentTheme.AccentColor,
            _editorController.CurrentTheme.ForegroundColor);

        _keyboard = new Keyboard();
    }

    public override void OnFrame()
    {
        // --- Input ---
        TouchState touch = Input.GetTouch();

        // Zeile per Touch wählen
        if (touch.IsTouched && !_editMode)
        {
            int touchY = touch.Y0;
            if (touchY >= HeaderHeight && touchY < StatusBarY)
            {
                int tappedIndex = (touchY - HeaderHeight - 10) / LineHeight + _scrollOffset;
                if (tappedIndex >= 0 && tappedIndex < _lines.Count)
                {
                    if (tappedIndex == _selectedLine)
                    {
                        // Doppeltap-Simulation: gleiche Zeile nochmal → direkt editieren
                        StartEdit();
                    }
                    else
                    {
                        _selectedLine = tappedIndex;
                    }
                }
            }
        }

        // D-Pad Navigation
        if (!_editMode)
        {
            if (Input.IsDown(NpadButton.Down))
            {
                if (_selectedLine < _lines.Count - 1)
                {
                    _selectedLine++;
                    if (_selectedLine >= _scrollOffset + MaxVisibleLines)
                        _scrollOffset++;
                }
            }
            if (Input.IsDown(NpadButton.Up))
            {
                if (_selectedLine > 0)
                {
                    _selectedLine--;
                    if (_selectedLine < _scrollOffset)
                        _scrollOffset--;
                }
            }

            // A → ausgewählte Zeile bearbeiten
            if (Input.IsDown(NpadButton.A))
                StartEdit();

            // X → neue Zeile hinzufügen
            if (Input.IsDown(NpadButton.X) || _addLineButton.HandleInput(Input.IsHeld(NpadButton.A)))
                StartAdd();

            // B → ausgewählte Zeile löschen
            if (Input.IsDown(NpadButton.B) && _lines.Count > 1)
            {
                _lines.RemoveAt(_selectedLine);
                if (_selectedLine >= _lines.Count)
                    _selectedLine = _lines.Count - 1;
                if (_scrollOffset > 0 && _scrollOffset + MaxVisibleLines > _lines.Count)
                    _scrollOffset--;
                _infoPanel.Update(AppTitle, "Line deleted.",
                    _editorController.CurrentTheme.AccentColor,
                    _editorController.CurrentTheme.BackgroundColor,
                    _editorController.CurrentTheme.ForegroundColor);
            }
        }

        // Keyboard-Ergebnis auswerten
        if (_keyboard.WasConfirmed())
        {
            string result = _keyboard.GetBuffer();
            if (!string.IsNullOrWhiteSpace(result))
            {
                if (_editMode)
                {
                    _lines[_selectedLine] = result;
                    _infoPanel.Update(AppTitle, "Line updated.",
                        _editorController.CurrentTheme.PrimaryColor,
                        _editorController.CurrentTheme.BackgroundColor,
                        _editorController.CurrentTheme.ForegroundColor);
                }
                else
                {
                    _lines.Add(result);
                    _selectedLine = _lines.Count - 1;
                    _scrollOffset = _selectedLine >= MaxVisibleLines ? _selectedLine - MaxVisibleLines + 1 : 0;
                    _infoPanel.Update(AppTitle, "Line added.",
                        _editorController.CurrentTheme.PrimaryColor,
                        _editorController.CurrentTheme.BackgroundColor,
                        _editorController.CurrentTheme.ForegroundColor);
                }
            }
            _editMode = false;
        }

        // --- Draw ---
        Graphics.FillScreen(_editorController.CurrentTheme.BackgroundColor);
        Graphics.DrawRect(1, 1, 1278, 718, _editorController.CurrentTheme.AccentColor);

        Graphics.FillRect(1, HeaderHeight, GutterWidth, 638, _editorController.CurrentTheme.HighlightColor);
        Graphics.DrawLine(GutterWidth + 1, HeaderHeight, GutterWidth + 1, 688, _editorController.CurrentTheme.AccentColor);

        _keyboard.Update();
        _keyboard.Draw();
        _infoPanel.Draw();

        int visibleCount = _lines.Count - _scrollOffset;
        if (visibleCount > MaxVisibleLines) visibleCount = MaxVisibleLines;

        for (int i = 0; i < visibleCount; i++)
        {
            int absIndex = i + _scrollOffset;
            int lineY = HeaderHeight + 10 + i * LineHeight;
            bool isSelected = absIndex == _selectedLine;

            // Selektions-Highlight
            if (isSelected)
                Graphics.FillRect(GutterWidth + 2, lineY - 2, 1274 - GutterWidth, LineHeight,
                    _editorController.CurrentTheme.HighlightColor);

            // Zeilennummer
            Graphics.DrawText(6, lineY, (absIndex + 1).ToString(),
                isSelected
                    ? _editorController.CurrentTheme.PrimaryColor
                    : _editorController.CurrentTheme.SecondaryColor, 1);

            // Zeileninhalt
            uint lineColor = isSelected
                ? _editorController.CurrentTheme.PrimaryColor
                : (absIndex == 0)
                    ? _editorController.CurrentTheme.AccentColor
                    : _editorController.CurrentTheme.ForegroundColor;

            Graphics.DrawText(ContentX, lineY, _lines[absIndex], lineColor, 2);
        }

        _statusBar.Draw("L:" + (_selectedLine + 1) + "/" + _lines.Count + "  Up/Down · A=Edit · B=Del · X=Add");
        _addLineButton.Draw(Input.IsHeld(NpadButton.A));
    }

    private void StartEdit()
    {
        _editMode = true;
        _keyboard.Show(_lines[_selectedLine]);
        _infoPanel.Update(AppTitle, "Editing line " + (_selectedLine + 1) + ":",
            _editorController.CurrentTheme.AccentColor,
            _editorController.CurrentTheme.BackgroundColor,
            _editorController.CurrentTheme.ForegroundColor);
    }

    private void StartAdd()
    {
        _editMode = false;
        _keyboard.Show("");
        _infoPanel.Update(AppTitle, "Enter a new line:",
            _editorController.CurrentTheme.AccentColor,
            _editorController.CurrentTheme.BackgroundColor,
            _editorController.CurrentTheme.ForegroundColor);
    }
}