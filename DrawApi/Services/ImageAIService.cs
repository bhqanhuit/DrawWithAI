
using DrawWithAI.DrawApi.Models;
using DrawWithAI.DrawApi.Services;

namespace DrawWithAI.DrawApi.Services
{
    public class ImageAIService
    {
        private readonly HttpClient _httpClient;
        private readonly ImageDriveService _imageDriveService;

        public ImageAIService(HttpClient httpClient, ImageDriveService imageDriveService)
        {
            _httpClient = httpClient;
            _imageDriveService = imageDriveService;
        }


        public string GetImageFromAI(string namePath, string prompt)
        {
            // Form AI Request (include namePath and prompt)
            // Send Request to AI
            // Get Response from AI
            // Extract namePath from Response
            string resultNamePath = "test.png";
            return resultNamePath;
        }
    }
}

