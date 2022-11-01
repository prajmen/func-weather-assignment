using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WeatherAssignment.DTOs;
using System.Net.Http.Headers;

namespace WeatherAssignment.Functions
{
    public static class ActivityTriggerYR
    {
        //För best practice, borde httpClient injectas
        //https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
        private static HttpClient httpClient = new HttpClient();

        [FunctionName(nameof(GetWeatherDataYR))]
        public static async Task<double> GetWeatherDataYR([ActivityTrigger] string name, ILogger log)
        {
            var url = "https://api.met.no/weatherapi/locationforecast/2.0/compact?lat=60&lon=11";

            var request = new HttpRequestMessage(HttpMethod.Get, url); 

            var header = new ProductHeaderValue("func-weather-assignment.azurewebsites.net");
            var userAgent = new ProductInfoHeaderValue(header);

            request.Headers.UserAgent.Add(userAgent);

            var response = await httpClient.GetAsync(url);
            log.LogInformation(response.ToString());

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var data = JsonSerializer.Deserialize<Root>(json);

                log.LogInformation($"weather from YR: {data.properties.timeseries[0].data.instant.details.air_temperature.ToString()}");

            }
            else
            {
                log.LogInformation(response.StatusCode.ToString());
            }
            
            log.LogInformation($"Saying hello to {name}.");
            return 18.8;
        }
    }
}