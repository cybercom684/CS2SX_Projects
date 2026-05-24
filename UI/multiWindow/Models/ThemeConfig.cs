namespace multiWindow.Models
{
    public class ThemeConfig
    {
        public uint Primary      { get; set; }
        public uint Secondary    { get; set; }
        public uint Background   { get; set; }
        public uint Foreground   { get; set; }
        public uint TitleBar     { get; set; }
        public uint TitleBarText { get; set; }
        public uint Border       { get; set; }
        public uint Accent       { get; set; }

        public static ThemeConfig Dark()
        {
            ThemeConfig t = new ThemeConfig();
            t.Background   = Color.RGB( 28,  28,  28);
            t.Foreground   = Color.White;
            t.Primary      = Color.RGB(  0, 120, 215);
            t.Secondary    = Color.RGB( 60,  60,  60);
            t.TitleBar     = Color.RGB( 42,  42,  42);
            t.TitleBarText = Color.White;
            t.Border       = Color.RGB( 70,  70,  70);
            t.Accent       = Color.RGB(  0, 120, 215);
            return t;
        }
    }
}
