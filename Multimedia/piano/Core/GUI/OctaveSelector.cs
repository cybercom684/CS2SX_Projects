namespace piano.Core.GUI
{
    public class OctaveSelector
    {
        public int Octave { get; private set; } = 4;
        private const int MIN = 1;
        private const int MAX = 7;

        private int _btnLX, _btnLY, _btnRX, _btnRY, _btnW, _btnH;
        private uint _color;
        private uint _textColor;

        public void SetPosition(int x, int y, int btnW, int btnH, uint color, uint textColor)
        {
            _btnW = btnW; _btnH = btnH;
            _btnLX = x; _btnLY = y;
            _btnRX = x + btnW + 60; _btnRY = y;
            _color = color; _textColor = textColor;
        }

        public void Draw()
        {
            Graphics.FillRoundedRect(_btnLX, _btnLY, _btnW, _btnH, 6, _color);
            Graphics.DrawText(_btnLX + 8, _btnLY + 6, "<", _textColor, 2);

            string label = "Oct " + Octave;
            int lw = Graphics.MeasureTextWidth(label, 2);
            int midX = _btnRX - 30 - lw / 2;
            Graphics.DrawText(midX, _btnLY + 6, label, _textColor, 2);

            Graphics.FillRoundedRect(_btnRX, _btnRY, _btnW, _btnH, 6, _color);
            Graphics.DrawText(_btnRX + 8, _btnRY + 6, ">", _textColor, 2);
        }

        public void HandleTouch(TouchState touch)
        {
            if (!touch.IsTouched) return;
            HandleTouchAt(touch.X0, touch.Y0);
        }

        public void HandleTouchAt(int tx, int ty)
        {
            if (tx >= _btnLX && tx < _btnLX + _btnW && ty >= _btnLY && ty < _btnLY + _btnH)
            {
                if (Octave > MIN) Octave--;
            }
            else if (tx >= _btnRX && tx < _btnRX + _btnW && ty >= _btnRY && ty < _btnRY + _btnH)
            {
                if (Octave < MAX) Octave++;
            }
        }

        public bool HandleButtons()
        {
            bool changed = false;
            if (Input.IsDown(NpadButton.L) && Octave > MIN) { Octave--; changed = true; }
            if (Input.IsDown(NpadButton.R) && Octave < MAX) { Octave++; changed = true; }
            return changed;
        }
    }
}