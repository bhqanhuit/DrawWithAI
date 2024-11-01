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
        public async Task<IActionResult> Login([FromBody] loginRequest RequestLogIn)
        {
            Console.WriteLine(RequestLogIn.Username, RequestLogIn.Password);
            var user = await _userService.Authenticate("user1", "password1");
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(user);
        }

        

    }
}
