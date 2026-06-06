using SXStore.Core.Controller;
using SXStore.Core.GUI.Components;
using SXStore.Core.Models;
using SXStore.Core.Repository;
using System;
using System.Collections.Generic;

namespace SXStore.Core.Screens
{
    public class DetailScreen : IScreen
    {
        private readonly InputHandler _input;
        private readonly IAppRepository _repository;
        private readonly AppEntry _entry;

        private const int CX = GlobalDefines.SidebarWidth;
        private const int CW = GlobalDefines.ContentWidth;

        private const uint CBg      = 0xFF11111B;
        private const uint CPanel   = 0xFF181825;
        private const uint CCard    = 0xFF1E1E2E;
        private const uint CAccent  = 0xFF89B4FA;
        private const uint CAccentB = 0xFFB4BEFE;
        private const uint CText    = 0xFFCDD6F4;
        private const uint CMuted   = 0xFF7F849C;
        private const uint CBorder  = 0xFF313244;
        private const uint CSuccess = 0xFFA6E3A1;
        private const uint CSuccBg  = 0xFF1A2E23;

        public DetailScreen(InputHandler input, IAppRepository repository, AppEntry entry)
        {
            _input = input;
            _repository = repository;
            _entry = entry;
        }

        public void OnInit() { }

        public void OnUpdate()
        {
            if (_input.IsDown(NpadButton.B))
                Navigator.Pop();

            if (_input.TouchBegan)
            {
                if (_input.Touch.HitRect(0, CX, 600, 220, 52))
                    OnDownload();
                if (_input.Touch.HitRect(0, CX + 240, 600, 160, 52))
                    Navigator.Pop();
            }

            if (_input.IsDown(NpadButton.A))
                OnDownload();
        }

        public void OnDraw()
        {
            Graphics.FillScreen(CBg);
            DrawHeroCard();
            DrawMetaRow();
            DrawDivider();
            DrawDescription();
            DrawActionButtons();
            ContentActions.Draw(new List<string> { "A = Download", "B = Back" });

        }

        public void OnDestroy() { }


        private void DrawHeroCard()
        {
            Card.Draw(CX, 15, CW - GlobalDefines.Padding, 148, CCard, CBorder, (cx, cy, cWidth, cHeight) =>
            {
                int spacedAfterImage = cx + cHeight + GlobalDefines.Padding;
                int titleHeight = Graphics.MeasureTextHeight(2);
                int badgeHeight = 30;
                int versionWidth = Graphics.MeasureTextWidth(_entry.Version, 1);
                int sizeWidth = Graphics.MeasureTextWidth(_entry.SizeFormatted, 1);
                int centeredTitleHeight = (cHeight / 2) - titleHeight;

                // Image
                Image.Draw(cx, cy, cHeight, cHeight, "test");

                // Title
                TextLabel.Draw(spacedAfterImage, centeredTitleHeight, _entry.Title, 2, GlobalDefines.Text);

                // Version + Size badges
                int badgeY = centeredTitleHeight + titleHeight + GlobalDefines.Padding;
                TextLabel.DrawBadged(spacedAfterImage, badgeY, versionWidth + 20, badgeHeight, _entry.Version, 1, GlobalDefines.Accent, GlobalDefines.Background);
                TextLabel.DrawBadged(spacedAfterImage + versionWidth + 20 + GlobalDefines.Padding, badgeY, sizeWidth + 20, badgeHeight, _entry.SizeFormatted, 1, GlobalDefines.Accent, GlobalDefines.Background);
            });
        }

        private void DrawMetaRow()
        {
            int my = 238;
            string cat = _entry.Category;
            int catW = cat.Length * 9 + 24;
            Graphics.FillRoundedRect(CX, my, catW, 30, 8, 0xFF1E2A4A);
            Graphics.DrawRoundedRect(CX, my, catW, 30, 8, CAccent);
            Graphics.DrawText(CX + 12, my + 8, cat, CAccent, 1);

            Graphics.FillRoundedRect(CX + catW + 10, my, 80, 30, 8, 0xFF1E2A1E);
            Graphics.DrawText(CX + catW + 22, my + 8, "Switch", CSuccess, 1);
        }

        private void DrawDivider()
        {
            int dy = 284;
            Graphics.FillRect(CX, dy, CW, 1, CBorder);
            Graphics.DrawText(CX, dy + 10, "DESCRIPTION", CMuted, 1);
            Graphics.FillRect(CX, dy + 28, 48, 2, CAccent);
        }

        private void DrawDescription()
        {
            DrawWrappedText(_entry.Description, CX, 322, CW - 24, CText, 1);
        }

        private void DrawActionButtons()
        {
            int by = 596;
            // Download (primary)
            Graphics.FillRoundedRect(CX, by, 220, 52, 10, CSuccBg);
            Graphics.DrawRoundedRect(CX, by, 220, 52, 10, CSuccess);
            Graphics.DrawText(CX + 52, by + 16, "Download", CSuccess, 1);
            // Cancel (secondary)
            Graphics.FillRoundedRect(CX + 236, by, 160, 52, 10, CCard);
            Graphics.DrawRoundedRect(CX + 236, by, 160, 52, 10, CBorder);
            Graphics.DrawText(CX + 270, by + 16, "Cancel", CMuted, 1);
        }

        private void OnDownload() { }

        private void DrawWrappedText(string text, int x, int y, int maxW, uint color, int scale)
        {
            int charW = Graphics.MeasureTextWidth("A", scale);
            int lineH = Graphics.MeasureTextHeight(scale) + 8;
            int charsPerLine = maxW / charW;
            int offset = 0;
            int maxLines = 8;
            int lines = 0;

            while (offset < text.Length && lines < maxLines)
            {
                int len = Math.Min(charsPerLine, text.Length - offset);
                if (offset + len < text.Length && len == charsPerLine)
                {
                    int lastSpace = -1;
                    for (int j = len - 1; j >= len / 2; j--)
                        if (text[offset + j] == ' ') { lastSpace = j; break; }
                    if (lastSpace > 0) len = lastSpace + 1;
                }
                Graphics.DrawText(x, y + lines * lineH, text.Substring(offset, len), color, scale);
                offset += len;
                lines++;
            }
        }
    }
}
