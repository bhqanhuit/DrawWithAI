using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using DrawApi.Exceptions;
using SkiaSharp;
using DrawWithAI.DrawApi.Models;
using DrawWithAI.DrawApi.Services;
using System.Drawing.Printing;

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
        public readonly string imageFolder = Path.GetFullPath(@"../Images/");  // turn relative path to absolute path

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile image, [FromForm] string prompt)
        {
            /*
             * Get the image from the request.ImagePath (on local)
             * Call ImageDriveService.UploadImage (request.ImagePath) to upload the image to drive service --> get the namePath (on Drive)
             * Call ImageAIService.GetImageFromAI(namePath, request.Prompt) -->  get the processed image namePath (on Drive)
             * Call ImageDriveService.DownloadImage(processedImageNamePath) --> get the processed image imagePath (on local)
             * Return  the processed image imagePath to Client
             */
            if (!ModelState.IsValid) throw new BadRequestException("The request is not valid!");
            
             // string imagePath = Path.Combine(imageFolder, "sketchh.png"); 
             // combine folder path with the filename

            // string imagePath = Path.Combine(imageFolder, request.ImagePath); // combine folder path with the filename
            // Console.WriteLine(imagePath);
            Console.WriteLine("Upload the images to drive...");

            // Read the byte array from the request body
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            // Save the image locally
            string imagePath = SaveImageLocally(imageBytes, "uploaded_image.png");
            Console.WriteLine("Image saved locally at: ");
            Console.WriteLine(imagePath);
        

            ImageDriveService DriveController = new ImageDriveService();
            string driveNamePath = DriveController.UploadImage(imagePath);
            Console.WriteLine($"Image uploaded and saved at {driveNamePath}");

            if (string.IsNullOrEmpty(driveNamePath)) throw new DriveServiceException("Failed to upload the image to drive.");
            

            Console.WriteLine("Get the image from AI...");

            return Ok();
            // string resultDriveNamePath = await _imageAiService.GetImageFromAIAsync(driveNamePath, request.Prompt.ToString());
            // Console.WriteLine("Download the images from drive...");


            // Console.WriteLine("Results image path: ", resultDriveNamePath, imageFolder);
            // string resultImagePath = _imageDriveService.DownloadImage(resultDriveNamePath, imageFolder);
            
        //     ImageProcessResponse response = new ImageProcessResponse
        //     {
        //         ResultImagePath = resultImagePath,
        //         Status = "Success",
        //         Message = "Image Processed Successfully"
        //     };

        //     // Return the processed image
        //     return Ok(response);
        }

         private string SaveImageLocally(byte[] imageBytes, string fileName)
        {
            string filePath = Path.Combine(imageFolder, fileName);
            Console.WriteLine(filePath);

            using (var image = SKBitmap.Decode(imageBytes))
            using (var imageStream = new SKFileWStream(filePath))
            {
                image.Encode(imageStream, SKEncodedImageFormat.Png, 100);
            }

            return filePath;
        }
        
    }
    
}
