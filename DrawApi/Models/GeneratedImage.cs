using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DrawApi.Models;

public partial class GeneratedImage
{
    [Key]
    public int ImageId { get; set; }

    public string ImageName { get; set; } = null!;

    public int? SketchId { get; set; }

    public virtual Sketch? Sketch { get; set; }
}
