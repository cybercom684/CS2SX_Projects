namespace textEditor.Core.UI.Components.Layout
{
    public class InfoPanel
    {
        private string _title, _content;
        private uint _bgColor, _borderColor, _textColor;

        public InfoPanel(string title, string content,
                         uint bgColor, uint borderColor, uint textColor)
        {
            _title = title; _content = content;
            _bgColor = bgColor; _borderColor = borderColor; _textColor = textColor;
        }

        public void Draw()
        {
            Graphics.FillRect(1, 1, 1278, 48, _bgColor);
            Graphics.DrawRect(1, 1, 1278, 48, _borderColor);
            Graphics.DrawText(12, 8, _title, _textColor, 2);
            Graphics.DrawText(12, 32, _content, Color.RGB(32, 1, 22), 1); // immer dunkel
        }

        public void Update(string title, string content,
                           uint bgColor, uint borderColor, uint textColor)
        {
            _title = title; _content = content;
            _bgColor = bgColor; _borderColor = borderColor; _textColor = textColor;
        }
    }
}
