using System;
using multiWindow.Models;

namespace multiWindow.UI
{
    public class Desktop
    {
        public TaskBar TaskBar { get; }
        public StartMenu StartMenu { get; }
        public ContextMenu ContextMenu { get; }

        public ThemeConfig DefaultTheme { get; set; }

        private const int MaxWindows = 8;
        private Window[] _windows = new Window[MaxWindows];
        private int _count;

        private bool _prevTouched;
        private Window _touchOwner;
        private bool _taskBarOwned;

        // ── Long-press detection ──────────────────────────────────────────────
        private const int LongPressFrames = 45; // ~0.75s at 60fps
        private int _touchHoldFrames;
        private int _touchHoldX, _touchHoldY;
        private bool _longPressFired;

        public Desktop()
        {
            DefaultTheme = ThemeConfig.Dark();
            TaskBar = new TaskBar();
            StartMenu = new StartMenu();
            ContextMenu = new ContextMenu();

            TaskBar.OnStartTapped = () =>
            {
                ContextMenu.Close();
                StartMenu.Toggle();
            };
            TaskBar.OnTabTapped = win =>
            {
                if (win.IsMinimized)
                    FocusWindow(win);
                else if (win.IsActive)
                    win.Minimize(TaskBar.GetTabX(win));
                else
                    FocusWindow(win);
            };
        }

        // ── Window management ─────────────────────────────────────────────────
        public void AddWindow(Window win)
        {
            if (_count >= MaxWindows) return;
            for (int i = 0; i < _count; i++)
                if (_windows[i] == win) return;

            win.IsClosed = false;
            win.Theme = DefaultTheme;
            _windows[_count++] = win;
            TaskBar.RegisterWindow(win);
            FocusWindow(win);
        }

        private void FocusWindow(Window target)
        {
            int tabX = TaskBar.GetTabX(target);

            if (target.IsMinimized) target.Restore(tabX);

            int idx = -1;
            for (int i = 0; i < _count; i++)
                if (_windows[i] == target) { idx = i; break; }
            if (idx < 0) return;

            Window w = _windows[idx];
            for (int i = idx; i < _count - 1; i++)
                _windows[i] = _windows[i + 1];
            _windows[_count - 1] = w;

            UpdateActiveFlags();
        }

        private void UpdateActiveFlags()
        {
            int topActive = -1;
            for (int i = _count - 1; i >= 0; i--)
                if (!_windows[i].IsMinimized && !_windows[i].IsAnimating) { topActive = i; break; }
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
                        _touchOwner = null;
                        _taskBarOwned = false;
                    }
                }
                else
                {
                    _windows[write++] = _windows[i];
                }
            }
            _count = write;
            UpdateActiveFlags();
        }

        // ── Per-frame update ──────────────────────────────────────────────────
        public void Update(TouchState touch)
        {
            bool isTouched = touch.IsTouched;
            bool touchBegan = isTouched && !_prevTouched;
            bool touchEnded = !isTouched && _prevTouched;
            _prevTouched = isTouched;

            // Tick animations every frame
            for (int i = 0; i < _count; i++)
                _windows[i].TickAnimation();

            RemoveClosed();

            // Long-press detection on desktop background
            if (isTouched && !touchBegan)
            {
                if (!_longPressFired)
                {
                    _touchHoldFrames++;
                    if (_touchHoldFrames >= LongPressFrames)
                    {
                        _longPressFired = true;
                        // Only fire if touch is on empty desktop (not on any window or taskbar)
                        bool onWindow = false;
                        for (int i = _count - 1; i >= 0; i--)
                            if (_windows[i].HitTestGlobal(_touchHoldX, _touchHoldY)) { onWindow = true; break; }
                        if (!onWindow && _touchHoldY < 680)
                        {
                            ContextMenu.Open(_touchHoldX, _touchHoldY);
                            StartMenu.Close();
                        }
                    }
                }
            }

            if (touchBegan)
            {
                _touchHoldFrames = 0;
                _longPressFired = false;
                _touchHoldX = touch.X0;
                _touchHoldY = touch.Y0;
            }

            if (touchEnded)
            {
                _touchHoldFrames = 0;
                _longPressFired = false;
                _touchOwner = null;
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

                // Context menu is topmost modal
                if (ContextMenu.IsOpen)
                {
                    ContextMenu.HandleTouch(touch, true);
                    return;
                }

                if (StartMenu.IsOpen)
                {
                    StartMenu.HandleTouch(touch, true);
                    return;
                }

                if (ty >= 680)
                {
                    _taskBarOwned = true;
                    _touchOwner = null;
                    TaskBar.HandleTouch(touch, true);
                    return;
                }

                for (int i = _count - 1; i >= 0; i--)
                {
                    if (_windows[i].HitTestGlobal(tx, ty))
                    {
                        ContextMenu.Close();
                        FocusWindow(_windows[i]);
                        _touchOwner = _windows[_count - 1];
                        _touchOwner.HandleTouch(touch, true);
                        return;
                    }
                }

                ContextMenu.Close();
                StartMenu.Close();
            }
            else
            {
                if (ContextMenu.IsOpen)
                {
                    ContextMenu.HandleTouch(touch, false);
                    return;
                }

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
            // Wallpaper
            Graphics.FillRect(0, 0, 1280, 680, Color.RGB(15, 20, 35));
            for (int gx = 0; gx < 1280; gx += 80)
                Graphics.DrawLine(gx, 0, gx, 680, Color.RGB(20, 28, 45));
            for (int gy = 0; gy < 680; gy += 80)
                Graphics.DrawLine(0, gy, 1280, gy, Color.RGB(20, 28, 45));

            // Windows back to front; all but topmost active get dimmed
            for (int i = 0; i < _count; i++)
            {
                if (_windows[i].IsMinimized && !_windows[i].IsAnimating) continue;
                bool dimmed = (i < _count - 1); // every window except the topmost
                _windows[i].Draw(dimmed);
            }

            TaskBar.Draw();

            if (StartMenu.IsOpen)
                StartMenu.Draw();

            if (ContextMenu.IsOpen)
                ContextMenu.Draw();
        }
    }
}