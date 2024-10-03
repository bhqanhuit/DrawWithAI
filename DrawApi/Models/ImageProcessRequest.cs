using System;

namespace DrawWithAI.DrawAPI.Models
{
    public class ImageProcessRequest
    {
        public required string ImagePath { get; set; }
        public required string Prompt { get; set; }
    }
}
