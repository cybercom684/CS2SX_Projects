using SXStore.Core.Controller;
using SXStore.Core.GUI.Components;
using SXStore.Core.Models;
using SXStore.Core.Repository;
using System;
using System.Collections.Generic;

namespace SXStore.Core.Screens
{
    public class HomeScreen : IScreen
    {
        private readonly InputHandler _input;
        private readonly IAppRepository _repository;

        private List<AppEntry> _featured;
        private int _focusedIndex = 0;
        private int _scrollOffset = 0;

        private const int CX       = GlobalDefines.SidebarWidth;
        private const int CW       = GlobalDefines.ContentWidth;   // ContentWidth
        private const int CardW    = 300;
        private const int CardH    = 200;
        private const int CardGap  = 24;
        private const int CardsVis = 3;     // cards shown at once
        private const int CardY    = 168;
        private const int HeaderH  = GlobalDefines.HeaderHeight;

        private const uint CBg        = 0xFF11111B;
        private const uint CCard      = 0xFF1E1E2E;
        private const uint CCardFocus = 0xFF252540;
        private const uint CAccent    = 0xFF89B4FA;
        private const uint CAccentAlt = 0xFFB4BEFE;
        private const uint CText      = 0xFFCDD6F4;
        private const uint CMuted     = 0xFF7F849C;
        private const uint CBorder    = 0xFF313244;
        private const uint CSuccess   = 0xFFA6E3A1;
        private const uint CPanel     = 0xFF181825;
        private const uint CSep       = 0xFF45475A;

        public HomeScreen(InputHandler input, IAppRepository repository)
        {
            _input = input;
            _repository = repository;
        }

        public void OnInit()
        {
            _featured = new List<AppEntry>(_repository.GetFeatured());
        }

        public void OnUpdate()
        {
            
            if (_input.IsDown(NpadButton.B)) { Navigator.Pop(); return; }

            if (_input.IsDown(NpadButton.Right) || _input.IsDown(NpadButton.StickLRight))
            {
                _focusedIndex = Math.Min(_focusedIndex + 1, _featured.Count - 1);
                if (_focusedIndex >= _scrollOffset + CardsVis) _scrollOffset++;
            }
            else if (_input.IsDown(NpadButton.Left) || _input.IsDown(NpadButton.StickLLeft))
            {
                _focusedIndex = Math.Max(_focusedIndex - 1, 0);
                if (_focusedIndex < _scrollOffset) _scrollOffset--;
            }

            if (_input.IsDown(NpadButton.A) && _featured.Count > 0)
                OpenDetail(_featured[_focusedIndex]);

            if (_input.TouchBegan)
            {
                for (int i = _scrollOffset; i < Math.Min(_scrollOffset + CardsVis, _featured.Count); i++)
                {
                    int cx = GetCardScreenX(i - _scrollOffset);
                    if (_input.Touch.HitRect(0, cx, CardY, CardW, CardH))
                    {
                        _focusedIndex = i;
                        OpenDetail(_featured[i]);
                        break;
                    }
                }
            }
        }

        public void OnDraw()
        {
            Graphics.FillScreen(CBg);

            ContentHeader.DrawBadged("Featured", $"{_featured.Count} apps", CBorder);
            TextLabel.DrawLined(CX, HeaderH + 14, "NEW & NOTEWORTHY",1,GlobalDefines.ContentWidth - GlobalDefines.Padding, CAccent, CAccent);
            DrawCards();
            DrawScrollIndicator();
            ContentActions.Draw(new List<string> { "A = Open", "Left/Right = Navigate", "B = Back" });
        }

        public void OnDestroy() { }


        private void DrawCards()
        {
            if (_featured.Count == 0)
            {
                Graphics.DrawText(CX + CW / 2 - 80, 360, "No featured apps.", CMuted, 1);
                return;
            }

            // Scroll arrows
            if (_scrollOffset > 0)
            {
                Graphics.FillRoundedRect(CX - 6, CardY + CardH / 2 - 18, 28, 36, 6, CBorder);
                Graphics.DrawText(CX, CardY + CardH / 2 - 10, "<", CAccent, 1);
            }
            if (_scrollOffset + CardsVis < _featured.Count)
            {
                int ax = CX + CardsVis * (CardW + CardGap) + 2;
                Graphics.FillRoundedRect(ax, CardY + CardH / 2 - 18, 28, 36, 6, CBorder);
                Graphics.DrawText(ax + 6, CardY + CardH / 2 - 10, ">", CAccent, 1);
            }

            for (int i = _scrollOffset; i < Math.Min(_scrollOffset + CardsVis, _featured.Count); i++)
                DrawCard(_featured[i], i - _scrollOffset, i == _focusedIndex);
        }

        private void DrawCard(AppEntry entry, int slot, bool focused)
        {
            int cx = GetCardScreenX(slot);
            uint bg     = focused ? CCardFocus : CCard;
            uint border = focused ? CAccent    : CBorder;
            int radius  = 6;

            // Card shadow (offset rect)
            Graphics.FillRoundedRect(cx + 4, CardY + 4, CardW, CardH, radius, 0xFF0D0D17);
            // Card body
            Graphics.FillRoundedRect(cx, CardY, CardW, CardH, radius, bg);

            if (focused)
            {
                // Outer glow border
                Graphics.DrawRoundedRect(cx - 2, CardY - 2, CardW + 4, CardH + 4, radius + 2, CAccent);
                Graphics.DrawRoundedRect(cx - 1, CardY - 1, CardW + 2, CardH + 2, radius + 1, CAccentAlt);
            }
            else
            {
                Graphics.DrawRoundedRect(cx, CardY, CardW, CardH, radius, border);
            }

            // Thumbnail placeholder with gradient-like layering
            Graphics.FillRoundedRect(cx + 12, CardY + 14, 68, 68, 8, CBorder);
            Graphics.FillRoundedRect(cx + 16, CardY + 18, 60, 60, 6, 0xFF24243A);
            Graphics.DrawText(cx + 28, CardY + 40, "NRO", CMuted, 1);

            // Divider under thumbnail area
            Graphics.FillRect(cx + 12, CardY + 92, CardW - 24, 1, CBorder);

            // Title
            Graphics.DrawText(cx + 12, CardY + 102, entry.Title, CText, 1);
            // Author
            Graphics.DrawText(cx + 12, CardY + 124, entry.Author, CMuted, 1);

            // Bottom row: version + category badge
            string ver = entry.Version;
            Graphics.DrawText(cx + 12, CardY + 154, ver, CAccent, 1);

            int badgeX = cx + CardW - 90;
            string cat = entry.Category;
            int catW = cat.Length * 9 + 16;
            Graphics.FillRoundedRect(cx + CardW - catW - 8, CardY + 150, catW, 24, 4, 0xFF1E3A5F);
            Graphics.DrawText(cx + CardW - catW - 2, CardY + 156, cat, CAccent, 1);
        }

        private void DrawScrollIndicator()
        {
            if (_featured.Count <= CardsVis) return;
            int dotY = CardY + CardH + 16;
            int totalDots = _featured.Count;
            int startX = CX + CW / 2 - (totalDots * 14) / 2;
            for (int i = 0; i < totalDots; i++)
            {
                uint col = i == _focusedIndex ? CAccent : CBorder;
                int dotW = i == _focusedIndex ? 20 : 8;
                Graphics.FillRoundedRect(startX + i * 14, dotY, dotW, 6, 3, col);
            }
        }

        private int GetCardScreenX(int slot) => CX + slot * (CardW + CardGap);

        private void OpenDetail(AppEntry entry) =>
            Navigator.Push(new DetailScreen(_input, _repository, entry));
    }
}
