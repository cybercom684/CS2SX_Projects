using multiWindow.Models;
using multiWindow.UI.Controls;

namespace multiWindow.UI
{
    public class Window
    {
        // ── Layout ────────────────────────────────────────────────────────────
        public int    X      { get; set; }
        public int    Y      { get; set; }
        public int    Width  { get; set; }
        public int    Height { get; set; }

        // ── Appearance ────────────────────────────────────────────────────────
        public string      Title    { get; set; } = "Fenster";
        public ThemeConfig Theme    { get; set; }
        public bool        IsActive { get; set; }

        // ── State ─────────────────────────────────────────────────────────────
        public bool IsClosed    { get; private set; }
        public bool IsMinimized { get; private set; }
        public bool IsMaximized { get; private set; }

        // ── Constants ─────────────────────────────────────────────────────────
        private const int TitleBarH = 30;
        private const int BtnSize   = 22;
        private const int BtnPad    = 4;
        private const int BtnGap    = 2;

        // ── Controls ──────────────────────────────────────────────────────────
        private UIControl[] _controls = new UIControl[32];
        private int         _controlCount;

        public int ControlCount => _controlCount;

        // Content area starts below the title bar
        private int ContentOffsetY => Y + TitleBarH;
        private int ContentHeight  => Height - TitleBarH;

        // ── Saved position (maximize / restore) ───────────────────────────────
        private int _savedX;
        private int _savedY;
        private int _savedWidth;
        private int _savedHeight;

        // ── Drag state ────────────────────────────────────────────────────────
        private bool _dragging;
        private int  _dragOffX;
        private int  _dragOffY;

        // ── Constructor ───────────────────────────────────────────────────────
        public Window()
        {
            Theme = ThemeConfig.Dark();
        }

        // ── Control management ────────────────────────────────────────────────
        public void AddControl(UIControl control)
        {
            if (_controlCount < 32)
                _controls[_controlCount++] = control;
        }

        public UIControl GetControl(int index)
        {
            if (index >= 0 && index < _controlCount)
                return _controls[index];
            return null;
        }

        // ── State transitions ─────────────────────────────────────────────────
        public void Minimize()
        {
            IsMinimized = true;
            _dragging   = false;
        }

        public void Restore()
        {
            IsMinimized = false;
        }

        private void ToggleMaximize()
        {
            if (IsMaximized)
            {
                X      = _savedX;
                Y      = _savedY;
                Width  = _savedWidth;
                Height = _savedHeight;
                IsMaximized = false;
            }
            else
            {
                _savedX      = X;
                _savedY      = Y;
                _savedWidth  = Width;
                _savedHeight = Height;
                X      = 0;
                Y      = 0;
                Width  = 1280;
                Height = 680;
                IsMaximized = true;
                _dragging   = false;
            }
        }

        // ── Hit test for Desktop touch routing ────────────────────────────────
        public bool HitTestGlobal(int tx, int ty)
        {
            if (IsMinimized) return false;
            return tx >= X && tx <= X + Width && ty >= Y && ty <= Y + Height;
        }

        // ── Touch handling ────────────────────────────────────────────────────
        public bool HandleTouch(TouchState touch, bool touchBegan)
        {
            if (!touch.IsTouched)
            {
                _dragging = false;
                for (int i = 0; i < _controlCount; i++)
                    _controls[i].HandleTouch(touch, false, X, ContentOffsetY);
                return false;
            }

            int tx = touch.X0;
            int ty = touch.Y0;

            if (touchBegan)
            {
                // Button strip (right side of title bar)
                int by  = Y + BtnPad;
                int cbX = X + Width - BtnPad - BtnSize;                              // close
                int mxX = X + Width - BtnPad - 2 * BtnSize - BtnGap;                // maximize
                int mnX = X + Width - BtnPad - 3 * BtnSize - 2 * BtnGap;            // minimize

                // Close
                if (tx >= cbX && tx <= cbX + BtnSize && ty >= by && ty <= by + BtnSize)
                {
                    IsClosed = true;
                    return true;
                }

                // Maximize / restore
                if (tx >= mxX && tx <= mxX + BtnSize && ty >= by && ty <= by + BtnSize)
                {
                    ToggleMaximize();
                    return true;
                }

                // Minimize
                if (tx >= mnX && tx <= mnX + BtnSize && ty >= by && ty <= by + BtnSize)
                {
                    Minimize();
                    return true;
                }

                // Title bar drag (disabled when maximized)
                if (!IsMaximized
                 && tx >= X && tx <= X + Width
                 && ty >= Y && ty <= Y + TitleBarH)
                {
                    _dragging = true;
                    _dragOffX = tx - X;
                    _dragOffY = ty - Y;
                    return true;
                }
            }

            // Continue drag
            if (_dragging)
            {
                int nx = tx - _dragOffX;
                int ny = ty - _dragOffY;
                if (nx < 0) nx = 0;
                if (ny < 0) ny = 0;
                if (nx + Width  > 1280) nx = 1280 - Width;
                if (ny + Height > 680)  ny = 680  - Height;
                X = nx;
                Y = ny;
                return true;
            }

            // Route to controls in content area
            for (int i = 0; i < _controlCount; i++)
                _controls[i].HandleTouch(touch, touchBegan, X, ContentOffsetY);

            return true;
        }

        // ── Drawing ───────────────────────────────────────────────────────────
        public void Draw()
        {
            // Drop shadow (skip when maximized — would bleed off-screen)
            if (!IsMaximized)
                Graphics.FillRect(X + 3, Y + 3, Width, Height, Color.RGB(10, 10, 10));

            // Window body
            Graphics.FillRect(X, Y, Width, Height, Theme.Background);

            // Title bar
            uint titleBg = IsActive ? Theme.Primary : Theme.TitleBar;
            Graphics.FillRect(X, Y, Width, TitleBarH, titleBg);

            // Button positions
            int by  = Y + BtnPad;
            int cbX = X + Width - BtnPad - BtnSize;
            int mxX = X + Width - BtnPad - 2 * BtnSize - BtnGap;
            int mnX = X + Width - BtnPad - 3 * BtnSize - 2 * BtnGap;

            // Title text — clipped so it never reaches the buttons
            int maxTitleW = mnX - (X + 10) - 6;
            string titleStr = Title;
            int textH = Graphics.MeasureTextHeight(2);
            int textY = Y + (TitleBarH - textH) / 2;
            while (titleStr.Length > 1 && Graphics.MeasureTextWidth(titleStr, 2) > maxTitleW)
                titleStr = titleStr.Substring(0, titleStr.Length - 1);
            if (titleStr.Length < Title.Length)
                titleStr = titleStr.Substring(0, titleStr.Length - 1) + "~";
            Graphics.DrawText(X + 10, textY, titleStr, Theme.TitleBarText, 2);

            // ── Close button (red, × icon) ────────────────────────────────────
            Graphics.FillRect(cbX, by, BtnSize, BtnSize, Color.RGB(196, 43, 28));
            {
                int cx = cbX + BtnSize / 2;
                int cy = by  + BtnSize / 2;
                int d  = BtnSize / 2 - 4;
                Graphics.DrawLine(cx - d, cy - d, cx + d, cy + d, Color.White);
                Graphics.DrawLine(cx + d, cy - d, cx - d, cy + d, Color.White);
            }

            // ── Maximize / restore button (green / amber, □ icon) ─────────────
            uint mxBg = IsMaximized ? Color.RGB(180, 95, 6) : Color.RGB(40, 160, 80);
            Graphics.FillRect(mxX, by, BtnSize, BtnSize, mxBg);
            {
                int p = 5;
                Graphics.DrawRect(mxX + p, by + p, BtnSize - 2 * p, BtnSize - 2 * p, Color.White);
                if (IsMaximized)
                {
                    // Restore: second outline offset up-right
                    int p2 = p - 2;
                    Graphics.DrawRect(mxX + p2 + 2, by + p2, BtnSize - 2 * p2 - 2, BtnSize - 2 * p2 - 2, Color.White);
                }
            }

            // ── Minimize button (gray, — icon) ────────────────────────────────
            Graphics.FillRect(mnX, by, BtnSize, BtnSize, Color.RGB(90, 90, 90));
            {
                int barH2 = 2;
                int barW2 = BtnSize - 8;
                Graphics.FillRect(mnX + 4, by + BtnSize - 6, barW2, barH2, Color.White);
            }

            // Separator between title bar and content
            Graphics.DrawLine(X, Y + TitleBarH, X + Width, Y + TitleBarH, Theme.Border);

            // Content controls
            for (int i = 0; i < _controlCount; i++)
            {
                if (_controls[i].Visible)
                    _controls[i].Draw(X, ContentOffsetY);
            }

            // Window border (drawn last, on top)
            uint borderColor = IsActive ? Theme.Primary : Theme.Border;
            Graphics.DrawRect(X, Y, Width, Height, borderColor);
        }
    }
}
