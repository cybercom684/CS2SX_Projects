namespace textEditor.Core.UI.Components
{
    public class ButtonWidget
    {
        private readonly int _x, _y, _scale;
        private readonly string _text;
        private readonly uint _color, _colorPressed;
        private bool _wasDown;

        private const int PadX = 12;
        private const int PadY = 6;

        public ButtonWidget(int x, int y, string text,
                            uint color, uint colorPressed, int scale)
        {
            _x = x; _y = y; _text = text;
            _color = color; _colorPressed = colorPressed;
            _scale = scale;
        }

        public void Draw(bool isDown)
        {
            uint bg = isDown ? _colorPressed : _color;
            int w = 8 * _scale * _text.Length + PadX * 2;
            int h = 8 * _scale + PadY * 2;

            Graphics.FillRect(_x, _y, w, h, bg);
            Graphics.DrawRect(_x, _y, w, h, Color.RGB(0, 0, 0)); // subtle border
            Graphics.DrawText(_x + PadX, _y + PadY, _text, Color.RGB(32, 1, 22), _scale);
        }

        public bool HandleInput(bool isDown)
        {
            bool triggered = isDown && !_wasDown;
            _wasDown = isDown;
            return triggered;
        }
    }
}