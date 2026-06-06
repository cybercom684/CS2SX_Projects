namespace SXStore.Core.GUI.Components
{
    public static class ContentHeader
    {
        private const uint CAccent = 0xFF89B4FA;
        private const uint CText = 0xFFCDD6F4;
        private const uint CMuted = 0xFF7F849C;
        private const uint CPanel = 0xFF181825;

        private static void DrawBase(string title)
        {
            Graphics.FillRect(GlobalDefines.SidebarWidth - 16, 0, GlobalDefines.ContentWidth + 16, GlobalDefines.HeaderHeight, CPanel);
            Graphics.FillRect(GlobalDefines.SidebarWidth - 16, GlobalDefines.HeaderHeight - 2, GlobalDefines.ContentWidth + 16, 2, CAccent);
            Graphics.DrawText(GlobalDefines.SidebarWidth, 24, title, CText, 2);
        }

        public static void Draw(string title,string subtitle)
        {
            DrawBase(title);
            Graphics.DrawText(GlobalDefines.SidebarWidth + GlobalDefines.ContentWidth - 180, 32, subtitle, CMuted, 1);
        }

        // TODO: Ein weiterer aufruf des selben Headers "Draw" führt zu kompilierungsfehlern, temp fix: DrawBadged
        //Später: cs2sx fixen, das sich funktionsnamen überladen lassen, dann hier einfach "Draw" verwenden
        public static void DrawBadged(string title, string badgeTitle, uint badgeColor)
        {
            DrawBase(title);
            int pillW = badgeTitle.Length * 9 + 20;
            Graphics.FillRoundedRect(GlobalDefines.SidebarWidth + GlobalDefines.ContentWidth - pillW - 4, 26, pillW, 20, GlobalDefines.BorderRadiusSmall, badgeColor);
            Graphics.DrawText(GlobalDefines.SidebarWidth + GlobalDefines.ContentWidth - pillW + 6, 32, badgeTitle, CMuted, 1);
        }
    }
}
