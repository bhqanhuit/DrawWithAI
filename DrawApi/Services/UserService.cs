using System;
using System.Linq;
using System.Threading.Tasks;
using DrawApi.Models;
using Microsoft.EntityFrameworkCore;
using DrawApi.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;

namespace DrawApi.Services
{

    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        Task<User?> RegisterAsync(User user);
        Task<User?> GetUserDataAsync(string username);
        Task<bool> UserExistsAsync(string username);
        Task<string> GetUserIdByUsername(string username);
        string GenerateJwtToken(User user);
        Task LogoutAsync();
    }


    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private string? _token;

        public UserService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Authenticates user by checking username and password directly
        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        // Registers a new user without password hashing
        public async Task<User?> RegisterAsync(User user)
        {
            if (await UserExistsAsync(user.Username))
            {
                return null; // Username already exists
            }

            user.CreatedAt = DateTime.UtcNow;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> GetUserDataAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<string> GetUserIdByUsername(string username)
        {
            var userId = await _context.Users
                               .Where(u => u.Username == username)
                               .Select(u => u.UserId)
                               .FirstOrDefaultAsync();

            return userId.ToString();
        }

        // Checks if a username already exists in the database
        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        // Generates a JWT token for the authenticated user
        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Logs out the user (e.g., clearing token or session)
        public async Task LogoutAsync()
        {
            _token = null;
            await Task.CompletedTask;
        }
    }




}
