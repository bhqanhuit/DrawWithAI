using System;
using System.Collections.Generic;

namespace DrawApi.Models;

public partial class SketchToImageView
{
    public int? UserId { get; set; }

    public int SketchId { get; set; }

    public string SketchName { get; set; } = null!;

    public string? Prompt { get; set; }

    public int ImageId { get; set; }

    public string ImageName { get; set; } = null!;

    public DateTime? UploadAt { get; set; }
}
