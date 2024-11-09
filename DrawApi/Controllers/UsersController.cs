using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrawApi.Data;
using DrawApi.Models;
using DrawApi.Services;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json.Linq;

namespace DrawApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }



        // POST api/login/
        [HttpPost("login")]
        public async Task<IActionResult> UserLogin([FromBody] loginRequest RequestLogIn)
        {
            Console.WriteLine("testing login");
            var user = await _userService.Authenticate(RequestLogIn.Username, RequestLogIn.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = _userService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        // PUT api/register
        [HttpPut("register")]
        public async Task<IActionResult> UserRegistor([FromBody] UserRegisterRequest userRegisterRequest)
        {
            var newUser = new User
            {
                Username = userRegisterRequest.Username,
                Password = userRegisterRequest.Password,
                Email = userRegisterRequest.Email,
            };

            var _user = await _userService.Register(newUser);
            if (_user == null)
            {
                return BadRequest();
            }
            return Ok(_user);

        }

        [Authorize]
        [HttpGet("{UserID}")]
        public async Task<IActionResult> GetProfile(string UserID)
        {
            Console.WriteLine(UserID);
            // Extract user data from JWT claims
            var UserData = await _userService.GetUserData(UserID);

            if (UserData == null)
            {
                return NotFound();
            }

            return Ok(UserData);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            // Retrieve the JWT token from the Authorization header
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest("Invalid or missing token.");
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            // Decode the token
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken;

            try
            {
                jwtToken = handler.ReadJwtToken(token);
            }
            catch (Exception)
            {
                return BadRequest("Invalid token format.");
            }

            // Extract all claims into a dictionary
            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);

            // Return the full payload as JSON
            return Ok(new
            {
                Header = jwtToken.Header,
                Payload = claims
            });


        }


        }
    }
