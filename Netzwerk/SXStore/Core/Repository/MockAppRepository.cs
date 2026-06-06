using SXStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SXStore.Core.Repository
{
    // Repository/MockAppRepository.cs
    public class MockAppRepository : IAppRepository
    {
        private readonly List<AppEntry> _entries = new List<AppEntry>
    {
        new AppEntry
        {
            Id           = "hbmenu",
            Title        = "Homebrew Menu",
            Description  = "Der offizielle Homebrew-Launcher für die Nintendo Switch. Startet NRO-Dateien von der SD-Karte.",
            Author       = "switchbrew",
            Version      = "3.4.0",
            Category     = AppCategory.Tools,
            SizeBytes    = 1_200_000,
            ThumbnailPath = "romfs:/thumbnails/hbmenu.png",
            NroPath      = "romfs:/nro/hbmenu.nro",
            IsFeatured   = true
        },
        new AppEntry
        {
            Id           = "retroarch",
            Title        = "RetroArch",
            Description  = "Multi-System-Emulator Frontend. Unterstützt zahlreiche Cores für klassische Konsolen.",
            Author       = "libretro",
            Version      = "1.16.0",
            Category     = AppCategory.Emulator,
            SizeBytes    = 42_000_000,
            ThumbnailPath = "romfs:/thumbnails/retroarch.png",
            NroPath      = "romfs:/nro/retroarch.nro",
            IsFeatured   = true
        },
        new AppEntry
        {
            Id           = "ftpd",
            Title        = "ftpd",
            Description  = "FTP-Server für die Switch. Ermöglicht kabellosen Dateizugriff über das Netzwerk.",
            Author       = "mtheall",
            Version      = "3.1.0",
            Category     = AppCategory.Tools,
            SizeBytes    = 800_000,
            ThumbnailPath = "romfs:/thumbnails/ftpd.png",
            NroPath      = "romfs:/nro/ftpd.nro",
            IsFeatured   = false
        },
        new AppEntry
        {
            Id           = "nx-ovlloader",
            Title        = "NX-OvlLoader",
            Description  = "Tesla Overlay Loader. Grundlage für das Tesla-Overlay-System.",
            Author       = "WerWolv",
            Version      = "1.0.7",
            Category     = AppCategory.Tools,
            SizeBytes    = 650_000,
            ThumbnailPath = "romfs:/thumbnails/ovlloader.png",
            NroPath      = "romfs:/nro/ovlloader.nro",
            IsFeatured   = false
        },
        new AppEntry
        {
            Id           = "chiaki",
            Title        = "Chiaki4deck",
            Description  = "PlayStation Remote Play Client für die Nintendo Switch.",
            Author       = "streetpea",
            Version      = "2.2.0",
            Category     = AppCategory.Media,
            SizeBytes    = 18_500_000,
            ThumbnailPath = "romfs:/thumbnails/chiaki.png",
            NroPath      = "romfs:/nro/chiaki.nro",
            IsFeatured   = true
        },
        new AppEntry
        {
            Id           = "2048",
            Title        = "2048",
            Description  = "Der klassische 2048-Klon für die Switch. Einfaches Puzzle-Spiel.",
            Author       = "SnapCrackleFlop",
            Version      = "1.0.0",
            Category     = AppCategory.Games,
            SizeBytes    = 320_000,
            ThumbnailPath = "romfs:/thumbnails/2048.png",
            NroPath      = "romfs:/nro/2048.nro",
            IsFeatured   = false
        },
        new AppEntry
        {
            Id           = "dbi",
            Title        = "DBI",
            Description  = "NSP/XCI Installer mit USB- und MTP-Support. Installiert Spiele direkt vom PC.",
            Author       = "rashevskyv",
            Version      = "627",
            Category     = AppCategory.Tools,
            SizeBytes    = 2_100_000,
            ThumbnailPath = "romfs:/thumbnails/dbi.png",
            NroPath      = "romfs:/nro/dbi.nro",
            IsFeatured   = true
        },
        new AppEntry
        {
            Id           = "mpv-switch",
            Title        = "mpv for Switch",
            Description  = "Portierung des mpv Media Players. Spielt Videos von der SD-Karte ab.",
            Author       = "nicoboss",
            Version      = "0.35.1",
            Category     = AppCategory.Media,
            SizeBytes    = 9_800_000,
            ThumbnailPath = "romfs:/thumbnails/mpv.png",
            NroPath      = "romfs:/nro/mpv.nro",
            IsFeatured   = false
        },
    };

        public IReadOnlyList<AppEntry> GetAll() => _entries;

        public IReadOnlyList<AppEntry> GetByCategory(string category) =>
            _entries.FindAll(e => e.Category == category);

        public IReadOnlyList<AppEntry> GetFeatured() =>
            _entries.FindAll(e => e.IsFeatured);

        public IReadOnlyList<AppEntry> Search(string query)
        {
            string q = query.ToLowerInvariant();
            return _entries.FindAll(e =>
                e.Title.ToLowerInvariant().Contains(q) ||
                e.Author.ToLowerInvariant().Contains(q) ||
                e.Description.ToLowerInvariant().Contains(q));
        }

        public AppEntry GetById(string id) =>
            _entries.Find(e => e.Id == id);
    }
}
