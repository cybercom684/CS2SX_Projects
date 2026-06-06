using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SXStore.Core.GUI.Components
{
    public static class TextLabel
    {
        public static void Draw(int x, int y, string text,int scale, uint foreground)
        {
            Graphics.DrawText(x, y, text, foreground, scale);
        }

        public static void DrawBadged(int x, int y, int width, int height, string text, int scale, uint foreground, uint background)
        {
            int textWidth = Graphics.MeasureTextWidth(text,scale);
            int textHeight = Graphics.MeasureTextHeight(scale);

            int centeredX = x + (width / 2) - (textWidth / 2);
            int centeredY = y + (height / 2) - (textHeight / 2);
            Graphics.FillRoundedRect(x,y,width,height,GlobalDefines.BorderRadius,background);
            Draw(centeredX, centeredY, text,scale, foreground);
        }

        public static void DrawLined(int x, int y, string text, int textScale,int width, uint foreground, uint hrColor)
        {
            int textHeigth = Graphics.MeasureTextHeight(textScale);
            Graphics.DrawText(x, y, text, foreground, textScale);
            Graphics.FillRect(x, y + textHeigth + 5, width, 2, hrColor);
        }
    }
}
