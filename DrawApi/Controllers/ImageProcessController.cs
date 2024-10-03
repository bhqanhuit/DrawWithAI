using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using DrawWithAI.DrawAPI.Models;

namespace DrawApi.Controllers
{
    // https://localhost:5001/api/ImageProcess 
    [Route("api/[controller]")]
    [ApiController]
    public class ImageProcessController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] ImageProcessRequest request)
        {
            Console.WriteLine("Success");

            // Return the processed image
            return Ok("feferfeeeeeeeeeeeeeeef");
        }

        [HttpGet]
        public string Get()
        {
            Console.WriteLine("HGET");
            return ("GETT SUCCESS");
        }
    }
    
}
