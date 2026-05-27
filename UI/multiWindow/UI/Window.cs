using multiWindow.Models;
using multiWindow.UI.Controls;

namespace multiWindow.UI
{
    public class Window
    {
        // ── Layout ────────────────────────────────────────────────────────────
        public int X      { get; set; }
        public int Y      { get; set; }
        public int Width  { get; set; }
        public int Height { get; set; }

        // ── Appearance ────────────────────────────────────────────────────────
        public string     Title            { get; set; } = "Fenster";
        public ThemeConfig Theme           { get; set; }
        public bool        IsActive        { get; set; }
        public uint        TitleAccentColor { get; set; } = 0; // 0 = keiner

        // ── State ─────────────────────────────────────────────────────────────
        public bool IsClosed    { get; private set; }
        public bool IsMinimized { get; private set; }
        public bool IsMaximized { get; private set; }

        // ── Minimize/Restore animation ────────────────────────────────────────
        private const int AnimFrames = 8;
        private int  _animFrame;
        private bool _animating;
        private bool _animMinimizing;
        private int  _animTargetY, _animStartY;
        private int  _animTargetX, _animStartX;

        public bool IsAnimating => _animating;

        // ── Focus glow animation ──────────────────────────────────────────────
        private const int FocusGlowTotal = 10;
        private int _focusGlow;

        public void TriggerFocusPop() => _focusGlow = FocusGlowTotal;

        // ── Layout constants ──────────────────────────────────────────────────
        private const int TitleBarH  = 30;
        private const int BtnSize    = 22;
        private const int BtnPad     = 4;
        private const int BtnGap     = 2;
        private const int MinWidth   = 160;
        private const int MinHeight  = 80;
        private const int CornerZone = 16; // px corner hit zone
        private const int EdgeZone   = 8;  // px edge hit zone

        // ── Controls ──────────────────────────────────────────────────────────
        private UIControl[] _controls = new UIControl[32];
        private int _controlCount;
        public int ControlCount => _controlCount;

        private int ContentOffsetY => Y + TitleBarH;

        // ── Saved positions (maximize / minimize restore) ─────────────────────
        private int _savedX, _savedY, _savedWidth, _savedHeight;

        // ── Drag ──────────────────────────────────────────────────────────────
        private bool _dragging;
        private int  _dragOffX, _dragOffY;

        // ── Aero Snap ─────────────────────────────────────────────────────────
        private bool _isSnapped;
        private int  _snapSavedX, _snapSavedY, _snapSavedW, _snapSavedH;

        public bool HasSnapPreview { get; private set; }
        public int  SnapPreviewX   { get; private set; }
        public int  SnapPreviewY   { get; private set; }
        public int  SnapPreviewW   { get; private set; }
        public int  SnapPreviewH   { get; private set; }

        // ── Resize ────────────────────────────────────────────────────────────
        // modes: 0=none 1=R 2=B 3=L 4=T 5=BR 6=BL 7=TR 8=TL
        private int _resizeMode;
        private int _resizeStartX, _resizeStartY, _resizeStartW, _resizeStartH;
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

        // ── Per-frame tick ────────────────────────────────────────────────────
        public void TickAnimation()
        {
            if (_focusGlow > 0) _focusGlow--;

            if (!_animating) return;

            _animFrame++;
            float t = (float)_animFrame / AnimFrames;
            if (t > 1.0f) t = 1.0f;

            float ease = _animMinimizing
                ? t * t
                : 1.0f - (1.0f - t) * (1.0f - t);

            Y = _animStartY + (int)((float)(_animTargetY - _animStartY) * ease);
            X = _animStartX + (int)((float)(_animTargetX - _animStartX) * ease);

            if (_animFrame >= AnimFrames)
            {
                _animating = false;
                Y = _animTargetY;
                X = _animTargetX;
                if (_animMinimizing) IsMinimized = true;
            }
        }

        // ── State transitions ─────────────────────────────────────────────────
        public void Minimize(int targetX)
        {
            if (_animating) return;
            _animMinimizing  = true;
            _animFrame       = 0;
            _animating       = true;
            _animStartY      = Y;
            _animStartX      = X;
            _animTargetY     = 750;
            _animTargetX     = targetX;
            _dragging        = false;
            _resizeMode      = 0;
        }

        public void MinimizeTo(int targetX)
        {
            _savedY = Y;
            _savedX = X;
            Minimize(targetX);
        }

        public void Restore(int fromX)
        {
            IsMinimized     = false;
            _animMinimizing = false;
            _animFrame      = 0;
            _animating      = true;
            _animStartY     = 750;
            _animStartX     = fromX;
            _animTargetY    = _savedY >= 0 ? _savedY : 60;
            _animTargetX    = _savedX;
            Y               = _animStartY;
            X               = _animStartX;
        }

        public void ForceNormal(int x, int y, int w, int h)
        {
            X           = x;  Y          = y;
            Width       = w;  Height     = h;
            IsMaximized = false;
            _isSnapped  = false;
            HasSnapPreview = false;
            _dragging   = false;
            _resizeMode = 0;
        }

        private void ToggleMaximize()
        {
            if (IsMaximized)
            {
                X = _savedX; Y = _savedY;
                Width = _savedWidth; Height = _savedHeight;
                IsMaximized = false;
            }
            else
            {
                _savedX = X; _savedY = Y;
                _savedWidth = Width; _savedHeight = Height;
                X = 0; Y = 0; Width = 1280; Height = 680;
                IsMaximized = true;
                _dragging   = false;
                _resizeMode = 0;
            }
        }

        private void ApplySnap(int sx, int sy, int sw, int sh)
        {
            _snapSavedX = X; _snapSavedY = Y;
            _snapSavedW = Width; _snapSavedH = Height;
            X = sx; Y = sy; Width = sw; Height = sh;
            _isSnapped = true;
        }

        private void Unsnap(int touchX)
        {
            int oldW = _snapSavedW;
            X      = touchX - oldW / 2;
            Y      = _snapSavedY;
            Width  = oldW;
            Height = _snapSavedH;
            if (X < 0)          X = 0;
            if (X + Width > 1280) X = 1280 - Width;
            _isSnapped  = false;
            _dragOffX   = touchX - X;
            _dragOffY   = 10;
        }

        // ── Hit tests ─────────────────────────────────────────────────────────
        public bool HitTestGlobal(int tx, int ty)
        {
            if (IsMinimized) return false;
            return tx >= X && tx <= X + Width && ty >= Y && ty <= Y + Height;
        }

        // Returns resize mode (0 = no hit)
        private int HitTestResize(int tx, int ty)
        {
            if (IsMaximized || _isSnapped) return 0;
            if (!HitTestGlobal(tx, ty))    return 0;

            bool nearLeft   = tx <= X + CornerZone;
            bool nearRight  = tx >= X + Width  - CornerZone;
            bool nearTop    = ty <= Y + CornerZone;
            bool nearBottom = ty >= Y + Height - CornerZone;

            bool onLEdge = tx <= X + EdgeZone  && !nearTop && !nearBottom;
            bool onREdge = tx >= X + Width - EdgeZone && !nearTop && !nearBottom;
            bool onTEdge = ty <= Y + EdgeZone  && !nearLeft && !nearRight;
            bool onBEdge = ty >= Y + Height - EdgeZone && !nearLeft && !nearRight;

            if (nearLeft  && nearTop)    return 8; // TL
            if (nearRight && nearTop)    return 7; // TR
            if (nearLeft  && nearBottom) return 6; // BL
            if (nearRight && nearBottom) return 5; // BR
            if (onLEdge) return 3;
            if (onREdge) return 1;
            if (onTEdge) return 4;
            if (onBEdge) return 2;
            return 0;
        }

        // ── Touch handling ────────────────────────────────────────────────────
        public bool HandleTouch(TouchState touch, bool touchBegan)
        {
            if (!touch.IsTouched)
            {
                // Apply snap if drag ended in a snap zone
                if (_dragging && HasSnapPreview)
                    ApplySnap(SnapPreviewX, SnapPreviewY, SnapPreviewW, SnapPreviewH);

                HasSnapPreview = false;
                _dragging      = false;
                _resizeMode    = 0;

                for (int i = 0; i < _controlCount; i++)
                    _controls[i].HandleTouch(touch, false, X, ContentOffsetY);
                return false;
            }

            int tx = touch.X0;
            int ty = touch.Y0;

            if (touchBegan)
            {
                int by  = Y + BtnPad;
                int cbX = X + Width - BtnPad - BtnSize;
                int mxX = X + Width - BtnPad - 2 * BtnSize - BtnGap;
                int mnX = X + Width - BtnPad - 3 * BtnSize - 2 * BtnGap;

                // Close
                if (tx >= cbX && tx <= cbX + BtnSize && ty >= by && ty <= by + BtnSize)
                { IsClosed = true; return true; }

                // Maximize
                if (tx >= mxX && tx <= mxX + BtnSize && ty >= by && ty <= by + BtnSize)
                { ToggleMaximize(); return true; }

                // Minimize
                if (tx >= mnX && tx <= mnX + BtnSize && ty >= by && ty <= by + BtnSize)
                { MinimizeTo(X + Width / 2); return true; }

                // Resize
                int rm = HitTestResize(tx, ty);
                if (rm != 0)
                {
                    _resizeMode         = rm;
                    _resizeStartX       = X;   _resizeStartY = Y;
                    _resizeStartW       = Width; _resizeStartH = Height;
                    _resizeTouchStartX  = tx;  _resizeTouchStartY = ty;
                    return true;
                }

                // Title bar drag
                if (!IsMaximized
                 && tx >= X && tx <= X + Width
                 && ty >= Y && ty <= Y + TitleBarH)
                {
                    if (_isSnapped) Unsnap(tx);
                    _dragging  = true;
                    _dragOffX  = tx - X;
                    _dragOffY  = ty - Y;
                    return true;
                }
            }

            if (_resizeMode != 0)
            {
                ApplyResize(tx - _resizeTouchStartX, ty - _resizeTouchStartY);
                return true;
            }

            if (_dragging)
            {
                int nx = tx - _dragOffX;
                int ny = ty - _dragOffY;
                if (nx < 0)          nx = 0;
                if (ny < 0)          ny = 0;
                if (nx + Width  > 1280) nx = 1280 - Width;
                if (ny + Height > 680)  ny = 680  - Height;
                X = nx;
                Y = ny;
                UpdateSnapPreview();
                return true;
            }

            for (int i = 0; i < _controlCount; i++)
                _controls[i].HandleTouch(touch, touchBegan, X, ContentOffsetY);
            return true;
        }

        private void ApplyResize(int dx, int dy)
        {
            int nx = _resizeStartX;
            int ny = _resizeStartY;
            int nw = _resizeStartW;
            int nh = _resizeStartH;

            if (_resizeMode == 1 || _resizeMode == 5 || _resizeMode == 7) nw += dx;
            if (_resizeMode == 2 || _resizeMode == 5 || _resizeMode == 6) nh += dy;
            if (_resizeMode == 3 || _resizeMode == 6 || _resizeMode == 8) { nx += dx; nw -= dx; }
            if (_resizeMode == 4 || _resizeMode == 7 || _resizeMode == 8) { ny += dy; nh -= dy; }

            bool leftEdge = _resizeMode == 3 || _resizeMode == 6 || _resizeMode == 8;
            bool topEdge  = _resizeMode == 4 || _resizeMode == 7 || _resizeMode == 8;

            if (nw < MinWidth)   { if (leftEdge) nx = _resizeStartX + _resizeStartW - MinWidth; nw = MinWidth; }
            if (nh < MinHeight)  { if (topEdge)  ny = _resizeStartY + _resizeStartH - MinHeight; nh = MinHeight; }
            if (nx < 0)          { nw += nx; nx = 0; }
            if (ny < 0)          { nh += ny; ny = 0; }
            if (nx + nw > 1280)  nw = 1280 - nx;
            if (ny + nh > 680)   nh = 680  - ny;
            if (nw < MinWidth)   nw = MinWidth;
            if (nh < MinHeight)  nh = MinHeight;

            X = nx; Y = ny; Width = nw; Height = nh;
        }

        private void UpdateSnapPreview()
        {
            HasSnapPreview = false;
            if (X <= 20)
            {
                HasSnapPreview = true;
                SnapPreviewX = 0;  SnapPreviewY = 0;
                SnapPreviewW = 640; SnapPreviewH = 680;
            }
            else if (X + Width >= 1260)
            {
                HasSnapPreview = true;
                SnapPreviewX = 640; SnapPreviewY = 0;
                SnapPreviewW = 640; SnapPreviewH = 680;
            }
            else if (Y <= 8)
            {
                HasSnapPreview = true;
                SnapPreviewX = 0;    SnapPreviewY = 0;
                SnapPreviewW = 1280; SnapPreviewH = 680;
            }
        }

        // ── Drawing ───────────────────────────────────────────────────────────
        public void Draw(bool dimmed)
        {
            if (_animating && _animMinimizing && IsMinimized) return;

            // ── Feature 3: Deep layered drop shadow ────────────────────────────
            if (!IsMaximized)
            {
                Graphics.FillRectAlpha(X + 10, Y + 10, Width, Height, Color.Black, 38);
                Graphics.FillRectAlpha(X + 6,  Y + 6,  Width, Height, Color.Black, 60);
                Graphics.FillRectAlpha(X + 3,  Y + 3,  Width, Height, Color.Black, 92);
            }

            // ── Feature 9: Focus glow pop ──────────────────────────────────────
            if (_focusGlow > 0)
            {
                byte glowAlpha = (byte)(_focusGlow * 18);
                Graphics.FillRectAlpha(X - 2, Y - 2, Width + 4, Height + 4,
                                       Color.RGB(0, 120, 215), glowAlpha);
            }

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

            // ── Feature 7: Title accent color dot ─────────────────────────────
            int titleTextX = X + 10;
            if (TitleAccentColor != 0)
            {
                int dotSz = 14;
                int dotX  = X + 8;
                int dotY  = Y + (TitleBarH - dotSz) / 2;
                Graphics.FillRoundedRect(dotX, dotY, dotSz, dotSz, 3, TitleAccentColor);
                titleTextX = dotX + dotSz + 6;
            }

            // Title text
            int maxTitleW = mnX - titleTextX - 6;
            string titleStr = Title;
            int textH = Graphics.MeasureTextHeight(2);
            int textY = Y + (TitleBarH - textH) / 2;
            while (titleStr.Length > 1 && Graphics.MeasureTextWidth(titleStr, 2) > maxTitleW)
                titleStr = titleStr.Substring(0, titleStr.Length - 1);
            if (titleStr.Length < Title.Length)
                titleStr = titleStr.Substring(0, titleStr.Length - 1) + "~";
            Graphics.DrawText(titleTextX, textY, titleStr, Theme.TitleBarText, 2);

            // Close button
            Graphics.FillRect(cbX, by, BtnSize, BtnSize, Color.RGB(196, 43, 28));
            { int cx = cbX + BtnSize / 2; int cy = by + BtnSize / 2; int d = BtnSize / 2 - 4;
              Graphics.DrawLine(cx - d, cy - d, cx + d, cy + d, Color.White);
              Graphics.DrawLine(cx + d, cy - d, cx - d, cy + d, Color.White); }

            // Maximize button
            uint mxBg = IsMaximized ? Color.RGB(180, 95, 6) : Color.RGB(40, 160, 80);
            Graphics.FillRect(mxX, by, BtnSize, BtnSize, mxBg);
            { int p = 5; Graphics.DrawRect(mxX + p, by + p, BtnSize - 2 * p, BtnSize - 2 * p, Color.White);
              if (IsMaximized) { int p2 = p - 2; Graphics.DrawRect(mxX + p2 + 2, by + p2, BtnSize - 2 * p2 - 2, BtnSize - 2 * p2 - 2, Color.White); } }

            // Minimize button
            Graphics.FillRect(mnX, by, BtnSize, BtnSize, Color.RGB(90, 90, 90));
            { int bw = BtnSize - 8; Graphics.FillRect(mnX + 4, by + BtnSize - 6, bw, 2, Color.White); }

            // Separator
            Graphics.DrawLine(X, Y + TitleBarH, X + Width, Y + TitleBarH, Theme.Border);

            // Controls
            for (int i = 0; i < _controlCount; i++)
                if (_controls[i].Visible) _controls[i].Draw(X, ContentOffsetY);

            // ── Feature 10: All-edge resize handles ────────────────────────────
            if (!IsMaximized && !_isSnapped)
                DrawResizeHandles();

            // Window border
            uint borderColor = IsActive ? Theme.Primary : Theme.Border;
            Graphics.DrawRect(X, Y, Width, Height, borderColor);

            // Dim overlay for inactive windows
            if (dimmed)
                Graphics.FillRectAlpha(X, Y, Width, Height, Color.RGB(0, 0, 0), 80);
        }

        private void DrawResizeHandles()
        {
            uint col       = Color.RGB(78, 78, 86);
            uint colActive = Color.RGB(0, 120, 215);

            uint brCol  = _resizeMode == 5 ? colActive : col;
            uint blCol  = _resizeMode == 6 ? colActive : col;
            uint trCol  = _resizeMode == 7 ? colActive : col;
            uint tlCol  = _resizeMode == 8 ? colActive : col;

            // BR (triangle stripes — existing style)
            int rx = X + Width - 16;
            int ry = Y + Height - 16;
            Graphics.DrawLine(rx + 16, ry + 4,  rx + 4,  ry + 16, brCol);
            Graphics.DrawLine(rx + 16, ry + 8,  rx + 8,  ry + 16, brCol);
            Graphics.DrawLine(rx + 16, ry + 12, rx + 12, ry + 16, brCol);

            // BL
            DrawCornerL(X,          Y + Height, 1,  -1, blCol);
            // TR
            DrawCornerL(X + Width,  Y,          -1, 1,  trCol);
            // TL
            DrawCornerL(X,          Y,           1,  1,  tlCol);
        }

        private void DrawCornerL(int cx, int cy, int dx, int dy, uint color)
        {
            int len = 8;
            Graphics.DrawLine(cx,           cy, cx + dx * len, cy,           color);
            Graphics.DrawLine(cx,           cy, cx,            cy + dy * len, color);
        }

        public void Draw() => Draw(false);
    }
}
