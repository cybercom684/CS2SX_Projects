using System;

namespace multiWindow.UI
{
    public class StartMenu
    {
        // ── Layout (anchored bottom-left, above taskbar) ───────────────────────
        private const int MenuW  = 260;
        private const int MenuH  = 320;
        private const int MenuX  = 0;
        private const int MenuY  = 680 - MenuH;   // 360
        private const int HeaderH = 36;
        private const int ItemH   = 34;
        private const int ItemX   = 8;

        // ── State ─────────────────────────────────────────────────────────────
        public bool IsOpen { get; private set; }

        // ── Items ─────────────────────────────────────────────────────────────
        private string[] _labels    = new string[12];
        private Action[] _callbacks = new Action[12];
        private int      _count;

        public void AddItem(string label, Action callback)
        {
            if (_count >= 12) return;
            _labels[_count]    = label;
            _callbacks[_count] = callback;
            _count++;
        }

        // ── Visibility ────────────────────────────────────────────────────────
        public void Toggle() => IsOpen = !IsOpen;
        public void Close()  => IsOpen = false;

        // ── Touch handling ────────────────────────────────────────────────────
        public bool HandleTouch(TouchState touch, bool touchBegan)
        {
            if (!IsOpen || !touch.IsTouched || !touchBegan) return IsOpen;

            int tx = touch.X0;
            int ty = touch.Y0;

            // Tap outside → close
            if (tx < MenuX || tx > MenuX + MenuW || ty < MenuY || ty > MenuY + MenuH)
            {
                Close();
                return false;
            }

            // Items (below header)
            int itemAreaY = MenuY + HeaderH;
            for (int i = 0; i < _count; i++)
            {
                int iy = itemAreaY + i * ItemH;
                if (ty >= iy && ty < iy + ItemH)
                {
                    _callbacks[i]();
                    Close();
                    return true;
                }
            }

            return true; // consumed
        }

        // ── Drawing ───────────────────────────────────────────────────────────
        public void Draw()
        {
            if (!IsOpen) return;

            // Background
            Graphics.FillRect(MenuX, MenuY, MenuW, MenuH, Color.RGB(30, 30, 30));

            // Header bar
            Graphics.FillRect(MenuX, MenuY, MenuW, HeaderH, Color.RGB(0, 120, 215));
            int hw = Graphics.MeasureTextWidth("Menu", 2);
            int hh = Graphics.MeasureTextHeight(2);
            Graphics.DrawText(MenuX + (MenuW - hw) / 2, MenuY + (HeaderH - hh) / 2,
                              "Menu", Color.White, 2);

            // Items
            int itemAreaY = MenuY + HeaderH;
            for (int i = 0; i < _count; i++)
            {
                int iy = itemAreaY + i * ItemH;

                // Separator
                Graphics.DrawLine(MenuX + 6, iy, MenuX + MenuW - 6, iy, Color.RGB(50, 50, 50));

                // Label
                int lh = Graphics.MeasureTextHeight(2);
                Graphics.DrawText(MenuX + ItemX + 4, iy + (ItemH - lh) / 2,
                                  _labels[i], Color.White, 2);
            }

            // Border
            Graphics.DrawRect(MenuX, MenuY, MenuW, MenuH, Color.RGB(70, 70, 70));
        }
    }
}
