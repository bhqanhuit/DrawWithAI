using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawClientMaui.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        //method to validate the user credentials (username and password)
        public static bool ValidateCredentials(string username, string password)
        {
            //validate username and password (use hashing)

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
