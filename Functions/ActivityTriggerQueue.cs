using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

using Microsoft.Extensions.Logging;
using System.Text.Json;
using WeatherAssignment.DTOs;

namespace WeatherAssignment.Functions
{
    public static class ActivityTriggerQueue
    {
        //För best practice, borde httpClient injectas
        //https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection

        [FunctionName(nameof(SaveToQueue))]
        [return: Queue("temperature-reports", Connection = "AzureWebJobsStorage")]
        public static WeatherData SaveToQueue([ActivityTrigger] WeatherData input, ILogger log)
        {
            log.LogInformation($"Processing queue function: {input.Timestamp.ToString()}");

            log.LogInformation($"Väderdata från SMHI inne i ActivityTrigger: {input.CelsiusSMHI} från YR: {input.CelsiusYR}");

            return input;
        }
    }
}