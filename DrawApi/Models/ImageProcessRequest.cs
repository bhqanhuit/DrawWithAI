using System;

namespace DrawWithAI.DrawApi.Models
{
    public class ImageProcessRequest
    {
        public required string ImagePath { get; set; }
        public required string Prompt { get; set; }
    }
}
