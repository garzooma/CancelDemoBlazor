using CancelDemoBlazor.Shared;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CancelDemoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppSettings _settings;
        private readonly HttpClient _httpClient;
        public MainWindow(IOptions<AppSettings> settings, IHttpClientFactory httpClientFactory)
        {
            _settings = settings.Value;
            InitializeComponent();
            _httpClient = httpClientFactory.CreateClient();
            ViewModel viewModel = new ViewModel();
            DataContext = viewModel;
        }

        private Task<IEnumerable<WeatherForecast>?>? _requestTask;
        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            string uri = "http://localhost:5031/weatherforecast";
            _requestTask = _httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>(uri);

            IEnumerable<WeatherForecast>? forecasts;
            try { 
                forecasts = await _requestTask;
            } 
            catch (Exception excp)
            {
                Debug.WriteLine("Refresh Cancelled");
                return;
            }
            ViewModel viewModel = (ViewModel)DataContext;
            foreach (WeatherForecast wf in forecasts) { 
                viewModel.WeatherForecasts.Add(wf);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _httpClient.CancelPendingRequests();
            ViewModel viewModel = (ViewModel)DataContext;
            viewModel.WeatherForecasts.Clear();
        }
    }
}
