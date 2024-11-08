
using DrawApi.Exceptions;
using DrawWithAI.DrawApi.Models;
using DrawWithAI.DrawApi.Services;


namespace DrawWithAI.DrawApi.Services
{
    
    public class ImageAIService
    {
        public readonly string AI_URI = "http://192.168.6.200:5000/api";
        private readonly HttpClient _httpClient;

        public ImageAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<String> GetImageFromAIAsync(string namePath, string prompt, string ImageId)
        {
            Console.WriteLine("uploading path: ");
            
            AIResponse aiResponse;
            using (MultipartFormDataContent formDataContent = new MultipartFormDataContent())
            {
                // Send Request to AI and receive AiResponse
                KeyValuePair<string, string>[] keyValuePairs = new[]
                {
                new KeyValuePair<string, string>("Id", ImageId),
                new KeyValuePair<string, string>("NamePath", namePath),
                new KeyValuePair<string, string>("Prompt", prompt)
                };

                foreach (var keyValuePair in keyValuePairs)
                {
                    formDataContent.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                }
                aiResponse = await _httpClient.PostAsync(AI_URI, formDataContent).Result.Content.ReadFromJsonAsync<AIResponse>();

            }


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