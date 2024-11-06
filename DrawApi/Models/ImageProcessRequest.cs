using System;
using System.ComponentModel.DataAnnotations;

namespace DrawWithAI.DrawApi.Models
{
    public class ImageProcessRequest
    {
        // [Required]
        // public required string ImagePath { get; set; }
        // [Required]
        // public required string Prompt { get; set; }
        [Required]
        public required ByteArrayContent Image { get; set; }
        [Required]
        public required StringContent Prompt { get; set; }
    }
}
