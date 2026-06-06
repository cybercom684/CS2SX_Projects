using SXStore.Core.Controller;
using SXStore.Core.GUI.Components;
using SXStore.Core.Models;
using SXStore.Core.Repository;
using System;
using System.Collections.Generic;

namespace SXStore.Core.Screens
{
    public class BrowseScreen : IScreen
    {
        private readonly InputHandler _input;
        private readonly IAppRepository _repository;

        private List<AppEntry> _entries;
        private int _focusedIndex = 0;
        private int _scrollTop    = 0;
        private string _activeCategory = null;

        private static readonly string[] Categories =
            { "All", AppCategory.Games, AppCategory.Tools, AppCategory.Emulator, AppCategory.Media, AppCategory.Homebrew };
        private int _categoryIndex = 0;

        private const int CX = GlobalDefines.SidebarWidth;
        private const int CW = GlobalDefines.ContentWidth;
        private const int HeaderH   = GlobalDefines.HeaderHeight;
        private const int TabY      = 96;
        private const int TabH      = 38;
        private const int ListY     = 148;
        private const int RowH      = 76;
        private const int RowGap    = 4;
        private const int ListBottom = 672;
        private const int VisRows   = 7;   // (ListBottom - ListY) / (RowH + RowGap) ≈ 7
        private const int SbW       = 4;   // scrollbar width
        private const int SbX       = CX + CW - SbW;

        private const uint CBg        = 0xFF11111B;
        private const uint CPanel     = 0xFF181825;
        private const uint CRow       = 0xFF1E1E2E;
        private const uint CRowFocus  = 0xFF252540;
        private const uint CAccent    = 0xFF89B4FA;
        private const uint CText      = 0xFFCDD6F4;
        private const uint CMuted     = 0xFF7F849C;
        private const uint CBorder    = 0xFF313244;
        private const uint CSep       = 0xFF24243A;
        private const uint CTabActive = 0xFF89B4FA;
        private const uint CTabBg     = 0xFF1E1E2E;
        private const uint CSuccess   = 0xFFA6E3A1;

        public BrowseScreen(InputHandler input, IAppRepository repository)
        {
            _input = input;
            _repository = repository;
        }

        public void OnInit() => RefreshEntries();

        public void OnUpdate()
        {
            if (_input.IsDown(NpadButton.B)) { Navigator.Pop(); return; }

            if (_input.IsDown(NpadButton.L))
            {
                _categoryIndex = Math.Max(_categoryIndex - 1, 0);
                ApplyCategory();
            }
            else if (_input.IsDown(NpadButton.R))
            {
                _categoryIndex = Math.Min(_categoryIndex + 1, Categories.Length - 1);
                ApplyCategory();
            }

            if (_input.IsDown(NpadButton.Down) || _input.IsDown(NpadButton.StickLDown))
            {
                _focusedIndex = Math.Min(_focusedIndex + 1, _entries.Count - 1);
                if (_focusedIndex >= _scrollTop + VisRows) _scrollTop++;
            }
            else if (_input.IsDown(NpadButton.Up) || _input.IsDown(NpadButton.StickLUp))
            {
                _focusedIndex = Math.Max(_focusedIndex - 1, 0);
                if (_focusedIndex < _scrollTop) _scrollTop--;
            }

            if (_input.IsDown(NpadButton.A) && _entries.Count > 0)
                Navigator.Push(new DetailScreen(_input, _repository, _entries[_focusedIndex]));

            if (_input.TouchBegan)
            {
                int tabX = CX;
                for (int i = 0; i < Categories.Length; i++)
                {
                    int tabW = GetTabWidth(Categories[i]);
                    if (_input.Touch.HitRect(0, tabX, TabY, tabW, TabH))
                    {
                        _categoryIndex = i;
                        ApplyCategory();
                        return;
                    }
                    tabX += tabW + 8;
                }

                for (int i = _scrollTop; i < Math.Min(_scrollTop + VisRows, _entries.Count); i++)
                {
                    int ry = ListY + (i - _scrollTop) * (RowH + RowGap);
                    if (_input.Touch.HitRect(0, CX, ry, CW - SbW - 4, RowH))
                    {
                        _focusedIndex = i;
                        Navigator.Push(new DetailScreen(_input, _repository, _entries[i]));
                        return;
                    }
                }
            }
        }

        public void OnDraw()
        {
            Graphics.FillScreen(CBg);
            ContentHeader.DrawBadged("Browse", $"{_entries.Count} results",CBorder);

            DrawCategoryTabs();
            DrawList();
            ContentActions.Draw(new List<string> { "A = Open", "Up/Down = Navigate", "L/R = Category", "B = Back" });

        }

        public void OnDestroy() { }

        private void DrawCategoryTabs()
        {
            int tabX = CX;
            for (int i = 0; i < Categories.Length; i++)
            {
                bool active = i == _categoryIndex;
                int tabW = GetTabWidth(Categories[i]);
                uint bgColor = active ? CAccent : CTabBg;
                uint txtColor = active ? 0xFF11111B : CMuted;

                Graphics.FillRoundedRect(tabX, TabY, tabW, TabH, 8, bgColor);
                if (!active)
                    Graphics.DrawRoundedRect(tabX, TabY, tabW, TabH, 8, CBorder);
                Graphics.DrawText(tabX + 10, TabY + 12, Categories[i], txtColor, 1);
                tabX += tabW + 8;
            }
            // Hint
            Graphics.DrawText(CX + CW - 120, TabY + 12, "L / R to switch", CMuted, 1);
        }

        private void DrawList()
        {
            if (_entries.Count == 0)
            {
                Graphics.FillRoundedRect(CX, ListY + 40, CW - SbW - 4, 80, 12, CRow);
                Graphics.DrawText(CX + CW / 2 - 80, ListY + 72, "No entries found.", CMuted, 1);
                return;
            }

            for (int i = _scrollTop; i < Math.Min(_scrollTop + VisRows, _entries.Count); i++)
            {
                int ry = ListY + (i - _scrollTop) * (RowH + RowGap);
                DrawRow(_entries[i], i, ry, i == _focusedIndex);
            }

            DrawScrollbar();
        }

        private void DrawRow(AppEntry e, int index, int ry, bool focused)
        {
            int rw = CW - SbW - 8;
            uint bg = focused ? CRowFocus : CRow;

            // Row background
            Graphics.FillRoundedRect(CX, ry, rw, RowH, 8, bg);

            if (focused)
            {
                Graphics.DrawRoundedRect(CX, ry, rw, RowH, 8, CAccent);
                // Left accent bar
                Graphics.FillRoundedRect(CX, ry, 3, RowH, 2, CAccent);
            }
            else
            {
                Graphics.DrawRoundedRect(CX, ry, rw, RowH, 8, CSep);
            }

            // Thumbnail
            Graphics.FillRoundedRect(CX + 10, ry + 10, 56, 56, 8, CBorder);
            Graphics.FillRoundedRect(CX + 14, ry + 14, 48, 48, 6, 0xFF24243A);

            // Title + author
            Graphics.DrawText(CX + 80, ry + 14, e.Title, CText, 1);
            Graphics.DrawText(CX + 80, ry + 36, e.Author, CMuted, 1);

            // Version
            int vx = CX + rw - 260;
            Graphics.DrawText(vx, ry + 14, e.Version, CAccent, 1);
            Graphics.DrawText(vx, ry + 36, e.SizeFormatted, CMuted, 1);

            // Category pill
            string cat = e.Category;
            int catW = cat.Length * 9 + 16;
            int catX = CX + rw - catW - 8;
            Graphics.FillRoundedRect(catX, ry + RowH / 2 - 11, catW, 22, 4, 0xFF1E2A4A);
            Graphics.DrawText(catX + 8, ry + RowH / 2 - 5, cat, CAccent, 1);
        }

        private void DrawScrollbar()
        {
            if (_entries.Count <= VisRows) return;
            int totalH = VisRows * (RowH + RowGap);
            int barH = Math.Max(24, totalH * VisRows / _entries.Count);
            int range = totalH - barH;
            int barY = ListY + (_entries.Count > VisRows
                ? range * _scrollTop / (_entries.Count - VisRows)
                : 0);

            // Track
            Graphics.FillRoundedRect(SbX, ListY, SbW, totalH, 2, CBorder);
            // Thumb
            Graphics.FillRoundedRect(SbX, barY, SbW, barH, 2, CAccent);
        }

        private void RefreshEntries()
        {
            _entries = _activeCategory == null
                ? new List<AppEntry>(_repository.GetAll())
                : new List<AppEntry>(_repository.GetByCategory(_activeCategory));
            _focusedIndex = 0;
            _scrollTop    = 0;
        }

        private void ApplyCategory()
        {
            _activeCategory = Categories[_categoryIndex] == "All" ? null : Categories[_categoryIndex];
            RefreshEntries();
        }

        private int GetTabWidth(string label) => label.Length * 9 + 20;
    }
}
