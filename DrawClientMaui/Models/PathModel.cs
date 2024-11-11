using SkiaSharp;
using System.Collections.Generic;

namespace DrawClientMaui.Models
{
    public class PathModel
    {
        public List<SKPoint> Points { get; set; } = new List<SKPoint>();
        public float Size { get; set; }
        public SKColor Color { get; set; }
    }
}
