using CancelDemoBlazor.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CancelDemoBlazor.Server.Controllers
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

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get(CancellationToken cancellationToken)
        {
            bool cancel = false;
            cancellationToken.Register(() => {
                cancel = true;
            });

            int count = 5;
            List<WeatherForecast> ret = new List<WeatherForecast>();
            for (int i = 1; i <= count; i++)
            {
                Debug.WriteLine($"Iter #{i}");
                if (cancel) // (cancellationToken.IsCancellationRequested)
                {
                    Debug.WriteLine("Cancelled");
                    throw new Exception($"Canceled after {i} forecast items");
                }
                await Task.Delay(1000);
                WeatherForecast wf = new WeatherForecast()
                {
                    Date = DateTime.Now.AddDays(i),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                };
                ret.Add(wf);
            }

            return ret;
        }
    }
}