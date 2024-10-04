
using DrawApi.Exceptions;
using DrawWithAI.DrawApi.Models;
using DrawWithAI.DrawApi.Services;

namespace DrawWithAI.DrawApi.Services
{
    
    public class ImageAIService
    {
        public readonly string AI_URI = "http://localhost:5160/weatherforecast";
        private readonly HttpClient _httpClient;

        public ImageAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<String> GetImageFromAIAsync(string namePath, string prompt)
        {
            // Form AI Request
            RequestToAI aiRequest = new RequestToAI()
            {
                NamePath = namePath,
                Prompt = prompt
            };

            // Send Request to AI and receive AiResponse
            AIResponse aiResponse = await _httpClient.PostAsJsonAsync(AI_URI, aiRequest).Result.Content.ReadFromJsonAsync<AIResponse>();
            // Extract namePath from Response
            if (aiResponse != null && !string.IsNullOrEmpty(aiResponse.NamePath))
            {
                // Assuming the ImageUrl is the path to the result image
                return aiResponse.NamePath;
            }

            throw new AIServiceException("Failed to get a valid response from AI service.");
        }
    }
}