using piano.Core.Models;

namespace piano.Core.GUI
{
    public class PianoKeyButton
    {
        private readonly PianoKey _key;
        private readonly Theme _theme;
        private bool _isPressed;
        private int _pressTimer;
        private const int PRESS_DURATION = 22; // ~366 ms @ 60 fps ≈ note duration

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsBlack => _key.isSecondary;
        public int  MidiKey => _key.MidiKey;

        public PianoKeyButton(PianoKey key, Theme theme)
        {
            _key = key;
            _theme = theme;
        }

        public void Draw()
        {
            uint bg = _isPressed
                ? _theme.AccentColor
                : (IsBlack ? _theme.PianoSecondaryKeyColor : _theme.PianoPrimaryKeyColor);
            uint fg = IsBlack ? _theme.PianoSecondaryKeyTextColor : _theme.PianoPrimaryKeyTextColor;

            Graphics.FillRect(X, Y, Width, Height, bg);
            Graphics.DrawRect(X, Y, Width, Height, _theme.ForegroundColor);

            if (_isPressed)
            {
                Graphics.FillRect(X + 2, Y + Height - 6, Width - 4, 4, fg);
            }

            // Tastenname nur bei Weiß-Tasten (unten) oder bei Schwarz (kompakt)
            if (!IsBlack)
            {
                int labelY = Y + Height - 18;
                Graphics.DrawText(X + (Width / 2) - 4, labelY, _key.Name, fg, 1);
            }
        }

        public void Update()
        {
            if (_pressTimer > 0)
            {
                _pressTimer--;
                if (_pressTimer == 0) _isPressed = false;
            }
        }

        public bool HitTest(int tx, int ty)
        {
            return tx >= X && tx < X + Width && ty >= Y && ty < Y + Height;
        }

        public void Press()
        {
            _isPressed = true;
            _pressTimer = PRESS_DURATION;
        }
    }
}