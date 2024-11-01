using System;
using System.Linq;
using System.Threading.Tasks;
using DrawApi.Models;
using Microsoft.EntityFrameworkCore;
using DrawApi.Data;

namespace DrawApi.Services
{
    
    public interface IUserService
    {
        Task<User?> Authenticate(string username, string password);
        Task<User?> Register(User user);
        Task<bool> UserExists(string username);
    }

    public class UserService: IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<User?> Authenticate(string username, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<User?> Register(User user)
        {
            if (await UserExists(user.Username))
            {
                return null; // Username already exists
            }

            user.CreatedAt = DateTime.UtcNow;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

    }

    
}
