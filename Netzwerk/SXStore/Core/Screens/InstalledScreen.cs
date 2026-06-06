using SXStore.Core.Controller;
using SXStore.Core.GUI.Components;
using SXStore.Core.Models;
using SXStore.Core.Repository;
using System;
using System.Collections.Generic;

namespace SXStore.Core.Screens
{
    public class InstalledScreen : IScreen
    {
        private readonly InputHandler _input;
        private readonly IAppRepository _repository;

        private readonly List<AppEntry> _installed = new List<AppEntry>();
        private int _focusedIndex = 0;
        private int _scrollTop    = 0;

        private const int CX = GlobalDefines.SidebarWidth;
        private const int CW = GlobalDefines.ContentWidth;
        private const int HeaderH   = GlobalDefines.HeaderHeight;
        private const int ListY     = 112;
        private const int RowH      = 80;
        private const int RowGap    = 4;
        private const int ListBottom = 672;
        private const int VisRows   = 7;
        private const int SbW       = 4;
        private const int SbX       = CX + CW - SbW;

        private const uint CBg       = 0xFF11111B;
        private const uint CPanel    = 0xFF181825;
        private const uint CRow      = 0xFF1E1E2E;
        private const uint CRowFocus = 0xFF252540;
        private const uint CAccent   = 0xFF89B4FA;
        private const uint CText     = 0xFFCDD6F4;
        private const uint CMuted    = 0xFF7F849C;
        private const uint CBorder   = 0xFF313244;
        private const uint CSep      = 0xFF24243A;
        private const uint CSuccess  = 0xFFA6E3A1;
        private const uint CSuccessBg = 0xFF1A2E23;

        public InstalledScreen(InputHandler input, IAppRepository repository)
        {
            _input = input;
            _repository = repository;
        }

        public void OnInit()
        {
            _installed.Clear();
            var all = _repository.GetAll();
            for (int i = 0; i < Math.Min(3, all.Count); i++)
                _installed.Add(all[i]);
        }

        public void OnUpdate()
        {
            if (_input.IsDown(NpadButton.B)) { Navigator.Pop(); return; }

            if (_input.IsDown(NpadButton.Down) || _input.IsDown(NpadButton.StickLDown))
            {
                _focusedIndex = Math.Min(_focusedIndex + 1, _installed.Count - 1);
                if (_focusedIndex >= _scrollTop + VisRows) _scrollTop++;
            }
            else if (_input.IsDown(NpadButton.Up) || _input.IsDown(NpadButton.StickLUp))
            {
                _focusedIndex = Math.Max(_focusedIndex - 1, 0);
                if (_focusedIndex < _scrollTop) _scrollTop--;
            }

            if (_input.IsDown(NpadButton.A) && _installed.Count > 0)
                Navigator.Push(new DetailScreen(_input, _repository, _installed[_focusedIndex]));

            if (_input.TouchBegan)
            {
                for (int i = _scrollTop; i < Math.Min(_scrollTop + VisRows, _installed.Count); i++)
                {
                    int ry = ListY + (i - _scrollTop) * (RowH + RowGap);
                    if (_input.Touch.HitRect(0, CX, ry, CW - SbW - 4, RowH))
                    {
                        _focusedIndex = i;
                        Navigator.Push(new DetailScreen(_input, _repository, _installed[i]));
                        return;
                    }
                }
            }
        }

        public void OnDraw()
        {
            Graphics.FillScreen(CBg);
            ContentHeader.DrawBadged("Installed", $"{_installed.Count} installed", CSuccessBg);
            DrawList();
            ContentActions.Draw(new List<string> { "A = Open", "Left/Right = Navigate", "B = Back" });
        }

        public void OnDestroy() { }

        private void DrawList()
        {
            if (_installed.Count == 0)
            {
                int ey = ListY + 80;
                Graphics.FillRoundedRect(CX, ey, CW - SbW - 4, 100, 12, CRow);
                Graphics.DrawText(CX + 40, ey + 24, "No apps installed.", CMuted, 1);
                Graphics.DrawText(CX + 40, ey + 48, "Browse and download apps to see them here.", CMuted, 1);
                return;
            }

            for (int i = _scrollTop; i < Math.Min(_scrollTop + VisRows, _installed.Count); i++)
            {
                int ry = ListY + (i - _scrollTop) * (RowH + RowGap);
                DrawRow(_installed[i], ry, i == _focusedIndex);
            }

            DrawScrollbar();
        }

        private void DrawRow(AppEntry e, int ry, bool focused)
        {
            int rw = CW - SbW - 8;
            Graphics.FillRoundedRect(CX, ry, rw, RowH, 8, focused ? CRowFocus : CRow);

            if (focused)
            {
                Graphics.DrawRoundedRect(CX, ry, rw, RowH, 8, CAccent);
                Graphics.FillRoundedRect(CX, ry, 3, RowH, 2, CAccent);
            }
            else
            {
                Graphics.DrawRoundedRect(CX, ry, rw, RowH, 8, CSep);
            }

            // App icon placeholder
            Graphics.FillRoundedRect(CX + 10, ry + 12, 56, 56, 10, CBorder);
            Graphics.FillRoundedRect(CX + 14, ry + 16, 48, 48, 8, 0xFF24243A);

            // Name + author
            Graphics.DrawText(CX + 80, ry + 16, e.Title, CText, 1);
            Graphics.DrawText(CX + 80, ry + 40, e.Author, CMuted, 1);

            // Version + size
            int vx = CX + rw - 280;
            Graphics.DrawText(vx, ry + 16, e.Version, CAccent, 1);
            Graphics.DrawText(vx, ry + 40, e.SizeFormatted, CMuted, 1);

            // Installed badge
            int badgeW = 88;
            int badgeX = CX + rw - badgeW - 8;
            Graphics.FillRoundedRect(badgeX, ry + RowH / 2 - 11, badgeW, 22, 6, CSuccessBg);
            Graphics.DrawRoundedRect(badgeX, ry + RowH / 2 - 11, badgeW, 22, 6, CSuccess);
            Graphics.DrawText(badgeX + 16, ry + RowH / 2 - 5, "Installed", CSuccess, 1);
        }

        private void DrawScrollbar()
        {
            if (_installed.Count <= VisRows) return;
            int totalH = VisRows * (RowH + RowGap);
            int barH = Math.Max(24, totalH * VisRows / _installed.Count);
            int range = totalH - barH;
            int barY = ListY + (_installed.Count > VisRows
                ? range * _scrollTop / (_installed.Count - VisRows)
                : 0);
            Graphics.FillRoundedRect(SbX, ListY, SbW, totalH, 2, CBorder);
            Graphics.FillRoundedRect(SbX, barY, SbW, barH, 2, CAccent);
        }
    }
}
