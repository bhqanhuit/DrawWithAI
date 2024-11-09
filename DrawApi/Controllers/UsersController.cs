using Microsoft.AspNetCore.Mvc;
using DrawApi.Services;
using DrawApi.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DrawApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.AuthenticateAsync(request.Username, request.Password);
            if (user == null) return Unauthorized("Invalid username or password");

            var token = _userService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

   
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new User { Username = request.Username, Email = request.Email };
            var registeredUser = await _userService.RegisterAsync(user);

            if (registeredUser == null) return BadRequest("Username already exists");

            var token = _userService.GenerateJwtToken(registeredUser);
            return Ok(new { Token = token });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var username = User.Identity?.Name;
            if (username == null) return Unauthorized();

            var user = await _userService.GetUserDataAsync(username);
            if (user == null) return NotFound();

            return Ok(user);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
