using System;
using System.Collections.Generic;

namespace DrawApi.Models;

public partial class Sketch
{
    public int SketchId { get; set; }

    public string SketchName { get; set; } = null!;

    public string? Prompt { get; set; }

    public DateTime? UploadAt { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<GeneratedImage> GeneratedImages { get; set; } = new List<GeneratedImage>();

    public virtual User? User { get; set; }
}
