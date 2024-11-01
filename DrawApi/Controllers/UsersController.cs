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
            var user = await _userService.Authenticate(RequestLogIn.Username, RequestLogIn.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(user);
        }

        // PUT api/register
        [HttpPut("registor")]
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

        

    }
}
