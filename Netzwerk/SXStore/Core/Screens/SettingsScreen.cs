using SXStore.Core.Controller;
using SXStore.Core.GUI.Components;
using System;
using System.Collections.Generic;

namespace SXStore.Core.Screens
{
    public class SettingsScreen : IScreen
    {
        private readonly InputHandler _input;

        private int _focusedIndex = 0;
        private int _scrollTop    = 0;

        private readonly Dictionary<string,string> _settings = new Dictionary<string, string>
        {
            { "Download Path", "sdmc:/switch/" },
            { "Theme", "Dark" },
            { "Language", "Deutsch" },
            { "Auto-Update", "Enabled" },
            {"Store URL", "https://sxstore.example.com" },
            {"Cache Size", "512 MB" },
            { "Network Timeout", "10 s" },
        };

        private const int CX = GlobalDefines.SidebarWidth;
        private const int CW = GlobalDefines.ContentWidth;
        private const int ListY   = 108;
        private const int RowH    = 72;
        private const int RowGap  = 6;
        private const int VisRows = 8;
        private const int SbW     = 4;
        private const int SbX     = CX + CW - SbW;

        private const uint CBg       = 0xFF11111B;
        private const uint CRow      = 0xFF1E1E2E;
        private const uint CRowFocus = 0xFF252540;
        private const uint CAccent   = 0xFF89B4FA;
        private const uint CText     = 0xFFCDD6F4;
        private const uint CMuted    = 0xFF7F849C;
        private const uint CBorder   = 0xFF313244;
        private const uint CSep      = 0xFF24243A;
        private const uint CValue    = 0xFFB4BEFE;

        public SettingsScreen(InputHandler input)
        {
            _input = input;
        }

        public void OnInit() { }

        public void OnUpdate()
        {
            if (_input.IsDown(NpadButton.B)) { Navigator.Pop(); return; }

            if (_input.IsDown(NpadButton.Down) || _input.IsDown(NpadButton.StickLDown))
            {
                _focusedIndex = Math.Min(_focusedIndex + 1, _settings.Count - 1);
                if (_focusedIndex >= _scrollTop + VisRows) _scrollTop++;
            }
            else if (_input.IsDown(NpadButton.Up) || _input.IsDown(NpadButton.StickLUp))
            {
                _focusedIndex = Math.Max(_focusedIndex - 1, 0);
                if (_focusedIndex < _scrollTop) _scrollTop--;
            }

            if (_input.TouchBegan)
            {
                for (int i = _scrollTop; i < Math.Min(_scrollTop + VisRows, _settings.Count); i++)
                {
                    int ry = ListY + (i - _scrollTop) * (RowH + RowGap);
                    if (_input.Touch.HitRect(0, CX, ry, CW - SbW - 4, RowH))
                    {
                        _focusedIndex = i;
                        return;
                    }
                }
            }
        }

        public void OnDraw()
        {
            Graphics.FillScreen(CBg);
            ContentHeader.Draw("Settings", "ZL  Back to Menu");
            DrawList();
            ContentActions.Draw(new List<string> { "Up/Down = Navigate", "B = Back" });

        }

        public void OnDestroy() { }

        private void DrawList()
        {
            var count = 0;

            foreach (var item in _settings)
            {
                if (count < _scrollTop)
                {
                    count++;
                    continue;
                }

                if (count >= _scrollTop + VisRows)
                    break;

                int ry = ListY + (count - _scrollTop) * (RowH + RowGap);

                DrawRow(item.Key, item.Value, ry,count == _focusedIndex);

                count++;
            }

            DrawScrollbar();
        }

        private void DrawRow(string label, string value, int ry, bool focused)
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

            // Label (left)
            Graphics.DrawText(CX + 20, ry + 18, label, CText, 1);

            // Separator dot
            Graphics.DrawText(CX + 20 + label.Length * 9 + 12, ry + 18, "·", CMuted, 1);

            // Value (right-ish, with pill)
            int valueW = value.Length * 9 + 24;
            int valueX = CX + rw - valueW - 12;
            Graphics.FillRoundedRect(valueX, ry + RowH / 2 - 14, valueW, 28, 6, CBorder);
            Graphics.DrawText(valueX + 12, ry + RowH / 2 - 6, value, CValue, 1);

            // Chevron
            Graphics.DrawText(CX + rw - 20, ry + RowH / 2 - 6, focused ? ">" : "›", focused ? CAccent : CMuted, 1);
        }

        private void DrawScrollbar()
        {
            if (_settings.Count <= VisRows) return;
            int totalH = VisRows * (RowH + RowGap);
            int barH = Math.Max(24, totalH * VisRows / _settings.Count);
            int range = totalH - barH;
            int barY = ListY + (_settings.Count > VisRows
                ? range * _scrollTop / (_settings.Count - VisRows)
                : 0);
            Graphics.FillRoundedRect(SbX, ListY, SbW, totalH, 2, CBorder);
            Graphics.FillRoundedRect(SbX, barY, SbW, barH, 2, CAccent);
        }
    }
}
