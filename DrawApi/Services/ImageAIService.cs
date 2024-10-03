
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

        public string ExtractImageFromResponse(AIResponse response)
        {
            return "Extracted Image from Response";
        }

        public async Task<AIResponse> GetImageFromAI(string imageUrl, string textPrompt)
        {
            
        }
    }
}

