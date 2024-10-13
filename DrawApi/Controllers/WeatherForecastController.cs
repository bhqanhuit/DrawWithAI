using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Text;
using DrawWithAI.DrawApi.Models;
using DrawWithAI.DrawApi.Services;
using System.Net;
using DrawApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DrawWithAI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        //private static readonly string[] Summaries = new[]
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        //private readonly ILogger<WeatherForecastController> _logger;

        //public WeatherForecastController(ILogger<WeatherForecastController> logger)
        //{
        //    _logger = logger;
        //}
        
        private readonly DataContext _context;

        public WeatherForecastController(DataContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {

            var users = await _context.SketchToImageView.ToListAsync();
            return Ok(users);

            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //    {
            //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            //        TemperatureC = Random.Shared.Next(-20, 55),
            //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //    })
            //    .ToArray();
        }

        [HttpPost]
        public IActionResult Post([FromBody] RequestToAI request)
        {


            AIResponse response = new AIResponse()
            {
                NamePath = "hello.txt"
            };
            // Return the processed image
            return Ok(response);
        }
    }
}
