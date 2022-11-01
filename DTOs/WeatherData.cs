using System;

namespace WeatherAssignment.DTOs
{
    public class WeatherData
    {
        public double CelsiusSMHI { get; set; }
        public double CelsiusOpenWeather {get; set;}
        public DateTimeOffset Timestamp {get; set;}
        
    }

}