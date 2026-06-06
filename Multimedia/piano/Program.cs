using piano.Core.GUI;
using piano.Core.Models;

public class PianoApp : SwitchApp
{
    // Screen: 1280x720
    private const int SCREEN_W = 1280;
    private const int SCREEN_H = 720;

    // 2-octave layout: 14 white keys × 88 px = 1232 px (fits 1280)
    private const int NUM_OCTAVES     = 2;
    private const int WHITE_KEY_COUNT = 14;   // 7 × NUM_OCTAVES
    private const int WHITE_KEY_W     = 88;
    private const int WHITE_KEY_H     = 255;
    private const int BLACK_KEY_W     = 50;
    private const int BLACK_KEY_H     = 155;
    private const int KEYBOARD_Y      = SCREEN_H - WHITE_KEY_H - 36;
    private const int KEYBOARD_X      = (SCREEN_W - WHITE_KEY_COUNT * WHITE_KEY_W) / 2;

    private Theme _theme;
    private PianoKeyButton[] _keys;
    private OctaveSelector _octaveSelector;
    private SynthPanel _synthPanel;
    private int _currentOctave = -1;

    // Touch tracking — IDs and which MIDI note each finger is holding
    private uint[] _prevTouchIds;
    private int[]  _prevTouchMidi;
    private int[]  _newMidi;
    private int    _prevTouchCount = 0;

    private const int PANEL_BOTTOM = 278; // == PANEL_BOTTOM

    // Chromatic index of each white key within an octave: C=0 D=2 E=4 F=5 G=7 A=9 B=11
    private static readonly int[] WhiteKeyOffsets  = { 0, 2, 4, 5, 7, 9, 11 };
    // Black keys: position = after white key wi, chromatic index chrIdx
    private static readonly int[] BlackKeyWhiteIdx  = { 0, 1, 3, 4, 5 };
    private static readonly int[] BlackKeyChrOffset = { 1, 3, 6, 8, 10 };

    private Theme BuildDarkTheme()
    {
        return new Theme
        {
            Name = "Dark",
            BackgroundColor = Color.RGB(18, 18, 24),
            ForegroundColor = Color.RGB(60, 60, 80),
            AccentColor = Color.RGB(120, 80, 220),
            PianoPrimaryKeyColor = Color.RGB(240, 238, 250),
            PianoPrimaryKeyTextColor = Color.RGB(80, 60, 120),
            PianoSecondaryKeyColor = Color.RGB(22, 18, 35),
            PianoSecondaryKeyTextColor = Color.RGB(180, 160, 220),
        };
    }

    private void BuildKeyboard(int octave)
    {
        Audio.ReleaseAll();
        _currentOctave = octave;
        _keys = new PianoKeyButton[12 * NUM_OCTAVES];

        for (int oct = 0; oct < NUM_OCTAVES; oct++)
        {
            PianoKey[] octaveKeys = PianoEngine.GenerateOctave(octave + oct);
            int keyBase = oct * 12;
            int xBase   = oct * 7;   // white-key column offset for this octave

            // White keys
            for (int i = 0; i < 7; i++)
            {
                int chrIdx = WhiteKeyOffsets[i];
                var btn = new PianoKeyButton(octaveKeys[chrIdx], _theme)
                {
                    X      = KEYBOARD_X + (xBase + i) * WHITE_KEY_W,
                    Y      = KEYBOARD_Y,
                    Width  = WHITE_KEY_W - 2,
                    Height = WHITE_KEY_H,
                };
                _keys[keyBase + chrIdx] = btn;
            }

            // Black keys (drawn on top of white keys)
            for (int i = 0; i < 5; i++)
            {
                int wi     = BlackKeyWhiteIdx[i];
                int chrIdx = BlackKeyChrOffset[i];
                int bx     = KEYBOARD_X + (xBase + wi) * WHITE_KEY_W + WHITE_KEY_W - BLACK_KEY_W / 2;
                var btn = new PianoKeyButton(octaveKeys[chrIdx], _theme)
                {
                    X      = bx,
                    Y      = KEYBOARD_Y,
                    Width  = BLACK_KEY_W,
                    Height = BLACK_KEY_H,
                };
                _keys[keyBase + chrIdx] = btn;
            }
        }
    }

    public override void OnInit()
    {
        Graphics.Init(SCREEN_W, SCREEN_H);
        Audio.Init(48000);

        _theme = BuildDarkTheme();

        _octaveSelector = new OctaveSelector();
        _octaveSelector.SetPosition(
            KEYBOARD_X, KEYBOARD_Y - 56, 44, 34,
            _theme.AccentColor, _theme.PianoPrimaryKeyColor
        );

        _synthPanel = new SynthPanel();
        _synthPanel.Init(_theme);

        BuildKeyboard(_octaveSelector.Octave);

        _prevTouchIds   = new uint[10];
        _prevTouchMidi  = new int[10];
        _newMidi        = new int[10];
        for (int i = 0; i < 10; i++) { _prevTouchMidi[i] = -1; _newMidi[i] = -1; }
        _prevTouchCount = 0;
    }

    public override void OnFrame()
    {
        // ── Octave buttons (L/R) ──────────────────────────────────────────────
        _octaveSelector.HandleButtons();
        if (_octaveSelector.Octave != _currentOctave)
            BuildKeyboard(_octaveSelector.Octave);

        // ── Touch processing ──────────────────────────────────────────────────
        TouchState touch = Input.GetTouch();

        for (int i = 0; i < 10; i++) _newMidi[i] = -1;

        // Step 1: Detect lifted fingers → release their notes
        for (int p = 0; p < _prevTouchCount; p++)
        {
            bool stillDown = false;
            for (int t = 0; t < touch.count; t++)
                if (touch.id[t] == _prevTouchIds[p]) { stillDown = true; break; }
            if (!stillDown && _prevTouchMidi[p] >= 0)
                Audio.ReleaseNote(_prevTouchMidi[p]);
        }

        // Step 2: Route ongoing touches in the synth panel area (for dragging sliders)
        for (int t = 0; t < touch.count; t++)
            if (touch.y[t] < PANEL_BOTTOM)
                _synthPanel.HandleTouchAt(touch.x[t], touch.y[t]);

        // Step 3: Dispatch each touch
        for (int t = 0; t < touch.count; t++)
        {
            int tx = touch.x[t];
            int ty = touch.y[t];

            // Determine if this touch was already active last frame
            bool inPrev = false;
            int  inheritedMidi = -1;
            for (int p = 0; p < _prevTouchCount; p++)
            {
                if (_prevTouchIds[p] == touch.id[t])
                {
                    inPrev = true;
                    inheritedMidi = _prevTouchMidi[p];
                    break;
                }
            }

            if (inPrev)
            {
                // Ongoing — keep its MIDI note assignment
                _newMidi[t] = inheritedMidi;
            }
            else
            {
                // New touch — dispatch by region (panel handled in step 2)
                if (ty >= PANEL_BOTTOM && ty < KEYBOARD_Y)
                {
                    _octaveSelector.HandleTouchAt(tx, ty);
                }
                else if (ty >= KEYBOARD_Y)
                {
                    // Piano keys — black keys have priority over white
                    bool hit = false;
                    for (int i = 0; i < _keys.Length && !hit; i++)
                        if (_keys[i] != null && _keys[i].IsBlack && _keys[i].HitTest(tx, ty))
                        { _keys[i].Press(); _newMidi[t] = _keys[i].MidiKey; Audio.PlayNote(_keys[i].MidiKey, 100); hit = true; }
                    if (!hit)
                        for (int i = 0; i < _keys.Length && !hit; i++)
                            if (_keys[i] != null && !_keys[i].IsBlack && _keys[i].HitTest(tx, ty))
                            { _keys[i].Press(); _newMidi[t] = _keys[i].MidiKey; Audio.PlayNote(_keys[i].MidiKey, 100); hit = true; }
                }
            }
        }

        // Step 4: Save touch state for next frame
        _prevTouchCount = touch.count;
        for (int t = 0; t < touch.count; t++)
        {
            _prevTouchIds[t]  = touch.id[t];
            _prevTouchMidi[t] = _newMidi[t];
        }

        // ── Button shortcuts: press on down, release on up ────────────────────
        if (Input.IsDown(NpadButton.A)     && _keys[4]  != null) { _keys[4].Press();  Audio.PlayNote(_keys[4].MidiKey,  100); }
        if (Input.IsDown(NpadButton.B)     && _keys[2]  != null) { _keys[2].Press();  Audio.PlayNote(_keys[2].MidiKey,  100); }
        if (Input.IsDown(NpadButton.X)     && _keys[7]  != null) { _keys[7].Press();  Audio.PlayNote(_keys[7].MidiKey,  100); }
        if (Input.IsDown(NpadButton.Y)     && _keys[9]  != null) { _keys[9].Press();  Audio.PlayNote(_keys[9].MidiKey,  100); }
        if (Input.IsDown(NpadButton.Up)    && _keys[0]  != null) { _keys[0].Press();  Audio.PlayNote(_keys[0].MidiKey,  100); }
        if (Input.IsDown(NpadButton.Down)  && _keys[5]  != null) { _keys[5].Press();  Audio.PlayNote(_keys[5].MidiKey,  100); }
        if (Input.IsDown(NpadButton.Left)  && _keys[11] != null) { _keys[11].Press(); Audio.PlayNote(_keys[11].MidiKey, 100); }
        if (Input.IsDown(NpadButton.Right) && _keys[6]  != null) { _keys[6].Press();  Audio.PlayNote(_keys[6].MidiKey,  100); }

        if (Input.IsUp(NpadButton.A)     && _keys[4]  != null) Audio.ReleaseNote(_keys[4].MidiKey);
        if (Input.IsUp(NpadButton.B)     && _keys[2]  != null) Audio.ReleaseNote(_keys[2].MidiKey);
        if (Input.IsUp(NpadButton.X)     && _keys[7]  != null) Audio.ReleaseNote(_keys[7].MidiKey);
        if (Input.IsUp(NpadButton.Y)     && _keys[9]  != null) Audio.ReleaseNote(_keys[9].MidiKey);
        if (Input.IsUp(NpadButton.Up)    && _keys[0]  != null) Audio.ReleaseNote(_keys[0].MidiKey);
        if (Input.IsUp(NpadButton.Down)  && _keys[5]  != null) Audio.ReleaseNote(_keys[5].MidiKey);
        if (Input.IsUp(NpadButton.Left)  && _keys[11] != null) Audio.ReleaseNote(_keys[11].MidiKey);
        if (Input.IsUp(NpadButton.Right) && _keys[6]  != null) Audio.ReleaseNote(_keys[6].MidiKey);

        Audio.Update();

        // Update key visuals
        for (int i = 0; i < _keys.Length; i++)
            _keys[i]?.Update();

        // ── Render ────────────────────────────────────────────────────────────
        Graphics.BeginFrame();
        Graphics.FillScreen(_theme.BackgroundColor);

        DrawHeader();
        _synthPanel.Draw();

        // White keys first, then black on top
        for (int i = 0; i < _keys.Length; i++)
            if (_keys[i] != null && !_keys[i].IsBlack) _keys[i].Draw();
        for (int i = 0; i < _keys.Length; i++)
            if (_keys[i] != null && _keys[i].IsBlack) _keys[i].Draw();

        _octaveSelector.Draw();
        DrawHints();

        Graphics.EndFrame();
    }

    private void DrawHeader()
    {
        Graphics.DrawText(SCREEN_W / 2 - 80, 20, "PIANO", _theme.AccentColor, 3);
        string octStr = "CS2SX  Okt " + _currentOctave + " + " + (_currentOctave + 1);
        Graphics.DrawText(SCREEN_W / 2 - 72, 62, octStr, _theme.PianoPrimaryKeyColor, 1);
        Graphics.DrawLine(60, 88, SCREEN_W - 60, 88, _theme.ForegroundColor);
    }

    private void DrawHints()
    {
        uint c = Color.RGB(100, 90, 130);
        Graphics.DrawText(KEYBOARD_X, KEYBOARD_Y + WHITE_KEY_H + 6,
            "L/R: Oktave    Touch: mehrere Tasten gleichzeitig    A/B/X/Y/D-Pad: Shortcuts", c, 1);
    }

    public override void OnExit()
    {
        Audio.Stop();
        Audio.Exit();
    }
}
