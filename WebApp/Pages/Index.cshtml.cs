using CancelDemoBlazor.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _httpClient;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            this._httpClient = clientFactory.CreateClient();
        }

        private Task<IEnumerable<WeatherForecast>?>? _requestTask;
        public async Task<IActionResult> OnGet()
        {
            string uri = "http://localhost:5031/weatherforecast";
            _requestTask = _httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>(uri);

            IEnumerable<WeatherForecast>? forecasts = await _requestTask;
            _requestTask = null;

            ViewData["WeatherForecastData"] = forecasts;

            return Page();
        }

        public async Task<IActionResult> OnPostRefresh()
        {
            string uri = "http://localhost:5031/weatherforecast";
            _requestTask = _httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>(uri);

            IEnumerable<WeatherForecast>? forecasts = await _requestTask;
            _requestTask = null;

            ViewData["WeatherForecastData"] = forecasts;

            return Page();
        }

        public void OnPostCancel ()
        {
            //cts.Cancel();
            _httpClient.CancelPendingRequests();
            ViewData["WeatherForecastData"] = new List<WeatherForecast>();
        }
    }
}