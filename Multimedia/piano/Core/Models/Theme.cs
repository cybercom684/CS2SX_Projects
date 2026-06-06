namespace piano.Core.Models
{
    public class Theme
    {
        public string Name { get; set; }

        public uint BackgroundColor { get; set; }
        public uint ForegroundColor { get; set; }
        public uint AccentColor { get; set; }

        public uint PianoPrimaryKeyColor { get; set; }
        public uint PianoPrimaryKeyTextColor { get; set; }
        public uint PianoSecondaryKeyColor { get; set; }
        public uint PianoSecondaryKeyTextColor { get; set; }
    }
}
