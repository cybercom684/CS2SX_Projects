namespace multiWindow.UI
{
    internal class Toast
    {
        public string Title   = "";
        public string Message = "";
        public int    Life;
        public int    Total;

        private const int SlideDuration = 10;
        private const int ToastW        = 280;

        public int SlideOffset()
        {
            int elapsed = Total - Life;
            if (elapsed < SlideDuration)
            {
                float t = 1.0f - (float)elapsed / SlideDuration;
                return (int)(t * t * (ToastW + 20));
            }
            if (Life < SlideDuration)
            {
                float t = 1.0f - (float)Life / SlideDuration;
                return (int)(t * t * (ToastW + 20));
            }
            return 0;
        }

        public bool IsDead => Life <= 0;
    }

    public class ToastManager
    {
        private const int MaxToasts = 3;
        private const int ToastW    = 280;
        private const int ToastH    = 58;
        private const int ToastGap  = 6;
        private const int ToastX    = 1280 - ToastW - 8;
        private const int BaseY     = 680 - ToastH - 48;

        private Toast[] _toasts = new Toast[MaxToasts];
        private int     _count;

        public void Show(string title, string message, int durationFrames = 240)
        {
            if (_count >= MaxToasts)
            {
                for (int i = 0; i < _count - 1; i++)
                    _toasts[i] = _toasts[i + 1];
                _count = MaxToasts - 1;
            }
            Toast t = new Toast();
            t.Title   = title;
            t.Message = message;
            t.Life    = durationFrames;
            t.Total   = durationFrames;
            _toasts[_count++] = t;
        }

        public void Update()
        {
            for (int i = 0; i < _count; i++)
                _toasts[i].Life--;

            int write = 0;
            for (int i = 0; i < _count; i++)
                if (!_toasts[i].IsDead)
                    _toasts[write++] = _toasts[i];
            _count = write;
        }

        public void Draw()
        {
            for (int i = 0; i < _count; i++)
            {
                int offsetX = _toasts[i].SlideOffset();
                int x = ToastX + offsetX;
                int y = BaseY - i * (ToastH + ToastGap);

                // Shadow
                Graphics.FillRectAlpha(x + 3, y + 3, ToastW, ToastH, Color.Black, 80);

                // Background
                Graphics.FillRoundedRect(x, y, ToastW, ToastH, 6, Color.RGB(38, 38, 44));

                // Left accent bar
                Graphics.FillRoundedRect(x, y, 4, ToastH, 2, Color.RGB(0, 120, 215));

                // Top stripe
                Graphics.FillRect(x + 4, y, ToastW - 4, 2, Color.RGB(0, 80, 160));

                // Border
                Graphics.DrawRoundedRect(x, y, ToastW, ToastH, 6, Color.RGB(60, 60, 70));

                // Title
                int titleH = Graphics.MeasureTextHeight(2);
                Graphics.DrawText(x + 12, y + 7, _toasts[i].Title, Color.White, 2);

                // Message
                Graphics.DrawText(x + 12, y + 9 + titleH, _toasts[i].Message, Color.RGB(175, 175, 182), 1);
            }
        }
    }
}
