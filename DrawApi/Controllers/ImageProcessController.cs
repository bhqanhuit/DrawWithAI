using DrawApi.Exceptions;
using DrawApi.Models;
using DrawApi.Services;
using DrawWithAI.DrawApi.Models;
using DrawWithAI.DrawApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using System.Security.Claims;
using System.Security.Principal;

namespace DrawApi.Controllers
{
    // https://localhost:5001/api/ImageProcess 
    [Route("api/[controller]")]
    [ApiController]
    public class ImageProcessController : ControllerBase
    {
        private readonly ImageAIService _imageAiService;
        private readonly ImageDriveService _imageDriveService;
        private readonly ISketchService _sketchService;
        private readonly IUserService _userService;
        public readonly string imageFolder = Path.GetFullPath(@"../Images/");  // turn relative path to absolute path

        public ImageProcessController(ImageDriveService imageDriveService, ImageAIService imageAiService, ISketchService sketchService, IUserService userService)
        {
            _imageDriveService = imageDriveService;
            _imageAiService = imageAiService;
            _sketchService = sketchService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var sketchesId = await _sketchService.GetLatestSketch();
            return Ok(sketchesId);
        }

        [Authorize]
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
            string userId = null;
            if (User.Identity is ClaimsIdentity identity)
            {
                var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                string username = userIdClaim.Value;
                userId = _userService.GetUserIdByUsername(username).Result;
                Console.WriteLine(username);
            }
            else
            {
                return Unauthorized("User ID not found in token.");
            }

            Console.WriteLine("Upload the images to drive...");

            // Read the byte array from the request body
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }


            // Save the image locally
            var current_sketchID = await _sketchService.GetLatestSketch() + 1;
            string imagePath = SaveImageLocally(imageBytes, userId + "_" + current_sketchID.ToString() + ".png");
            Console.WriteLine("Image saved locally at: ");
            Console.WriteLine(imagePath);

            // sketch data create


            var imageName = userId + "_" + current_sketchID.ToString();
            Console.WriteLine(imageName);
            var newSketch = new Sketch
            {
                SketchName = imageName,
                Prompt = prompt,
                UserId = int.Parse(userId),
            };
            var sketch = _sketchService.InsertSketchToDatabase(newSketch);

            ImageDriveService DriveController = new ImageDriveService();
            string driveNamePath = DriveController.UploadImage(imagePath);
            Console.WriteLine($"Image uploaded and saved at {driveNamePath}");

            if (string.IsNullOrEmpty(driveNamePath)) throw new DriveServiceException("Failed to upload the image to drive.");

            Console.WriteLine("Get the image from AI...");
            string resultDriveNamePath = await _imageAiService.GetImageFromAIAsync(driveNamePath, prompt, imageName);
            Console.WriteLine("Download the images from drive...");


            Console.WriteLine("Results image path: ", resultDriveNamePath, imageFolder);
            Console.WriteLine(resultDriveNamePath);
            Console.WriteLine(imageFolder);
            string resultImagePath = _imageDriveService.DownloadImage(resultDriveNamePath, imageFolder);

            ImageProcessResponse response = new ImageProcessResponse
            {
                ResultImagePath = resultImagePath,
                Status = "Success",
                Message = "Image Processed Successfully"
            };


            // insert generated image to database
            var generatedImage = new GeneratedImage
            {
                ImageName = resultDriveNamePath,
                SketchId = current_sketchID,
            };
            await _sketchService.InsertGeneratedImageToDatabase(generatedImage);

            // Read the image file into a byte array
            resultImagePath = Path.Combine(@"../Images/" + resultDriveNamePath);
            byte[] resultBytes = System.IO.File.ReadAllBytes(resultImagePath);
            ByteArrayContent byteArrayContent = new ByteArrayContent(resultBytes);
            ImageToClient responseClient = new ImageToClient
            {
                Image = byteArrayContent
            };

            // Console.WriteLine(response.ResultImageP);

            // Return the processed image
            return File(resultBytes, "image/png");
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