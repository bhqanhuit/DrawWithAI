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
using System.Security.Claims;

namespace DrawApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SketchesController : ControllerBase
    {
        private readonly ISketchService _sketchService;

        public SketchesController(ISketchService sketchService)
        {
            _sketchService = sketchService;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSketches()
        {
            var sketches = await _sketchService.GetAllSketches();
            return Ok(sketches);
        }

        [Authorize]
        [HttpGet("user-sketches")]
        public async Task<IActionResult> GetUserSketches()
        {
            if (User.Identity is ClaimsIdentity identity)
            {
                var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                string username = userIdClaim.Value;
                Console.WriteLine(username);

                // Fetch sketches for the specific user ID
                var sketches = await _sketchService.GetSketchesByUsername(username);


                var sketchResponses = new List<SketchResponse>();
                foreach (var sketch in sketches)
                {
                    var imagePath = Path.Combine(@"../Images/", sketch.SketchName);
                    if (System.IO.File.Exists(imagePath))
                    {
                        var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
                        sketchResponses.Add(new SketchResponse
                        {
                            SketchName = sketch.SketchName,
                            Prompt = sketch.Prompt,
                            ImageBytes = imageBytes
                        });
                    }
                }

                return Ok(sketchResponses);
            }

            return Unauthorized("Invalid user identity.");
        }


    }
    public class SketchResponse
    {
        public string SketchName { get; set; }
        public string? Prompt { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}
