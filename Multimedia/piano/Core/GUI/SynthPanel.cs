using piano.Core.Models;

namespace piano.Core.GUI
{
    public class SynthPanel
    {
        private static readonly string[] WaveNames = { "SINE", "SAW", "SQR", "TRI", "NOI" };
        private static readonly string[] FiltNames = { "OFF", "LP", "HP", "BP", "NOT" };

        // OSC A
        private int   _waveA  = Audio.WAVE_SINE;
        private int   _unison = 1;
        private float _detune = 0.0f;

        // Amplitude ADSR — stored as normalized t ∈ [0,1]; quadratic: TToMs(t) = 5 + t²·2995
        private float _attackT  = 0.04f;
        private float _decayT   = 0.26f;
        private float _sustainT = 0.70f;
        private float _releaseT = 0.32f;

        // Filter — quadratic: TToHz(t) = 20 + t²·19980
        private int   _filtType = Audio.FILT_OFF;
        private float _cutoffT  = 0.63f;
        private float _resoT    = 0.51f;

        private Theme _theme;
        private uint _bg, _dim, _accent, _trackDim, _thumb, _labelC, _valC;

        // ── Layout ────────────────────────────────────────────────────────────

        private const int OSC_X  = 40;
        private const int ENV_X  = 445;
        private const int FILT_X = 850;
        private const int COL_W  = 380;

        private const int PY     = 95;
        private const int R1Y    = 119;
        private const int R2Y    = 163;
        private const int R3Y    = 207;
        private const int R4Y    = 251;
        public  const int BOTTOM = 278;

        private const int BTN_H   = 28;
        private const int TRACK_H = 10;
        private const int LW      = 52;
        private const int VW      = 56;
        private const int SW      = COL_W - LW - VW - 4;  // = 268

        // ── Scale helpers ─────────────────────────────────────────────────────

        private static float TToMs(float t) { return 5.0f + t * t * 2995.0f; }
        private static float TToHz(float t) { return 20.0f + t * t * 19980.0f; }

        // ── Format helpers ────────────────────────────────────────────────────

        private static string FmtMs(float ms)
        {
            int v = (int)ms;
            if (v >= 1000) return (v / 1000) + "." + ((v % 1000) / 100) + "s";
            return v + "ms";
        }

        private static string FmtHz(float hz)
        {
            int v = (int)hz;
            if (v >= 1000) return (v / 1000) + "." + ((v % 1000) / 100) + "k";
            return v + "Hz";
        }

        private static string FmtPct(float t) { return ((int)(t * 100)) + "%"; }
        private static string FmtCt(float ct)  { return ((int)ct) + "ct"; }

        // ── Public API ────────────────────────────────────────────────────────

        public void Init(Theme theme)
        {
            _theme    = theme;
            _bg       = Color.RGB(10, 10, 16);
            _dim      = Color.RGB(38, 32, 56);
            _accent   = theme.AccentColor;
            _trackDim = Color.RGB(28, 24, 44);
            _thumb    = theme.PianoPrimaryKeyColor;
            _labelC   = Color.RGB(100, 90, 130);
            _valC     = Color.RGB(200, 192, 220);
            ApplyAll();
        }

        public void HandleTouchAt(int tx, int ty)
        {
            bool changed = false;

            if (tx >= OSC_X && tx < OSC_X + COL_W)
            {
                if (ty >= R1Y && ty < R1Y + BTN_H)
                {
                    int hit = HitTypeBtn(tx, OSC_X, 5);
                    if (hit >= 0 && hit != _waveA) { _waveA = hit; changed = true; }
                }
                else if (ty >= R2Y && ty < R2Y + BTN_H)
                {
                    int hit = HitUnisonBtn(tx);
                    if (hit > 0 && hit != _unison) { _unison = hit; changed = true; }
                }
                else if (ty >= R3Y - 8 && ty < R3Y + TRACK_H + 8)
                {
                    _detune = SliderT(tx, OSC_X) * 50.0f;
                    changed = true;
                }
            }
            else if (tx >= ENV_X && tx < ENV_X + COL_W)
            {
                if (ty >= R1Y - 8 && ty < R1Y + TRACK_H + 8)
                    { _attackT = SliderT(tx, ENV_X);  changed = true; }
                else if (ty >= R2Y - 8 && ty < R2Y + TRACK_H + 8)
                    { _decayT = SliderT(tx, ENV_X);   changed = true; }
                else if (ty >= R3Y - 8 && ty < R3Y + TRACK_H + 8)
                    { _sustainT = SliderT(tx, ENV_X); changed = true; }
                else if (ty >= R4Y - 8 && ty < R4Y + TRACK_H + 8)
                    { _releaseT = SliderT(tx, ENV_X); changed = true; }
            }
            else if (tx >= FILT_X && tx < FILT_X + COL_W)
            {
                if (ty >= R1Y && ty < R1Y + BTN_H)
                {
                    int hit = HitTypeBtn(tx, FILT_X, 5);
                    if (hit >= 0 && hit != _filtType) { _filtType = hit; changed = true; }
                }
                else if (ty >= R2Y - 8 && ty < R2Y + TRACK_H + 8)
                    { _cutoffT = SliderT(tx, FILT_X); changed = true; }
                else if (ty >= R3Y - 8 && ty < R3Y + TRACK_H + 8)
                    { _resoT = SliderT(tx, FILT_X);   changed = true; }
            }

            if (changed) ApplyAll();
        }

        public void Draw()
        {
            int bgX = OSC_X - 6;
            int bgW = FILT_X + COL_W - OSC_X + 12;
            Graphics.FillRect(bgX, PY - 6, bgW, BOTTOM - PY + 6, _bg);

            uint divC = Color.RGB(45, 38, 65);
            Graphics.DrawLine(ENV_X - 8,  PY - 4, ENV_X - 8,  BOTTOM, divC);
            Graphics.DrawLine(FILT_X - 8, PY - 4, FILT_X - 8, BOTTOM, divC);

            DrawTitle(OSC_X, "OSC A");
            DrawTypeButtons(OSC_X, R1Y, WaveNames, 5, _waveA);
            DrawUnisonRow();
            DrawSlider(OSC_X, R3Y, "DET", _detune / 50.0f, FmtCt(_detune));

            DrawTitle(ENV_X, "ENVELOPE");
            DrawSlider(ENV_X, R1Y, "ATK", _attackT,  FmtMs(TToMs(_attackT)));
            DrawSlider(ENV_X, R2Y, "DEC", _decayT,   FmtMs(TToMs(_decayT)));
            DrawSlider(ENV_X, R3Y, "SUS", _sustainT, FmtPct(_sustainT));
            DrawSlider(ENV_X, R4Y, "REL", _releaseT, FmtMs(TToMs(_releaseT)));

            DrawTitle(FILT_X, "FILTER");
            DrawTypeButtons(FILT_X, R1Y, FiltNames, 5, _filtType);
            DrawSlider(FILT_X, R2Y, "CUT", _cutoffT, FmtHz(TToHz(_cutoffT)));
            DrawSlider(FILT_X, R3Y, "RES", _resoT,   FmtPct(_resoT));
        }

        // ── Draw helpers ──────────────────────────────────────────────────────

        private void DrawTitle(int colX, string text)
        {
            int tw = Graphics.MeasureTextWidth(text, 1);
            Graphics.DrawText(colX + (COL_W - tw) / 2, PY, text, _accent, 1);
        }

        private void DrawTypeButtons(int colX, int y, string[] names, int count, int active)
        {
            int gap  = 6;
            int btnW = (COL_W - (count - 1) * gap) / count;
            for (int i = 0; i < count; i++)
            {
                int  bx = colX + i * (btnW + gap);
                uint bg = (i == active) ? _accent : _dim;
                uint tc = (i == active) ? _thumb  : _labelC;
                Graphics.FillRoundedRect(bx, y, btnW, BTN_H, 5, bg);
                int tw = Graphics.MeasureTextWidth(names[i], 1);
                Graphics.DrawText(bx + (btnW - tw) / 2, y + 8, names[i], tc, 1);
            }
        }

        private void DrawUnisonRow()
        {
            Graphics.DrawText(OSC_X, R2Y + 8, "UNI", _labelC, 1);
            int startX = OSC_X + LW;
            int avail  = COL_W - LW;
            int gap    = 4;
            int btnW   = (avail - 6 * gap) / 7;
            for (int i = 1; i <= 7; i++)
            {
                int  bx = startX + (i - 1) * (btnW + gap);
                uint bg = (i == _unison) ? _accent : _dim;
                uint tc = (i == _unison) ? _thumb  : _labelC;
                Graphics.FillRoundedRect(bx, R2Y, btnW, BTN_H, 5, bg);
                string s = i.ToString();
                int tw = Graphics.MeasureTextWidth(s, 1);
                Graphics.DrawText(bx + (btnW - tw) / 2, R2Y + 8, s, tc, 1);
            }
        }

        private void DrawSlider(int colX, int y, string label, float t, string valStr)
        {
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;
            int sx   = colX + LW;
            int fill = (int)(t * SW);
            if (fill < 0)  fill = 0;
            if (fill > SW) fill = SW;

            Graphics.DrawText(colX, y + 1, label, _labelC, 1);
            Graphics.FillRect(sx, y + 1, SW, TRACK_H, _trackDim);
            if (fill > 0) Graphics.FillRect(sx, y + 1, fill, TRACK_H, _accent);
            int thumbX = sx + fill - 3;
            if (thumbX < sx - 3) thumbX = sx - 3;
            Graphics.FillRect(thumbX, y - 4, 6, TRACK_H + 8, _thumb);
            Graphics.DrawText(sx + SW + 6, y + 1, valStr, _valC, 1);
        }

        // ── Touch helpers ─────────────────────────────────────────────────────

        private static int HitTypeBtn(int tx, int colX, int count)
        {
            int gap  = 6;
            int btnW = (COL_W - (count - 1) * gap) / count;
            for (int i = 0; i < count; i++)
            {
                int bx = colX + i * (btnW + gap);
                if (tx >= bx && tx < bx + btnW) return i;
            }
            return -1;
        }

        private int HitUnisonBtn(int tx)
        {
            int startX = OSC_X + LW;
            int avail  = COL_W - LW;
            int gap    = 4;
            int btnW   = (avail - 6 * gap) / 7;
            for (int i = 1; i <= 7; i++)
            {
                int bx = startX + (i - 1) * (btnW + gap);
                if (tx >= bx && tx < bx + btnW) return i;
            }
            return -1;
        }

        private static float SliderT(int tx, int colX)
        {
            int   sx = colX + LW;
            float t  = (float)(tx - sx) / (float)SW;
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;
            return t;
        }

        // ── Apply to audio engine ─────────────────────────────────────────────

        private void ApplyAll()
        {
            float spread = _unison > 1 ? 0.8f : 0.0f;
            Audio.SetOscA(_waveA, 1.0f, 0.0f, _unison, _detune, spread);
            Audio.SetADSR(TToMs(_attackT), TToMs(_decayT), _sustainT, TToMs(_releaseT));
            Audio.SetFilter(_filtType, TToHz(_cutoffT), _resoT * 0.98f);
        }
    }
}
