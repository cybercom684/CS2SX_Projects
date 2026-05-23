using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using textEditor.Core.Models;

namespace textEditor.Core.Controller
{
    public class EditorController
    {
        public ThemeConfig CurrentTheme { get; private set; }
        public List<ThemeConfig> AvailableThemes { get; private set; }

        public enum Themes
        {
            Dark,
            Light,
            Solarized,
            Dracula,
            Nord
        }

        public EditorController()
        {
            InitializeThemes();
            CurrentTheme = AvailableThemes.First();
        }

        public void SetTheme(Themes theme)
        {
            string themeName = theme.ToString();
            CurrentTheme = AvailableThemes.FirstOrDefault(t => t.ThemeName == themeName) ?? CurrentTheme;
        }

        private void InitializeThemes()
        {
            // Dark — blauer Akzent, gedämpftes Blaugrau-Highlight
            AvailableThemes.Add(new ThemeConfig
            {
                ThemeName = "Dark",
                BackgroundColor = Color.RGB(30, 30, 30),
                ForegroundColor = Color.RGB(220, 220, 220),
                SecondaryColor = Color.RGB(60, 60, 60),
                PrimaryColor = Color.RGB(86, 156, 214),
                AccentColor = Color.RGB(86, 156, 214),
                HighlightColor = Color.RGB(40, 60, 90)
            });

            // Light — klares Blau, helles Blaugrau-Highlight
            AvailableThemes.Add(new ThemeConfig
            {
                ThemeName = "Light",
                BackgroundColor = Color.RGB(245, 245, 245),
                ForegroundColor = Color.RGB(30, 30, 30),
                SecondaryColor = Color.RGB(210, 210, 210),
                PrimaryColor = Color.RGB(0, 122, 204),
                AccentColor = Color.RGB(0, 102, 180),
                HighlightColor = Color.RGB(210, 230, 255)
            });

            // Solarized — Cyan-Akzent, dunkles Teal-Highlight
            AvailableThemes.Add(new ThemeConfig
            {
                ThemeName = "Solarized",
                BackgroundColor = Color.RGB(0, 43, 54),
                ForegroundColor = Color.RGB(131, 148, 150),
                SecondaryColor = Color.RGB(7, 54, 66),
                PrimaryColor = Color.RGB(38, 139, 210),
                AccentColor = Color.RGB(42, 161, 152),
                HighlightColor = Color.RGB(0, 65, 80)
            });

            // Dracula — Pink-Akzent, dunkles Lila-Highlight
            AvailableThemes.Add(new ThemeConfig
            {
                ThemeName = "Dracula",
                BackgroundColor = Color.RGB(40, 42, 54),
                ForegroundColor = Color.RGB(248, 248, 242),
                SecondaryColor = Color.RGB(68, 71, 90),
                PrimaryColor = Color.RGB(189, 147, 249),
                AccentColor = Color.RGB(255, 121, 198),
                HighlightColor = Color.RGB(60, 45, 90)
            });

            // Nord — Eistürkis-Akzent, dunkles Slate-Highlight
            AvailableThemes.Add(new ThemeConfig
            {
                ThemeName = "Nord",
                BackgroundColor = Color.RGB(46, 52, 64),
                ForegroundColor = Color.RGB(236, 239, 244),
                SecondaryColor = Color.RGB(59, 66, 82),
                PrimaryColor = Color.RGB(136, 192, 208),
                AccentColor = Color.RGB(129, 161, 193),
                HighlightColor = Color.RGB(55, 70, 95)
            });
        }
    }
}