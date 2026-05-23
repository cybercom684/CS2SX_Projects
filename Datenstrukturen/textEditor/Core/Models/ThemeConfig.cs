namespace textEditor.Core.Models
{
    public class ThemeConfig
    {
        public string ThemeName { get; set; } = "Light";
        public uint BackgroundColor { get; set; }
        public uint ForegroundColor { get; set; }
        public uint PrimaryColor { get; set; }
        public uint AccentColor { get; set; }
        public uint HighlightColor { get; set; }
        public uint SecondaryColor { get; set; }
    }
}
