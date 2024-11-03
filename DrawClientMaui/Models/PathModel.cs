using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;

namespace DrawClientMaui.Models
{
    public class PathModel
    {
        public SixLabors.ImageSharp.Color Color { get; set; }
        public float Size { get; set; }
        public List<SixLabors.ImageSharp.PointF> Points { get; set; } = new();
    }
}