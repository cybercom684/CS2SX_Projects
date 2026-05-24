namespace multiWindow.UI.Controls
{
    public class UIControl
    {
        public int  X       { get; set; }
        public int  Y       { get; set; }
        public int  Width   { get; set; }
        public int  Height  { get; set; }
        public bool Visible { get; set; } = true;

        public virtual void Draw(int offsetX, int offsetY) { }

        public virtual bool HandleTouch(TouchState touch, bool touchBegan, int offsetX, int offsetY)
            => false;

        protected bool HitTest(int touchX, int touchY, int offsetX, int offsetY)
        {
            int ax = offsetX + X;
            int ay = offsetY + Y;
            return touchX >= ax && touchX <= ax + Width
                && touchY >= ay && touchY <= ay + Height;
        }
    }
}
