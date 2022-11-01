using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WeatherAssignment.DTOs;
using System;

namespace WeatherAssignment.Functions
{
    public class ActivityTriggerOpenWeather
    {
        //För best practice, borde httpClient injectas
        //https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
        private readonly HttpClient _client;

        public ActivityTriggerOpenWeather(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        [FunctionName(nameof(GetWeatherDataOpenWeather))]
        public async Task<double> GetWeatherDataOpenWeather([ActivityTrigger] Coordinate coordinate, ILogger log)
        {
            var latitude = coordinate.Latitude.ToString().Replace(',','.');
            var longitude = coordinate.Longitude.ToString().Replace(',','.');

            var apiToken = Environment.GetEnvironmentVariable("OpenWeatherApiToken");
            var url = $"https://api.openweathermap.org/data/2.5/onecall?lat={latitude}&lon={longitude}&units=metric&exclude=hourly,daily,minutely,alerts&appid={apiToken}";
            
            var request = new HttpRequestMessage(HttpMethod.Get, url); 
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var data = JsonSerializer.Deserialize<RootOpenWeather>(json);

                log.LogInformation($"Current weather from Open Weather: {data.current.temp.ToString()}");
            
                return data.current.temp;
            }
            else
            {
                var statusCode = response.StatusCode.ToString();
                log.LogInformation(statusCode);

                throw new InvalidOperationException($"Open Weather response status code do not indicate success. Status code {statusCode}");
            }  
        }
    }
}