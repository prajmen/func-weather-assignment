using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using WeatherAssignment.DTOs;
using System;

namespace WeatherAssignment.Functions
{
    public static class Orchestration
    {         

        [FunctionName("Orchestration")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var coordinate = new Coordinate(59.345, 18.02);
            var weatherData = new WeatherData();          
            weatherData.Timestamp = context.CurrentUtcDateTime;
            weatherData.CelsiusSMHI = await context.CallActivityAsync<double>(nameof(ActivityTriggerSMHI.GetWeatherDataSMHI), coordinate);
            weatherData.CelsiusOpenWeather = await context.CallActivityAsync<double>(nameof(ActivityTriggerOpenWeather.GetWeatherDataOpenWeather), coordinate);
            
            await context.CallActivityAsync<WeatherData>(nameof(ActivityTriggerQueue.SaveToQueue), weatherData);
        }

        [FunctionName("TimerFunction")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer,
            [DurableClient] IDurableOrchestrationClient starter, ILogger log)
        {            
            string instanceId = await starter.StartNewAsync("Orchestration", null);
            
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        }
    }
}