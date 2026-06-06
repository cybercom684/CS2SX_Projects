using System;
using System.Drawing;

namespace SXStore.Core.GUI.Components
{
    public static class Card
    {
        private static int imageSize = 120;

        private static void DrawBase(int x, int y, int width, int height, uint bgColor, uint borderColor)
        {
            Graphics.FillRoundedRect(x, y, width, height, GlobalDefines.BorderRadius, bgColor);
            Graphics.DrawRoundedRect(x, y, width, height, GlobalDefines.BorderRadius, borderColor);

        }
        public static void Draw(int x, int y, int width, int height,uint bgColor,uint borderColor, Action<int,int,int,int> content)
        {
            DrawBase(x, y, width, height, bgColor, borderColor);
            
            int cx = x + GlobalDefines.Padding;
            int cy = y + GlobalDefines.Padding;
            int cwidth = width - GlobalDefines.Padding;
            int cheight = height - (GlobalDefines.Padding * 2);

            content(cx,cy, cwidth,cheight);
        }
    }
}
