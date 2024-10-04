using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Text;
using DrawWithAI.DrawApi.Models;

namespace DrawWithAI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            string relativePath = @"..\Images\";
            string testFilePath = @"..\Images\test.txt";
            if (!Directory.Exists(relativePath))
            {
                Directory.CreateDirectory(relativePath);
            }

            // Check if test.txt exists, and create it if it doesn't
            if (!System.IO.File.Exists(relativePath))
            {
                System.IO.File.WriteAllText(testFilePath, "This is a test file.");
                Console.WriteLine("test.txt created.");
            }
            else
            {
                Console.WriteLine("test.txt already exists.");
            }

            using (StreamWriter sw = System.IO.File.CreateText(testFilePath))
            {
                sw.WriteLine("Hello Worewrewrwrwrewrewrerwerwerwerewrewrewrwerrwrwerwewwrwerwerwerwerwerewrwerld");
            }

            // Read and display the content of test.txt
            string content = System.IO.File.ReadAllText(testFilePath);
            Console.WriteLine("Content of test.txt: " + content);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }

        [HttpPost]
        public IActionResult Post([FromBody] RequestToAI request)
        {


            AIResponse response = new AIResponse()
            {
                NamePath = "hello.txt",
                Status = "Success",
                Message = "Image Processed Successfully"
            };
            // Return the processed image
            return Ok(response);
        }
    }
}
