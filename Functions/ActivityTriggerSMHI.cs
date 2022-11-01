using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Newtonsoft.Json;
using WeatherAssignment.DTOs;
using System;

namespace WeatherAssignment.Functions
{
    public static class ActivityTriggerSMHI 
    {
        //För best practice, borde httpClient injectas
        //https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
        private static HttpClient httpClient = new HttpClient();

        [FunctionName(nameof(GetWeatherDataSMHI))]
        public static async Task<double> GetWeatherDataSMHI([ActivityTrigger] Coordinate coordinate, ILogger log)
        {
            var latitude = coordinate.Latitude.ToString().Replace(',','.');
            var longitude = coordinate.Longitude.ToString().Replace(',','.');

            var url = $"https://opendata-download-metfcst.smhi.se/api/category/pmp3g/version/2/geotype/point/lon/{longitude}/lat/{latitude}/data.json";

            log.LogInformation(url);

            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;

                RootSMHI data = JsonConvert.DeserializeObject<RootSMHI>(json);

                var forecastTime = data.timeSeries[0].validTime;
                var currentWeatherParameterName = data.timeSeries[0].parameters[10].name;
                var currentAirTempCelsius = data.timeSeries[0].parameters[10].values[0];

                log.LogInformation($" Weather from SMHI. Forecast for Nackademin at: {forecastTime}. Current air temp is: {currentAirTempCelsius}");

                return currentAirTempCelsius;
            }
            else
            {
                var statusCode = response.StatusCode.ToString();
                log.LogInformation(statusCode);

                throw new InvalidOperationException($"SMHI response status code do not indicate success. Status code {statusCode}");
            }             
        }
    }
}