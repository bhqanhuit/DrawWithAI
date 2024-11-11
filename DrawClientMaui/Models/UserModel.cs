using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using DrawClientMaui.Services;
using System.Security.Principal;

namespace DrawClientMaui.Models
{
    public class UserModel
    {
        public static string Token { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        //method to validate the user credentials (username and password)
        public static async Task<bool>ValidateCredentialsAsync(string username, string password)
        {
            //validate username and password (use hashing)
            using var httpClient = new HttpClient();
            var request = new
            {
                Username = username,
                Password = password
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("http://localhost:5160/api/User/login", content);
            if (response == null || !response.IsSuccessStatusCode)
            {
                return false; // invalid credentials
            }
            UserModel.Token = await response.Content.ReadAsStringAsync();
            return true; // successfully validating
        }

        //method to store user credentials
        public static async Task<bool> StoreCredentials(string username, string email, string password)
        {
            //check if the user already exists

            //store new user's credentials

            //add the user to simulated database

            return true; // Successfully stored
        }

        //method to retrieve stored credentials (ex: display or user profile page)
        public static UserModel RetrieveCredentials(string username)
        {
            //return user;

            return null; //user not found
        }
    }
}
