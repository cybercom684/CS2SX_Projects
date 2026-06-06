using CS2SX.Switch;

namespace SXStore.Core.GUI.Components
{
    public static class Image
    {
        public static void Draw(int x, int y, int width, int height,string path)
        {
            //if (File.Exists(path))
            //{
            //    Texture img = Graphics.LoadTexture(path);
            //    Graphics.DrawTexture(img, x, y);
            //}

            //Placeholder
            string infoText = "NO IMAGE";
            var textWidth = Graphics.MeasureTextWidth(infoText, 1);
            var textHeight = Graphics.MeasureTextHeight(1);

            int centeredX = x + (width / 2) - (textWidth / 2);
            int centeredY = y + (height / 2) - (textHeight / 2);
            Graphics.FillRoundedRect(x, y, width, height, GlobalDefines.BorderRadius, Color.Black);
            Graphics.DrawRoundedRect(x,y,width,height,GlobalDefines.BorderRadius, Color.Red);
            Graphics.DrawText(centeredX,centeredY,infoText,Color.Red,1);
        }
    }
}
