using multiWindow.Models;
using multiWindow.UI.Controls;

namespace multiWindow.UI
{
    public class Window
    {
        // ── Layout ────────────────────────────────────────────────────────────
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // ── Appearance ────────────────────────────────────────────────────────
        public string Title { get; set; } = "Fenster";
        public ThemeConfig Theme { get; set; }
        public bool IsActive { get; set; }

        // ── State ─────────────────────────────────────────────────────────────
        public bool IsClosed { get; private set; }
        public bool IsMinimized { get; private set; }
        public bool IsMaximized { get; private set; }

        // ── Animation ─────────────────────────────────────────────────────────
        private const int AnimFrames = 8;
        private int _animFrame;
        private bool _animating;
        private bool _animMinimizing; // true = sliding down, false = sliding up (restore)
        private int _animTargetY;
        private int _animStartY;
        private int _animTargetX;
        private int _animStartX;

        public bool IsAnimating => _animating;

        // ── Constants ─────────────────────────────────────────────────────────
        private const int TitleBarH = 30;
        private const int BtnSize = 22;
        private const int BtnPad = 4;
        private const int BtnGap = 2;
        private const int ResizeSize = 16;
        private const int MinWidth = 160;
        private const int MinHeight = 80;

        // ── Controls ──────────────────────────────────────────────────────────
        private UIControl[] _controls = new UIControl[32];
        private int _controlCount;
        public int ControlCount => _controlCount;

        private int ContentOffsetY => Y + TitleBarH;
        private int ContentHeight => Height - TitleBarH;

        // ── Saved position ────────────────────────────────────────────────────
        private int _savedX, _savedY, _savedWidth, _savedHeight;

        // ── Drag state ────────────────────────────────────────────────────────
        private bool _dragging;
        private int _dragOffX, _dragOffY;

        // ── Resize state ──────────────────────────────────────────────────────
        private bool _resizing;
        private int _resizeStartW, _resizeStartH;
        private int _resizeTouchStartX, _resizeTouchStartY;

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

        // ── Animation tick (call once per frame from Desktop) ─────────────────
        public void TickAnimation()
        {
            if (!_animating) return;

            _animFrame++;
            float t = (float)_animFrame / AnimFrames;
            if (t > 1.0f) t = 1.0f;

            // Ease-in für Minimize (beschleunigt), Ease-out für Restore
            float ease;
            if (_animMinimizing)
                ease = t * t;
            else
                ease = 1.0f - (1.0f - t) * (1.0f - t);

            Y = _animStartY + (int)((float)(_animTargetY - _animStartY) * ease);
            X = _animStartX + (int)((float)(_animTargetX - _animStartX) * ease);

            if (_animFrame >= AnimFrames)
            {
                _animating = false;
                Y = _animTargetY;
                X = _animTargetX;
                if (_animMinimizing)
                    IsMinimized = true;
            }
        }

        // ── State transitions ─────────────────────────────────────────────────
        public void Minimize(int targetX)
        {
            if (_animating) return;
            _animMinimizing = true;
            _animFrame = 0;
            _animating = true;
            _animStartY = Y;
            _animStartX = X;
            _animTargetY = 750;
            _animTargetX = targetX;
            _dragging = false;
            _resizing = false;
        }

        public void Restore(int fromX)
        {
            IsMinimized = false;
            _animMinimizing = false;
            _animFrame = 0;
            _animating = true;
            _animStartY = 750;
            _animStartX = fromX;
            _animTargetY = _savedY >= 0 ? _savedY : 60;
            _animTargetX = _savedX;
            Y = _animStartY;
            X = _animStartX;
        }

        private void ToggleMaximize()
        {
            if (IsMaximized)
            {
                X = _savedX;
                Y = _savedY;
                Width = _savedWidth;
                Height = _savedHeight;
                IsMaximized = false;
            }
            else
            {
                _savedX = X;
                _savedY = Y;
                _savedWidth = Width;
                _savedHeight = Height;
                X = 0;
                Y = 0;
                Width = 1280;
                Height = 680;
                IsMaximized = true;
                _dragging = false;
                _resizing = false;
            }
        }

        // ── Hit tests ─────────────────────────────────────────────────────────
        public bool HitTestGlobal(int tx, int ty)
        {
            if (IsMinimized) return false;
            return tx >= X && tx <= X + Width && ty >= Y && ty <= Y + Height;
        }

        private bool HitTestResize(int tx, int ty)
        {
            if (IsMaximized) return false;
            return tx >= X + Width - ResizeSize && tx <= X + Width
                && ty >= Y + Height - ResizeSize && ty <= Y + Height;
        }

        // ── Touch handling ────────────────────────────────────────────────────
        public bool HandleTouch(TouchState touch, bool touchBegan)
        {
            if (!touch.IsTouched)
            {
                _dragging = false;
                _resizing = false;
                for (int i = 0; i < _controlCount; i++)
                    _controls[i].HandleTouch(touch, false, X, ContentOffsetY);
                return false;
            }

            int tx = touch.X0;
            int ty = touch.Y0;

            if (touchBegan)
            {
                int by = Y + BtnPad;
                int cbX = X + Width - BtnPad - BtnSize;
                int mxX = X + Width - BtnPad - 2 * BtnSize - BtnGap;
                int mnX = X + Width - BtnPad - 3 * BtnSize - 2 * BtnGap;

                if (tx >= cbX && tx <= cbX + BtnSize && ty >= by && ty <= by + BtnSize)
                {
                    IsClosed = true;
                    return true;
                }
                if (tx >= mxX && tx <= mxX + BtnSize && ty >= by && ty <= by + BtnSize)
                {
                    ToggleMaximize();
                    return true;
                }
                if (tx >= mnX && tx <= mnX + BtnSize && ty >= by && ty <= by + BtnSize)
                {
                    _savedY = Y;
                    _savedX = X;
                    Minimize(X + Width / 2); // zur eigenen Mitte — Tab-X unbekannt hier
                    return true;
                }

                // Resize handle (before drag, so corner doesn't start drag)
                if (HitTestResize(tx, ty))
                {
                    _resizing = true;
                    _resizeStartW = Width;
                    _resizeStartH = Height;
                    _resizeTouchStartX = tx;
                    _resizeTouchStartY = ty;
                    return true;
                }

                // Title bar drag
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

            if (_resizing)
            {
                int dx = tx - _resizeTouchStartX;
                int dy = ty - _resizeTouchStartY;
                int nw = _resizeStartW + dx;
                int nh = _resizeStartH + dy;
                if (nw < MinWidth) nw = MinWidth;
                if (nh < MinHeight) nh = MinHeight;
                if (X + nw > 1280) nw = 1280 - X;
                if (Y + nh > 680) nh = 680 - Y;
                Width = nw;
                Height = nh;
                return true;
            }

            if (_dragging)
            {
                int nx = tx - _dragOffX;
                int ny = ty - _dragOffY;
                if (nx < 0) nx = 0;
                if (ny < 0) ny = 0;
                if (nx + Width > 1280) nx = 1280 - Width;
                if (ny + Height > 680) ny = 680 - Height;
                X = nx;
                Y = ny;
                return true;
            }

            for (int i = 0; i < _controlCount; i++)
                _controls[i].HandleTouch(touch, touchBegan, X, ContentOffsetY);

            return true;
        }

        // ── Drawing ───────────────────────────────────────────────────────────
        public void Draw(bool dimmed)
        {
            if (_animating && _animMinimizing && IsMinimized) return;

            // Drop shadow
            if (!IsMaximized)
                Graphics.FillRect(X + 4, Y + 4, Width, Height, Color.RGB(8, 8, 8));

            // Window body
            Graphics.FillRect(X, Y, Width, Height, Theme.Background);

            // Title bar
            uint titleBg = IsActive ? Theme.Primary : Theme.TitleBar;
            Graphics.FillRect(X, Y, Width, TitleBarH, titleBg);

            // Button positions
            int by = Y + BtnPad;
            int cbX = X + Width - BtnPad - BtnSize;
            int mxX = X + Width - BtnPad - 2 * BtnSize - BtnGap;
            int mnX = X + Width - BtnPad - 3 * BtnSize - 2 * BtnGap;

            // Title text
            int maxTitleW = mnX - (X + 10) - 6;
            string titleStr = Title;
            int textH = Graphics.MeasureTextHeight(2);
            int textY = Y + (TitleBarH - textH) / 2;
            while (titleStr.Length > 1 && Graphics.MeasureTextWidth(titleStr, 2) > maxTitleW)
                titleStr = titleStr.Substring(0, titleStr.Length - 1);
            if (titleStr.Length < Title.Length)
                titleStr = titleStr.Substring(0, titleStr.Length - 1) + "~";
            Graphics.DrawText(X + 10, textY, titleStr, Theme.TitleBarText, 2);

            // Close button
            Graphics.FillRect(cbX, by, BtnSize, BtnSize, Color.RGB(196, 43, 28));
            {
                int cx = cbX + BtnSize / 2;
                int cy = by + BtnSize / 2;
                int d = BtnSize / 2 - 4;
                Graphics.DrawLine(cx - d, cy - d, cx + d, cy + d, Color.White);
                Graphics.DrawLine(cx + d, cy - d, cx - d, cy + d, Color.White);
            }

            // Maximize button
            uint mxBg = IsMaximized ? Color.RGB(180, 95, 6) : Color.RGB(40, 160, 80);
            Graphics.FillRect(mxX, by, BtnSize, BtnSize, mxBg);
            {
                int p = 5;
                Graphics.DrawRect(mxX + p, by + p, BtnSize - 2 * p, BtnSize - 2 * p, Color.White);
                if (IsMaximized)
                {
                    int p2 = p - 2;
                    Graphics.DrawRect(mxX + p2 + 2, by + p2, BtnSize - 2 * p2 - 2, BtnSize - 2 * p2 - 2, Color.White);
                }
            }

            // Minimize button
            Graphics.FillRect(mnX, by, BtnSize, BtnSize, Color.RGB(90, 90, 90));
            {
                int barW2 = BtnSize - 8;
                Graphics.FillRect(mnX + 4, by + BtnSize - 6, barW2, 2, Color.White);
            }

            // Title bar / content separator
            Graphics.DrawLine(X, Y + TitleBarH, X + Width, Y + TitleBarH, Theme.Border);

            // Controls
            for (int i = 0; i < _controlCount; i++)
            {
                if (_controls[i].Visible)
                    _controls[i].Draw(X, ContentOffsetY);
            }

            // Resize handle (bottom-right triangle indicator)
            if (!IsMaximized)
            {
                int rx = X + Width - ResizeSize;
                int ry = Y + Height - ResizeSize;
                uint handleColor = _resizing ? Color.RGB(0, 120, 215) : Color.RGB(80, 80, 85);
                Graphics.DrawLine(rx + ResizeSize, ry + 4, rx + 4, ry + ResizeSize, handleColor);
                Graphics.DrawLine(rx + ResizeSize, ry + 8, rx + 8, ry + ResizeSize, handleColor);
                Graphics.DrawLine(rx + ResizeSize, ry + 12, rx + 12, ry + ResizeSize, handleColor);
            }

            // Window border
            uint borderColor = IsActive ? Theme.Primary : Theme.Border;
            Graphics.DrawRect(X, Y, Width, Height, borderColor);

            // Dim overlay for inactive windows
            if (dimmed)
                Graphics.FillRectAlpha(X, Y, Width, Height, Color.RGB(0, 0, 0), 80);
        }

        // Keep old signature working (active window — never dimmed)
        public void Draw() => Draw(false);
    }
}