using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SXStore.Core.Models
{
    public class AppEntry
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Category { get; set; }
        public long SizeBytes { get; set; }
        public string ThumbnailPath { get; set; }
        public string NroPath { get; set; }
        public bool IsFeatured { get; set; }

        public string SizeFormatted =>
            SizeBytes >= 1_048_576
                ? $"{SizeBytes / 1_048_576.0:F1} MB"
                : $"{SizeBytes / 1024} KB";
    }
}
