using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using DrawClientMaui.Models;

namespace DrawClientMaui.Services
{
    internal class UserServices
    {

        public class TokenResponse
        {
            public string token { get; set; }
        }

        public async Task StoreTokenAsync(string token)
        {
            await SecureStorage.SetAsync("token", token);
        }

        // public async Task<bool> LoginAndSetAuthHeaderAsync(string username, string password)
        // {
        //     var token = await LoginAsync(username, password);
        //     if (token != null)
        //     {
        //         await StoreTokenAsync(token);
        //         ApiService.SetAuthToken(token); // Set token for future requests
        //         return true;
        //     }

        //     return false;
        // }

        public async Task<UserModel> GetUserDataAsync()
        {
            try
            {
                // var token = await SecureStorage.GetAsync("token");
                // if (token == null)
                //     throw new InvalidOperationException("User is not authenticated");

                // ApiService.SetAuthToken(token);
                var response = await ApiService.Client.GetAsync("/api/User/me");
                response.EnsureSuccessStatusCode();

                var user = await response.Content.ReadFromJsonAsync<UserModel>();
                if (user == null)
                {
                    throw new InvalidOperationException("Failed to retrieve user data.");
                }
                return user;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new InvalidOperationException($"Failed to get user data: {ex.Message}", ex);
            }
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            try
            {
                var loginData = new { Username = username, Password = password };

                var response = await ApiService.Client.PostAsJsonAsync("/api/User/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(jsonResponse);
                    ApiService.SetAuthToken(tokenResponse.token);
                    // if (tokenResponse?.token != null)
                    // {
                    //     await StoreTokenAsync(tokenResponse.token);
                    // }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new InvalidOperationException($"Failed to validate credentials: {ex.Message}", ex);
            }
        }
    }
}