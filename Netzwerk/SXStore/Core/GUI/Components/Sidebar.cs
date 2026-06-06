using System;
using System.Collections.Generic;

namespace SXStore.Core.GUI.Components
{
    public class SidebarItem
    {
        public string Title { get; set; }
        public Texture Icon { get; set; }
        public Action OnClick { get; set; }

        public SidebarItem(string title, Action onClick, Texture icon = null)
        {
            Title = title;
            Icon = icon;
            OnClick = onClick;
        }
    }

    public class Sidebar
    {
        public List<SidebarItem> Items { get; private set; } = new List<SidebarItem>();
        public bool IsVisible { get; set; } = true;
        public int Width  { get; set; } = 240;
        public int Height { get; set; }
        public string Title { get; set; }
        public Texture Icon { get; set; }
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        private int _selectedIndex = -1;
        private int _hoveredIndex  = -1;

        // Layout — matches SidebarScreen.GetItemY
        private const int ItemH      = 52;
        private const int LogoBlockH = 66;
        private const int SectionH   = 28;
        private const int PaddingX   = 20;
        private const int IconSize   = 22;

        // Colors — use GlobalDefines where available, hardcode the rest
        private const uint CBackground   = 0xFF181825;
        private const uint CItemSelected = 0xFF2A2A3E;
        private const uint CItemHover    = 0xFF222236;
        private const uint CTextDim      = 0xFF45475A;

        public Sidebar() { }

        public void AddItem(SidebarItem item)    => Items.Add(item);
        public void RemoveItem(SidebarItem item) => Items.Remove(item);

        public void Clear()
        {
            Items.Clear();
            _selectedIndex = -1;
            _hoveredIndex  = -1;
        }

        public void Draw()
        {
            if (!IsVisible) return;

            Graphics.FillRect(X, Y, Width, Height, CBackground);
            // Right separator
            Graphics.FillRect(X + Width - 2, Y, 1, Height, GlobalDefines.Border);
            Graphics.FillRect(X + Width - 1, Y, 1, Height, GlobalDefines.Separator);

            int offsetY = Y;

            // Logo / header block
            if (!string.IsNullOrEmpty(Title))
            {
                Graphics.FillRect(X, offsetY, Width - 2, LogoBlockH, 0xFF1E1E2E);
                // Accent line bottom of logo
                Graphics.FillRect(X, offsetY + LogoBlockH - 2, Width - 2, 2, GlobalDefines.Accent);

                int titleX = X + PaddingX;
                if (Icon != null)
                {
                    Graphics.DrawTextureScaled(Icon, titleX, offsetY + 21, 24, 24);
                    titleX += 34;
                }
                Graphics.DrawText(titleX, offsetY + 24, Title, GlobalDefines.Text, 1);
                offsetY += LogoBlockH;
            }

            // Section label
            Graphics.DrawText(X + PaddingX, offsetY + 8, "MENU", GlobalDefines.TextMuted, 1);
            offsetY += SectionH;

            // Items
            for (int i = 0; i < Items.Count; i++)
            {
                DrawItem(Items[i], i, offsetY);
                offsetY += ItemH;
            }

            // Version hint at bottom
            Graphics.DrawText(X + PaddingX, Y + Height - 20, "v" + GlobalDefines.AppVersion, CTextDim, 1);
        }

        private void DrawItem(SidebarItem item, int index, int itemY)
        {
            bool isSelected = index == _selectedIndex;
            bool isHovered  = index == _hoveredIndex;

            if (isSelected)
            {
                Graphics.FillRect(X, itemY, Width - 2, ItemH, CItemSelected);
                Graphics.FillRect(X, itemY, 3, ItemH, GlobalDefines.Accent);
                Graphics.FillRect(X + Width - 6, itemY, 4, ItemH, GlobalDefines.AccentDim);
            }
            else if (isHovered)
            {
                Graphics.FillRect(X, itemY, Width - 2, ItemH, CItemHover);
                Graphics.FillRect(X, itemY, 2, ItemH, GlobalDefines.AccentDim);
            }

            // Bottom separator
            Graphics.FillRect(X + PaddingX, itemY + ItemH - 1, Width - PaddingX * 2, 1, GlobalDefines.Separator);

            int contentX = X + PaddingX + (isSelected ? 8 : 4);
            int centerY  = itemY + ItemH / 2;
            int textH    = Graphics.MeasureTextHeight(1);
            int textY    = centerY - textH / 2;

            if (item.Icon != null)
            {
                int iconY = itemY + (ItemH - IconSize) / 2;
                Graphics.DrawTextureScaled(item.Icon, contentX, iconY, IconSize, IconSize);
                contentX += IconSize + 10;
            }

            uint textColor = isSelected ? GlobalDefines.Text : GlobalDefines.TextMuted;
            Graphics.DrawText(contentX, textY, item.Title, textColor, 1);

            if (isSelected)
                Graphics.DrawText(X + Width - 24, textY, ">", GlobalDefines.Accent, 1);
        }

        public void HandleInput(int touchX, int touchY, bool pressed)
        {
            if (!IsVisible) return;

            int headerOffset = GetHeaderHeight();
            int relY = touchY - Y - headerOffset;

            if (touchX < X || touchX > X + Width || touchY < Y || touchY > Y + Height)
            {
                _hoveredIndex = -1;
                return;
            }

            int index = relY / ItemH;
            if (index < 0 || index >= Items.Count)
            {
                _hoveredIndex = -1;
                return;
            }

            _hoveredIndex = index;

            if (pressed)
            {
                _selectedIndex = index;
                Items[index].OnClick?.Invoke();
            }
        }

        public void Update()
        {
            int required = GetHeaderHeight() + Items.Count * ItemH + 40;
            if (Height < required)
                Height = required;
        }

        private int GetHeaderHeight()
        {
            int h = SectionH;
            if (!string.IsNullOrEmpty(Title)) h += LogoBlockH;
            return h;
        }
    }
}
