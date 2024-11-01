using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace DrawClientMaui.Models
{
    public class PathModel
    {
        public Color Color { get; set; }
        public float Size { get; set; }
        public List<Point> Points { get; set; } = new();
    }
}