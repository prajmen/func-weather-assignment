// using System.Net.Http;
// using System.Threading.Tasks;
// using Microsoft.Azure.WebJobs;
// using Microsoft.Azure.WebJobs.Extensions.DurableTask;
// using Microsoft.Extensions.Logging;
// using System.Text.Json;
// using WeatherAssignment.DTOs;
// using System.Net.Http.Headers;

// namespace WeatherAssignment.Functions
// {
//     public static class ActivityTriggerYR
//     {
//         //För best practice, borde httpClient injectas
//         //https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
//         private static HttpClient httpClient = new HttpClient();

//         [FunctionName(nameof(GetWeatherDataYR))]
//         public static async Task<double> GetWeatherDataYR([ActivityTrigger] Coordinate coordinate, ILogger log)
//         {
//             var latitude = coordinate.Latitude.ToString().Replace(',','.');
//             var longitude = coordinate.Longitude.ToString().Replace(',','.');

//             //var url = $"https://api.met.no/weatherapi/locationforecast/2.0/compact?lat={latitude}&lon={longitude}";
//             var url = "https://api.met.no/weatherapi/locationforecast/2.0/compact?lat=60&lon=11";
//             log.LogInformation($"url yr: {url}");
//             var request = new HttpRequestMessage(HttpMethod.Get, url); 

//             //YR api requires UserAgent-Header
            
//             var header = new ProductHeaderValue("func-weather-assignment.azurewebsites.net");
//             //var header = new ProductHeaderValue("func-weather-assignment.azurewebsites.net");
//             var userAgent = new ProductInfoHeaderValue(header);
//             request.Headers.UserAgent.Add(userAgent);

//             var response = await httpClient.GetAsync(url);

//             if (response.IsSuccessStatusCode)
//             {
//                 var json = response.Content.ReadAsStringAsync().Result;
//                 var data = JsonSerializer.Deserialize<Root>(json);

//                 log.LogInformation($"weather from YR: {data.properties.timeseries[0].data.instant.details.air_temperature.ToString()}");
//             }
//             else
//             {
//                 log.LogInformation(response.StatusCode.ToString());
//                 log.LogInformation(response.ReasonPhrase);
//             }

//             return 18.8;
//         }
//     }
// }