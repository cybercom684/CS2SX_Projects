using System;

namespace multiWindow.UI
{
    public class ContextMenu
    {
        private const int ItemH = 32;
        private const int MenuW = 200;
        private const int PadX = 10;
        private const int Radius = 5;

        public bool IsOpen { get; private set; }

        private int _x, _y;

        private string[] _labels = new string[8];
        private Action[] _callbacks = new Action[8];
        private int _count;

        public void AddItem(string label, Action callback)
        {
            if (_count >= 8) return;
            _labels[_count] = label;
            _callbacks[_count] = callback;
            _count++;
        }

        public void Open(int x, int y)
        {
            int menuH = _count * ItemH + 8;

            // Keep within screen bounds
            _x = x + MenuW > 1280 ? 1280 - MenuW - 2 : x;
            _y = y + menuH > 680 ? y - menuH : y;
            if (_y < 0) _y = 0;

            IsOpen = true;
        }

        public void Close() => IsOpen = false;

        public bool HandleTouch(TouchState touch, bool touchBegan)
        {
            if (!IsOpen || !touch.IsTouched) return false;

            int tx = touch.X0;
            int ty = touch.Y0;
            int menuH = _count * ItemH + 8;

            if (!touchBegan) return IsOpen;

            // Tap outside → close
            if (tx < _x || tx > _x + MenuW || ty < _y || ty > _y + menuH)
            {
                Close();
                return false;
            }

            int itemAreaY = _y + 4;
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

            return true;
        }

        public void Draw()
        {
            if (!IsOpen) return;

            int menuH = _count * ItemH + 8;

            // Shadow
            Graphics.FillRect(_x + 3, _y + 3, MenuW, menuH, Color.RGB(5, 5, 5));

            // Background
            Graphics.FillRoundedRect(_x, _y, MenuW, menuH, Radius, Color.RGB(32, 32, 36));

            // Border
            Graphics.DrawRoundedRect(_x, _y, MenuW, menuH, Radius, Color.RGB(70, 70, 78));

            // Top accent line
            Graphics.FillRect(_x + 1, _y + 1, MenuW - 2, 2, Color.RGB(0, 120, 215));

            int itemAreaY = _y + 4;
            int textH = Graphics.MeasureTextHeight(2);

            for (int i = 0; i < _count; i++)
            {
                int iy = itemAreaY + i * ItemH;

                // Hover/separator line between items
                if (i > 0)
                    Graphics.DrawLine(_x + PadX, iy, _x + MenuW - PadX, iy, Color.RGB(50, 50, 56));

                Graphics.DrawText(_x + PadX + 4, iy + (ItemH - textH) / 2,
                                  _labels[i], Color.White, 2);
            }
        }
    }
}