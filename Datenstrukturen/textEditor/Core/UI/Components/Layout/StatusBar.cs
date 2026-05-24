namespace textEditor.Core.UI.Components.Layout
{
    public class StatusBar
    {
        private readonly int _y;
        private readonly uint _bgColor, _borderColor, _textColor;

        public StatusBar(int y, uint bgColor, uint borderColor, uint textColor)
        {
            _y = y;
            _bgColor = bgColor; _borderColor = borderColor; _textColor = textColor;
        }

        public void Draw(string info)
        {
            Graphics.FillRect(1, _y, 1278, 28, _bgColor);
            Graphics.DrawRect(1, _y, 1278, 28, _borderColor);
            Graphics.DrawText(12, _y + 8, info, _textColor, 1);
        }
    }
}