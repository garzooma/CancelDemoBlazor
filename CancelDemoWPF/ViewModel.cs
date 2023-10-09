using CancelDemoBlazor.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancelDemoWPF
{
    public class ViewModel
    {
        public ObservableCollection<WeatherForecast> WeatherForecasts { get; set; } = new ObservableCollection<WeatherForecast>();
    }
}
