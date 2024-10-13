using System;
using System.Collections.Generic;

namespace DrawApi.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Sketch> Sketches { get; set; } = new List<Sketch>();
}
