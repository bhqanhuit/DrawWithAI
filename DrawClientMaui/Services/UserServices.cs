using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;


namespace DrawClientMaui.Services
{
    internal class UserServices
    {
        public async Task<string?> LoginAsync(string username, string password)
        {
            var loginData = new { Username = username, Password = password };

            var response = await ApiService.Client.PostAsJsonAsync("/api/user/login", loginData);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(jsonResponse);
                return tokenResponse?.Token; // Assuming the token is returned as { "token": "your_jwt_token" }
            }

            return null;
        }

        public class TokenResponse
        {
            public string Token { get; set; }
        }

        public async Task StoreTokenAsync(string token)
        {
            await SecureStorage.SetAsync("jwt_token", token);
        }

        public async Task<bool> LoginAndSetAuthHeaderAsync(string username, string password)
        {
            var token = await LoginAsync(username, password);
            if (token != null)
            {
                await StoreTokenAsync(token);
                ApiService.SetAuthToken(token); // Set token for future requests
                return true;
            }

            return false;
        }
    }
}
