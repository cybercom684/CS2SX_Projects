namespace multiWindow.UI.Controls
{
    public class LabelControl : UIControl
    {
        public string Text     { get; set; } = "";
        public uint   Color    { get; set; }
        public int    FontSize { get; set; } = 2;

        public override void Draw(int offsetX, int offsetY)
        {
            if (!Visible) return;
            Graphics.DrawText(offsetX + X, offsetY + Y, Text, Color, FontSize);
        }
    }
}
