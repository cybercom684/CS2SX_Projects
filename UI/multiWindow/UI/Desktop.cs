using System;
using multiWindow.Models;

namespace multiWindow.UI
{
    public class Desktop
    {
        public TaskBar      TaskBar      { get; }
        public StartMenu    StartMenu    { get; }
        public ContextMenu  ContextMenu  { get; }
        public ThemeConfig  DefaultTheme { get; set; }

        // ── Sub-systems ───────────────────────────────────────────────────────
        private readonly ToastManager  _toasts        = new ToastManager();
        private readonly QuickSettings _quickSettings = new QuickSettings();
        private readonly WindowSwitcher _switcher     = new WindowSwitcher();

        // ── Desktop icons ─────────────────────────────────────────────────────
        private const int MaxIcons = 12;
        private DesktopIcon[] _icons = new DesktopIcon[MaxIcons];
        private int _iconCount;

        public void AddDesktopIcon(DesktopIcon icon)
        {
            if (_iconCount < MaxIcons)
                _icons[_iconCount++] = icon;
        }

        public void ShowToast(string title, string message)
            => _toasts.Show(title, message);

        // ── Windows ───────────────────────────────────────────────────────────
        private const int MaxWindows = 8;
        private Window[] _windows = new Window[MaxWindows];
        private int _count;

        // ── Input state ───────────────────────────────────────────────────────
        private bool   _prevTouched;
        private Window _touchOwner;
        private bool   _taskBarOwned;

        // ── Long-press detection ──────────────────────────────────────────────
        private const int LongPressFrames = 45;
        private int  _touchHoldFrames;
        private int  _touchHoldX, _touchHoldY;
        private bool _longPressFired;

        // ── Constructor ───────────────────────────────────────────────────────
        public Desktop()
        {
            DefaultTheme = ThemeConfig.Dark();
            TaskBar      = new TaskBar();
            StartMenu    = new StartMenu();
            ContextMenu  = new ContextMenu();

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
                    win.MinimizeTo(TaskBar.GetTabX(win));
                else
                    FocusWindow(win);
            };

            TaskBar.OnSystemTrayTapped = () =>
            {
                ContextMenu.Close();
                StartMenu.Close();
                _quickSettings.Toggle();
            };
        }

        // ── Window management ─────────────────────────────────────────────────
        public void AddWindow(Window win)
        {
            if (_count >= MaxWindows) return;
            for (int i = 0; i < _count; i++)
                if (_windows[i] == win) return;

            win.IsClosed = false;
            win.Theme    = DefaultTheme;
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

            w.TriggerFocusPop();
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
            UpdateActiveFlags();
        }

        // ── Window arrangement ────────────────────────────────────────────────
        public void CascadeWindows()
        {
            int x = 30;
            int y = 30;
            for (int i = 0; i < _count; i++)
            {
                if (_windows[i].IsMinimized) continue;
                int w = _windows[i].Width;
                int h = _windows[i].Height;
                _windows[i].ForceNormal(x, y, w, h);
                x += 25; y += 25;
                if (x + 200 > 1280) x = 30;
                if (y + 60  > 640)  y = 30;
            }
        }

        public void TileHorizontal()
        {
            int vis = 0;
            for (int i = 0; i < _count; i++)
                if (!_windows[i].IsMinimized) vis++;
            if (vis == 0) return;
            int slotW = 1280 / vis;
            int slot  = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_windows[i].IsMinimized) continue;
                _windows[i].ForceNormal(slot * slotW, 0, slotW, 680);
                slot++;
            }
        }

        public void TileVertical()
        {
            int vis = 0;
            for (int i = 0; i < _count; i++)
                if (!_windows[i].IsMinimized) vis++;
            if (vis == 0) return;
            int slotH = 680 / vis;
            int slot  = 0;
            for (int i = 0; i < _count; i++)
            {
                if (_windows[i].IsMinimized) continue;
                _windows[i].ForceNormal(0, slot * slotH, 1280, slotH);
                slot++;
            }
        }

        public void MinimizeAll()
        {
            for (int i = 0; i < _count; i++)
                if (!_windows[i].IsMinimized && !_windows[i].IsAnimating)
                    _windows[i].MinimizeTo(TaskBar.GetTabX(_windows[i]));
        }

        // ── Per-frame update ──────────────────────────────────────────────────
        public void Update(TouchState touch)
        {
            bool isTouched  = touch.IsTouched;
            bool touchBegan = isTouched && !_prevTouched;
            bool touchEnded = !isTouched && _prevTouched;
            _prevTouched = isTouched;

            // Tick window animations every frame
            for (int i = 0; i < _count; i++)
                _windows[i].TickAnimation();

            RemoveClosed();
            _toasts.Update();

            // ── Gamepad: L = window switcher ──────────────────────────────────
            if (Input.IsDown(NpadButton.L))
            {
                if (_switcher.IsOpen)
                    _switcher.Close();
                else
                    _switcher.Open(_windows, _count, w => FocusWindow(w));
            }

            // ── Long-press on desktop background ──────────────────────────────
            if (isTouched && !touchBegan && !_longPressFired)
            {
                _touchHoldFrames++;
                if (_touchHoldFrames >= LongPressFrames)
                {
                    _longPressFired = true;
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

            if (touchBegan)
            {
                _touchHoldFrames = 0;
                _longPressFired  = false;
                _touchHoldX      = touch.X0;
                _touchHoldY      = touch.Y0;
            }

            if (touchEnded)
            {
                _touchHoldFrames = 0;
                _longPressFired  = false;
                _touchOwner      = null;
                _taskBarOwned    = false;
                for (int i = 0; i < _count; i++)
                    _windows[i].HandleTouch(touch, false);
                return;
            }

            if (!isTouched) return;

            if (touchBegan)
            {
                int tx = touch.X0;
                int ty = touch.Y0;

                // Window switcher is topmost modal
                if (_switcher.IsOpen)
                {
                    _switcher.HandleTouch(touch, true);
                    return;
                }

                // Quick settings
                if (_quickSettings.IsOpen)
                {
                    _quickSettings.HandleTouch(touch, true);
                    return;
                }

                // Context menu
                if (ContextMenu.IsOpen)
                {
                    ContextMenu.HandleTouch(touch, true);
                    return;
                }

                // Start menu
                if (StartMenu.IsOpen)
                {
                    StartMenu.HandleTouch(touch, true);
                    return;
                }

                // Taskbar
                if (ty >= 680)
                {
                    _taskBarOwned = true;
                    _touchOwner   = null;
                    TaskBar.HandleTouch(touch, true);
                    return;
                }

                // Windows (top to bottom)
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

                // Desktop icons
                for (int i = 0; i < _iconCount; i++)
                {
                    if (_icons[i].HitTest(tx, ty))
                    {
                        ContextMenu.Close();
                        StartMenu.Close();
                        _icons[i].Update(touch, true);
                        return;
                    }
                }

                ContextMenu.Close();
                StartMenu.Close();
            }
            else
            {
                if (_switcher.IsOpen)   { _switcher.HandleTouch(touch, false);       return; }
                if (_quickSettings.IsOpen) { _quickSettings.HandleTouch(touch, false); return; }
                if (ContextMenu.IsOpen) { ContextMenu.HandleTouch(touch, false);     return; }
                if (StartMenu.IsOpen)   { StartMenu.HandleTouch(touch, false);       return; }
                if (_taskBarOwned)      { TaskBar.HandleTouch(touch, false);         return; }
                if (_touchOwner != null) _touchOwner.HandleTouch(touch, false);
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

            // Desktop icons (below windows)
            for (int i = 0; i < _iconCount; i++)
                _icons[i].Draw();

            // ── Feature 2: Aero Snap preview ──────────────────────────────────
            for (int i = 0; i < _count; i++)
            {
                if (_windows[i].HasSnapPreview)
                {
                    Graphics.FillRectAlpha(
                        _windows[i].SnapPreviewX, _windows[i].SnapPreviewY,
                        _windows[i].SnapPreviewW, _windows[i].SnapPreviewH,
                        Color.RGB(0, 120, 215), 55);
                    Graphics.DrawRect(
                        _windows[i].SnapPreviewX, _windows[i].SnapPreviewY,
                        _windows[i].SnapPreviewW, _windows[i].SnapPreviewH,
                        Color.RGB(0, 140, 240));
                }
            }

            // Windows back to front; all but topmost active get dimmed
            for (int i = 0; i < _count; i++)
            {
                if (_windows[i].IsMinimized && !_windows[i].IsAnimating) continue;
                bool dimmed = (i < _count - 1);
                _windows[i].Draw(dimmed);
            }

            TaskBar.Draw();

            if (StartMenu.IsOpen)
                StartMenu.Draw();

            if (_quickSettings.IsOpen)
                _quickSettings.Draw();

            if (ContextMenu.IsOpen)
                ContextMenu.Draw();

            if (_switcher.IsOpen)
                _switcher.Draw();

            // Toast notifications (always on top)
            _toasts.Draw();
        }
    }
}
