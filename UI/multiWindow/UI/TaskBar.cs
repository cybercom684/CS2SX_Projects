using System;
using System.Collections.Generic;

namespace multiWindow.UI
{
    public class TaskBar
    {
        // ── Layout ────────────────────────────────────────────────────────────
        private const int BarY      = 680;
        private const int BarH      = 40;
        private const int StartBtnW = 82;
        private const int StartBtnX = 4;
        private const int TabW      = 138;
        private const int TabGap    = 2;
        private const int TabStartX = StartBtnX + StartBtnW + 6;

        // ── Callbacks (set by Desktop) ─────────────────────────────────────
        public Action         OnStartTapped { get; set; } = () => { };
        public Action<Window> OnTabTapped   { get; set; } = _ => { };

        // ── Start button icon ─────────────────────────────────────────────────
        private const int StartIconSize  = 30;   // icon rendered at this px size
        private Texture   _startIcon     = null;
        private bool      _startIconLoaded = false;

        // ── Window registry ───────────────────────────────────────────────────
        private List<Window> _windows = new List<Window>();

        public void RegisterWindow(Window win)   => _windows.Add(win);
        public void UnregisterWindow(Window win) => _windows.Remove(win);

        // ── Touch handling ────────────────────────────────────────────────────
        public bool HandleTouch(TouchState touch, bool touchBegan)
        {
            if (!touch.IsTouched || !touchBegan) return false;

            int tx = touch.X0;
            int ty = touch.Y0;

            if (ty < BarY || ty > BarY + BarH) return false;

            // Start button
            if (tx >= StartBtnX && tx <= StartBtnX + StartBtnW)
            {
                OnStartTapped();
                return true;
            }

            // Window tabs
            for (int i = 0; i < _windows.Count; i++)
            {
                int x0 = TabStartX + i * (TabW + TabGap);
                if (tx >= x0 && tx <= x0 + TabW)
                {
                    OnTabTapped(_windows[i]);
                    return true;
                }
            }

            return true; // consumed — stay in taskbar area
        }

        // ── Drawing ───────────────────────────────────────────────────────────
        public void Draw()
        {
            // Taskbar background
            Graphics.FillRect(0, BarY, 1280, BarH, Color.RGB(22, 22, 22));

            // Top separator line
            Graphics.DrawLine(0, BarY, 1280, BarY, Color.RGB(55, 55, 55));

            // Start button
            if (!_startIconLoaded)
            {
                _startIcon       = Graphics.LoadTexture("romfs:/haus.bmp");
                _startIconLoaded = true;
            }

            Graphics.FillRect(StartBtnX, BarY + 4, StartBtnW, BarH - 8, Color.RGB(0, 120, 215));
            if (_startIcon != null)
            {
                Graphics.DrawTextureCenteredScaled(_startIcon,
                    StartBtnX, BarY + 4, StartBtnW, BarH - 8,
                    StartIconSize, StartIconSize);
            }
            else
            {
                int swW = Graphics.MeasureTextWidth("Start", 2);
                int swH = Graphics.MeasureTextHeight(2);
                Graphics.DrawText(StartBtnX + (StartBtnW - swW) / 2, BarY + (BarH - swH) / 2,
                                  "Start", Color.White, 2);
            }

            // Window tabs
            for (int i = 0; i < _windows.Count; i++)
            {
                int x0       = TabStartX + i * (TabW + TabGap);
                bool active   = _windows[i].IsActive;
                bool minimized = _windows[i].IsMinimized;

                uint tabBg = active    ? Color.RGB(60, 60, 60)
                           : minimized ? Color.RGB(26, 26, 26)
                                       : Color.RGB(38, 38, 38);

                Graphics.FillRect(x0, BarY + 4, TabW, BarH - 8, tabBg);

                // Active indicator: blue for focused, amber for minimized
                if (active && !minimized)
                    Graphics.FillRect(x0, BarY + BarH - 4, TabW, 3, Color.RGB(0, 120, 215));
                else if (minimized)
                    Graphics.FillRect(x0, BarY + BarH - 4, TabW, 3, Color.RGB(180, 110, 20));

                // Tab title (truncate if too long)
                string title = _windows[i].Title;
                if (title.Length > 13)
                    title = title.Substring(0, 12) + "~";

                uint titleColor = minimized ? Color.RGB(140, 140, 140) : Color.White;
                int twH = Graphics.MeasureTextHeight(2);
                Graphics.DrawText(x0 + 8, BarY + (BarH - twH) / 2, title, titleColor, 2);
            }

            // Time and battery (bottom-right)
            TimeInfo   t    = System.GetTime();
            BatteryInfo bat = System.GetBattery();

            string hh  = t.hour   < 10 ? "0" + t.hour   : "" + t.hour;
            string mm  = t.minute < 10 ? "0" + t.minute : "" + t.minute;
            string timeStr = hh + ":" + mm;
            string batStr  = bat.percent + "%";

            int infoTextH = Graphics.MeasureTextHeight(2);
            int timeW     = Graphics.MeasureTextWidth(timeStr, 2);
            int batW      = Graphics.MeasureTextWidth(batStr,  2);

            int rightMargin = 10;
            int gap         = 12;

            int batX  = 1280 - rightMargin - batW;
            int timeX = batX - gap - timeW;
            int textY = BarY + (BarH - infoTextH) / 2;

            Graphics.DrawText(timeX, textY, timeStr, Color.White, 2);
            Graphics.DrawText(batX,  textY, batStr,  Color.White, 2);
        }
    }
}
