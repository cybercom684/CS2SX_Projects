using System;

namespace multiWindow.UI
{
    public class WindowSwitcher
    {
        private const int CardW   = 178;
        private const int CardH   = 108;
        private const int CardGap = 14;
        private const int MaxCols = 5;

        public bool IsOpen { get; private set; }

        private Window[]       _windows;
        private int            _count;
        private Action<Window> _onSelect;

        public void Open(Window[] windows, int count, Action<Window> onSelect)
        {
            _windows  = windows;
            _count    = count;
            _onSelect = onSelect;
            IsOpen    = true;
        }

        public void Close() => IsOpen = false;

        public bool HandleTouch(TouchState touch, bool touchBegan)
        {
            if (!IsOpen || !touch.IsTouched || !touchBegan) return IsOpen;

            int tx = touch.X0;
            int ty = touch.Y0;

            int cols   = _count < MaxCols ? _count : MaxCols;
            int totalW = cols * CardW + (cols - 1) * CardGap;
            int startX = (1280 - totalW) / 2;
            int startY = (680 - CardH) / 2;

            for (int i = 0; i < _count; i++)
            {
                int cx = startX + i * (CardW + CardGap);
                int cy = startY;
                if (tx >= cx && tx <= cx + CardW && ty >= cy && ty <= cy + CardH)
                {
                    _onSelect(_windows[i]);
                    Close();
                    return true;
                }
            }

            Close();
            return true;
        }

        public void Draw()
        {
            if (!IsOpen) return;

            // Backdrop
            Graphics.FillRectAlpha(0, 0, 1280, 680, Color.Black, 165);

            int cols   = _count < MaxCols ? _count : MaxCols;
            int totalW = cols * CardW + (cols - 1) * CardGap;
            int startX = (1280 - totalW) / 2;
            int startY = (680 - CardH) / 2;

            // Hint
            string hint = "L - Schliessen     Tippen - Fenster fokussieren";
            int hintW = Graphics.MeasureTextWidth(hint, 1);
            int hintH = Graphics.MeasureTextHeight(1);
            Graphics.DrawText((1280 - hintW) / 2, startY - 28, hint, Color.RGB(170, 170, 180), 1);

            for (int i = 0; i < _count; i++)
            {
                int cx = startX + i * (CardW + CardGap);
                int cy = startY;
                bool active = _windows[i].IsActive;

                // Card shadow
                Graphics.FillRectAlpha(cx + 4, cy + 4, CardW, CardH, Color.Black, 110);

                // Card background
                uint cardBg = active ? Color.RGB(0, 55, 115) : Color.RGB(36, 36, 42);
                Graphics.FillRoundedRect(cx, cy, CardW, CardH, 8, cardBg);

                // Card border
                uint borderCol = active ? Color.RGB(0, 120, 215) : Color.RGB(62, 62, 74);
                Graphics.DrawRoundedRect(cx, cy, CardW, CardH, 8, borderCol);

                // Mini window preview
                int preX = cx + 8;
                int preY = cy + 8;
                int preW = CardW - 16;
                int preH = CardH - 34;

                Graphics.FillRoundedRect(preX, preY, preW, preH, 3, Color.RGB(42, 42, 48));
                Graphics.FillRect(preX, preY, preW, 11, _windows[i].Theme.Primary);
                Graphics.DrawRoundedRect(preX, preY, preW, preH, 3, Color.RGB(68, 68, 78));

                // Mini title in preview bar
                string mini = _windows[i].Title;
                if (mini.Length > 9) mini = mini.Substring(0, 8) + ".";
                Graphics.DrawText(preX + 3, preY + 2, mini, Color.White, 1);

                // Window label below card
                string lbl = _windows[i].Title;
                if (lbl.Length > 14) lbl = lbl.Substring(0, 13) + "~";
                int lw = Graphics.MeasureTextWidth(lbl, 1);
                int lh = Graphics.MeasureTextHeight(1);
                uint lblColor = active ? Color.White : Color.RGB(155, 155, 162);
                Graphics.DrawText(cx + (CardW - lw) / 2, cy + CardH - lh - 6, lbl, lblColor, 1);
            }
        }
    }
}
