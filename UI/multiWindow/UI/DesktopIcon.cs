using System;

namespace multiWindow.UI
{
    public class DesktopIcon
    {
        public int    X          { get; set; }
        public int    Y          { get; set; }
        public string Label      { get; set; } = "";
        public uint   IconColor  { get; set; } = Color.RGB(0, 120, 215);
        public Action OnTap      { get; set; } = () => { };

        private const int BoxSize  = 48;
        private const int TotalW   = 64;
        private const int LabelGap = 4;
        private const int LabelH   = 10;
        private const int TotalH   = BoxSize + LabelGap + LabelH;

        private bool _pressed;

        public bool HitTest(int tx, int ty)
            => tx >= X && tx <= X + TotalW && ty >= Y && ty <= Y + TotalH;

        public void Update(TouchState touch, bool touchBegan)
        {
            if (!touch.IsTouched) { _pressed = false; return; }
            if (touchBegan && HitTest(touch.X0, touch.Y0))
            {
                _pressed = true;
                OnTap();
            }
            else if (!touchBegan)
            {
                _pressed = false;
            }
        }

        public void Draw()
        {
            int bx = X + (TotalW - BoxSize) / 2;
            byte bgAlpha = _pressed ? (byte)100 : (byte)45;

            // Icon box background
            Graphics.FillRectAlpha(bx, Y, BoxSize, BoxSize, Color.White, bgAlpha);
            Graphics.DrawRoundedRect(bx, Y, BoxSize, BoxSize, 6, Color.RGB(100, 100, 120));

            // Colored inner shape
            Graphics.FillRoundedRect(bx + 8, Y + 8, BoxSize - 16, BoxSize - 16, 4, IconColor);

            // Label with drop-shadow for readability on any wallpaper
            string lbl = Label;
            int lw = Graphics.MeasureTextWidth(lbl, 1);
            while (lbl.Length > 1 && lw > TotalW + 16)
            {
                lbl = lbl.Substring(0, lbl.Length - 1);
                lw  = Graphics.MeasureTextWidth(lbl, 1);
            }
            int lx = X + (TotalW - lw) / 2;
            int ly = Y + BoxSize + LabelGap;
            Graphics.DrawText(lx + 1, ly + 1, lbl, Color.RGB(0, 0, 0), 1);
            Graphics.DrawText(lx,     ly,     lbl, Color.White, 1);
        }
    }
}
