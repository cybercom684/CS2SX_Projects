using System.Collections.Generic;

namespace SXStore.Core.GUI.Components
{
    public static class ContentActions
    {
        private const int CX = GlobalDefines.SidebarWidth;
        private const int CW = GlobalDefines.ContentWidth;
        private const int CActionY = GlobalDefines.WindowHeight - 15;
        private const uint CBorder = 0xFF313244;
        private const uint CMuted = 0xFF7F849C;

        public static void Draw(List<string> actions)
        {
            Graphics.FillRect(CX - 16, CActionY - 8, CW + 16, 1, CBorder);
            Graphics.DrawText(CX, CActionY, string.Join(" | ", actions), CMuted, 1);
        }
    }
}
