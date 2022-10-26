using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace WeatherAssignment
{
    public static class ActivityTriggerSMHI 
    {
        //För best practice, borde httpClient injectas
        //https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
        private static HttpClient httpClient = new HttpClient();

        [FunctionName(nameof(GetWeatherDataSMHI))]
        public static async Task<string> GetWeatherDataSMHI([ActivityTrigger] string name, ILogger log)
        {
            var url = "https://opendata-download-metfcst.smhi.se/api/category/pmp3g/version/2/geotype/point/lon/16.158/lat/58.5812/data.json";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
            }
            
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello!";
        }
    }
}