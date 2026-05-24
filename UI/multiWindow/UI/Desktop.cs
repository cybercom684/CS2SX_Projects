using System;
using multiWindow.Models;

namespace multiWindow.UI
{
    public class Desktop
    {
        // ── Sub-systems ───────────────────────────────────────────────────────
        public TaskBar   TaskBar   { get; }
        public StartMenu StartMenu { get; }

        // ── Default theme applied to new windows ──────────────────────────────
        public ThemeConfig DefaultTheme { get; set; }

        // ── Window z-ordered array (index 0 = back, last = front/active) ──────
        private const int MaxWindows = 8;
        private Window[] _windows    = new Window[MaxWindows];
        private int      _count;

        // ── Touch routing state ───────────────────────────────────────────────
        private bool   _prevTouched;
        private Window _touchOwner;   // window that owns the current gesture
        private bool   _taskBarOwned; // true when taskbar owns current gesture

        // ── Constructor ───────────────────────────────────────────────────────
        public Desktop()
        {
            DefaultTheme = ThemeConfig.Dark();
            TaskBar      = new TaskBar();
            StartMenu    = new StartMenu();

            TaskBar.OnStartTapped = () => StartMenu.Toggle();
            TaskBar.OnTabTapped   = win =>
            {
                if (win.IsMinimized)
                    FocusWindow(win);       // restore + bring to front
                else if (win.IsActive)
                    win.Minimize();         // minimize active window via taskbar
                else
                    FocusWindow(win);       // focus inactive window
            };
        }

        // ── Window management ─────────────────────────────────────────────────
        public void AddWindow(Window win)
        {
            if (_count >= MaxWindows) return;

            // Skip if already registered
            for (int i = 0; i < _count; i++)
                if (_windows[i] == win) return;

            win.IsClosed = false;
            win.Theme = DefaultTheme;
            _windows[_count++] = win;
            TaskBar.RegisterWindow(win);
            FocusWindow(win); // bring to front and activate
        }

        private void FocusWindow(Window target)
        {
            // Un-minimize if needed
            if (target.IsMinimized) target.Restore();

            // Find the window
            int idx = -1;
            for (int i = 0; i < _count; i++)
                if (_windows[i] == target) { idx = i; break; }
            if (idx < 0) return;

            // Rotate to end (highest z-order)
            Window w = _windows[idx];
            for (int i = idx; i < _count - 1; i++)
                _windows[i] = _windows[i + 1];
            _windows[_count - 1] = w;

            // Mark only the topmost non-minimized window as active
            UpdateActiveFlags();
        }

        private void UpdateActiveFlags()
        {
            int topActive = -1;
            for (int i = _count - 1; i >= 0; i--)
                if (!_windows[i].IsMinimized) { topActive = i; break; }
            for (int i = 0; i < _count; i++)
                _windows[i].IsActive = (i == topActive);
        }

        private void RemoveClosed()
        {
            int write = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_windows[i].IsClosed)
                {
                    TaskBar.UnregisterWindow(_windows[i]);
                    if (_touchOwner == _windows[i])
                    {
                        _touchOwner  = null;
                        _taskBarOwned = false;
                    }
                }
                else
                {
                    _windows[write++] = _windows[i];
                }
            }
            _count = write;

            // Re-evaluate active flag (skip minimized windows)
            UpdateActiveFlags();
        }

        // ── Per-frame update ──────────────────────────────────────────────────
        public void Update(TouchState touch)
        {
            bool isTouched  = touch.IsTouched;
            bool touchBegan = isTouched && !_prevTouched;
            bool touchEnded = !isTouched && _prevTouched;
            _prevTouched = isTouched;

            RemoveClosed();

            // On release: reset all stateful controls in every window
            if (touchEnded)
            {
                _touchOwner  = null;
                _taskBarOwned = false;
                for (int i = 0; i < _count; i++)
                    _windows[i].HandleTouch(touch, false);
                return;
            }

            if (!isTouched) return;

            if (touchBegan)
            {
                int tx = touch.X0;
                int ty = touch.Y0;

                // Start menu is a modal overlay — route first
                if (StartMenu.IsOpen)
                {
                    StartMenu.HandleTouch(touch, true);
                    return;
                }

                // Taskbar area (y >= 680)
                if (ty >= 680)
                {
                    _taskBarOwned = true;
                    _touchOwner  = null;
                    TaskBar.HandleTouch(touch, true);
                    return;
                }

                // Find the topmost window that was hit (iterate from front)
                for (int i = _count - 1; i >= 0; i--)
                {
                    if (_windows[i].HitTestGlobal(tx, ty))
                    {
                        FocusWindow(_windows[i]);
                        _touchOwner = _windows[_count - 1]; // now at top after FocusWindow
                        _touchOwner.HandleTouch(touch, true);
                        return;
                    }
                }

                // Tapped on empty desktop background → close start menu
                StartMenu.Close();
            }
            else
            {
                // Continuing gesture — route to owner
                if (StartMenu.IsOpen)
                {
                    StartMenu.HandleTouch(touch, false);
                    return;
                }

                if (_taskBarOwned)
                {
                    TaskBar.HandleTouch(touch, false);
                    return;
                }

                if (_touchOwner != null)
                    _touchOwner.HandleTouch(touch, false);
            }
        }

        // ── Drawing ───────────────────────────────────────────────────────────
        public void Draw()
        {
            // Desktop wallpaper gradient (dark blue → near-black)
            Graphics.FillRect(0, 0, 1280, 680, Color.RGB(15, 20, 35));

            // Subtle grid pattern (optional visual depth)
            for (int gx = 0; gx < 1280; gx += 80)
                Graphics.DrawLine(gx, 0, gx, 680, Color.RGB(20, 28, 45));
            for (int gy = 0; gy < 680; gy += 80)
                Graphics.DrawLine(0, gy, 1280, gy, Color.RGB(20, 28, 45));

            // Windows (back to front — lowest index first, skip minimized)
            for (int i = 0; i < _count; i++)
                if (!_windows[i].IsMinimized) _windows[i].Draw();

            // Taskbar always above windows
            TaskBar.Draw();

            // Start menu as topmost overlay
            if (StartMenu.IsOpen)
                StartMenu.Draw();
        }
    }
}
