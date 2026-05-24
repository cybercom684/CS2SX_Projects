using System;

namespace multiWindow.UI.Controls
{
    public class ButtonControl : UIControl
    {
        public string Text     { get; set; } = "";
        public uint   BackColor { get; set; }
        public uint   ForeColor { get; set; }
        public int    FontSize  { get; set; } = 2;
        public Action OnClick   { get; set; } = () => { };

        private bool _isPressed;

        public override bool HandleTouch(TouchState touch, bool touchBegan, int offsetX, int offsetY)
        {
            if (!Visible) return false;

            if (!touch.IsTouched)
            {
                _isPressed = false;
                return false;
            }

            if (HitTest(touch.X0, touch.Y0, offsetX, offsetY))
            {
                if (touchBegan)
                {
                    _isPressed = true;
                    OnClick();
                }
                return true;
            }

            return false;
        }

        public override void Draw(int offsetX, int offsetY)
        {
            if (!Visible) return;

            int ax = offsetX + X;
            int ay = offsetY + Y;

            uint bg = _isPressed ? ForeColor : BackColor;
            uint fg = _isPressed ? BackColor : ForeColor;

            Graphics.FillRect(ax, ay, Width, Height, bg);
            Graphics.DrawRect(ax, ay, Width, Height, fg);

            int tw = Graphics.MeasureTextWidth(Text, FontSize);
            int th = Graphics.MeasureTextHeight(FontSize);
            Graphics.DrawText(ax + (Width - tw) / 2, ay + (Height - th) / 2, Text, fg, FontSize);
        }
    }
}
