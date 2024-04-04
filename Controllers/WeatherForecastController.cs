using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecasttestController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "vijitest"
        };

        private readonly ILogger<WeatherForecasttestController> _logger;

        public WeatherForecasttestController(ILogger<WeatherForecasttestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        //[Route("getweather")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
