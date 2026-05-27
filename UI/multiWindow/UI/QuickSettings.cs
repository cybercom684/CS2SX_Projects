namespace multiWindow.UI
{
    public class QuickSettings
    {
        private const int PanelW = 262;
        private const int PanelH = 186;
        private const int PanelX = 1280 - PanelW - 2;
        private const int PanelY = 680 - PanelH - 42;

        public bool IsOpen { get; private set; }

        public void Toggle() => IsOpen = !IsOpen;
        public void Close()  => IsOpen = false;

        public bool HandleTouch(TouchState touch, bool touchBegan)
        {
            if (!IsOpen || !touch.IsTouched || !touchBegan) return IsOpen;

            int tx = touch.X0;
            int ty = touch.Y0;

            if (tx < PanelX || tx > PanelX + PanelW || ty < PanelY || ty > PanelY + PanelH)
            {
                Close();
                return false;
            }
            return true;
        }

        public void Draw()
        {
            if (!IsOpen) return;

            TimeInfo    t   = NX.GetTime();
            BatteryInfo bat = NX.GetBattery();

            // Shadow
            Graphics.FillRectAlpha(PanelX + 5, PanelY + 5, PanelW, PanelH, Color.Black, 90);

            // Panel background
            Graphics.FillRoundedRect(PanelX, PanelY, PanelW, PanelH, 8, Color.RGB(26, 26, 32));
            Graphics.DrawRoundedRect(PanelX, PanelY, PanelW, PanelH, 8, Color.RGB(58, 58, 72));

            // Top accent
            Graphics.FillRect(PanelX + 1, PanelY + 1, PanelW - 2, 2, Color.RGB(0, 120, 215));

            // ── Large time display ─────────────────────────────────────────────
            string hh = t.hour   < 10 ? "0" + t.hour   : "" + t.hour;
            string mm = t.minute < 10 ? "0" + t.minute : "" + t.minute;
            string timeStr = hh + ":" + mm;
            int timeW = Graphics.MeasureTextWidth(timeStr, 4);
            int timeH = Graphics.MeasureTextHeight(4);
            Graphics.DrawText(PanelX + (PanelW - timeW) / 2, PanelY + 14, timeStr, Color.White, 4);

            // ── Battery label ──────────────────────────────────────────────────
            int barAreaY = PanelY + 14 + timeH + 8;
            string batLabel = "Akku: " + bat.percent + "%" + (bat.charging ? " (laden)" : "");
            int lblW = Graphics.MeasureTextWidth(batLabel, 1);
            int lblH = Graphics.MeasureTextHeight(1);
            Graphics.DrawText(PanelX + (PanelW - lblW) / 2, barAreaY, batLabel, Color.RGB(175, 175, 185), 1);

            // ── Battery bar ────────────────────────────────────────────────────
            int barX = PanelX + 16;
            int barY = barAreaY + lblH + 4;
            int barW = PanelW - 32;
            int barH = 14;

            Graphics.FillRoundedRect(barX, barY, barW, barH, 4, Color.RGB(48, 48, 54));
            int fillW = barW * bat.percent / 100;
            if (fillW < 0)   fillW = 0;
            if (fillW > barW) fillW = barW;
            uint batColor = bat.percent > 20 ? Color.RGB(0, 175, 80) : Color.RGB(200, 50, 30);
            if (fillW > 0)
                Graphics.FillRoundedRect(barX, barY, fillW, barH, 4, batColor);
            Graphics.DrawRoundedRect(barX, barY, barW, barH, 4, Color.RGB(78, 78, 88));

            // ── Separator ──────────────────────────────────────────────────────
            int sepY = barY + barH + 12;
            Graphics.DrawLine(PanelX + 12, sepY, PanelX + PanelW - 12, sepY, Color.RGB(52, 52, 64));

            // ── Quick-toggle buttons ────────────────────────────────────────────
            int toggleY  = sepY + 10;
            int tW       = (PanelW - 32 - 8) / 3;
            string[] toggleLabels = new string[] { "WLAN", "BT", "Flugm." };
            for (int i = 0; i < 3; i++)
            {
                int tileX = PanelX + 16 + i * (tW + 4);
                Graphics.FillRoundedRect(tileX, toggleY, tW, 28, 5, Color.RGB(44, 44, 52));
                Graphics.DrawRoundedRect(tileX, toggleY, tW, 28, 5, Color.RGB(68, 68, 80));
                int lw2 = Graphics.MeasureTextWidth(toggleLabels[i], 1);
                int lh2 = Graphics.MeasureTextHeight(1);
                Graphics.DrawText(tileX + (tW - lw2) / 2, toggleY + (28 - lh2) / 2,
                                  toggleLabels[i], Color.RGB(155, 155, 168), 1);
            }
        }
    }
}
