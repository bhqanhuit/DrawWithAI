using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using DrawWithAI.DrawApi.Models;
using DrawWithAI.DrawApi.Services;

namespace DrawApi.Controllers
{
    // https://localhost:5001/api/ImageProcess 
    [Route("api/[controller]")]
    [ApiController]
    public class ImageProcessController(ImageDriveService imageDriveService, ImageAIService imageAiService)
        : ControllerBase
    {
        private readonly ImageAIService _imageAiService = imageAiService;
        private readonly ImageDriveService _imageDriveService = imageDriveService;

        [HttpPost]
        public IActionResult Post([FromBody] ImageProcessRequest request)
        {
            // Get the image from the request.ImagePath (on local)
            // Call ImageDriveService.UploadImage (request.ImagePath) to upload the image to drive service --> get the namePath (on Drive)
            // Call ImageAIService.GetImageFromAI(namePath, request.Prompt) -->  get the processed image namePath (on Drive)
            // Call ImageDriveService.DownloadImage(processedImageNamePath) --> get the processed image imagePath (on local)
            // Return  the processed image imagePath to Client

            string imageFolder = Path.GetFullPath(@"..\Images\"); // turn relative path to absolute path
            string imagePath = Path.Combine(imageFolder, request.ImagePath); // combine folder path with the filename
            Console.WriteLine(imagePath);
            Console.WriteLine("Upload the images to drive...");
            string driveNamePath = imageDriveService.UploadImage(imagePath);

            Console.WriteLine("Get the image from AI...");
            string resultDriveNamePath = imageAiService.GetImageFromAI(driveNamePath, request.Prompt);
            Console.WriteLine("Download the images from drive...");
            string resultImagePath = imageDriveService.DownloadImage(resultDriveNamePath, imageFolder);
            
            ImageProcessResponse response = new ImageProcessResponse
            {
                ResultImagePath = resultImagePath,
                Status = "Success",
                Message = "Image Processed Successfully"
            };
            // Return the processed image
            return Ok(response);
        }
    }
    
}
