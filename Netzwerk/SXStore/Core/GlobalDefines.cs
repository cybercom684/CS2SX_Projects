namespace SXStore.Core
{
    /// <summary>
    /// Statische App- und GUI-weite Konstanten, z.B. Farben, Layout-Maße, etc.
    /// </summary>
    public static class GlobalDefines
    {
        // App-Info
        public const string AppName = "SXStore";
        public const string AppDescription = "Ein moderner Store für Software, Spiele und mehr.";
        public const string AppAuthor = "Cybercom684";
        public const string AppVersion = "0.1.0";


        // Bildschirmauflösung
        public const int WindowWidth = 1280;
        public const int WindowHeight = 720;

        // Farben
        public const uint Background = 0xFF000000; // Entspricht Color.Black (ARGB: 0xFF000000)
        public const uint Foreground = 0xFFFFFFFF; // Entspricht Color.White (ARGB: 0xFFFFFFFF)
        public const uint Accent     = 0xFF89B4FA; // Ein helles Blau als Akzentfarbe
        public const uint AccentDim  = 0xFF313264; // Ein dunkleres Blau für Hover/Active-Zustände
        public const uint Text       = 0xFFCDD6F4; // Ein helles Grau für normalen Text
        public const uint TextMuted  = 0xFF7F849C; // Ein gedämpftes Grau für sekundären Text
        public const uint Border     = 0xFF313244; // Ein dunkles Grau für Rahmen
        public const uint Separator  = 0xFF24243A; // Ein dunkleres Grau für Trennlinien

        // Layout-Maße
        // -- Sidebar
        public const int SidebarWidth = WindowWidth / 5;
        public const int SidebarHeight = WindowHeight;

        // -- Content
        public const int ContentWidth = WindowWidth - SidebarWidth;
        public const int ContentHeight = WindowHeight;

        // -- Extras
        public const int HeaderHeight = 70;

        public const int Padding = 16;
        public const int Margin = 16;
        public const int BorderRadius = 6;
        public const int BorderRadiusSmall = 3;
    }
}
