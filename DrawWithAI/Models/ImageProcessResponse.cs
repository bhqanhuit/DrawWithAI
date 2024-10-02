using System;

namespace DrawWithAI.Models
{
    public class ImageProcessResponse
    {
        public string FileUrl { get; set; }
        public string Prompt { get; set; }
        public string ResultImagePath { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}

