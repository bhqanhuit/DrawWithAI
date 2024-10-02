using System;

namespace DrawWithAI.Models
{
    public class ImageProcessRequest
    {
        public IFormFile Image { get; set; }
        public string Prompt { get; set; }
    }
}
