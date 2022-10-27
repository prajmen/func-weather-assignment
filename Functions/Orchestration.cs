using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using WeatherAssignment.Functions;
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
            //var outputs = new List<double>();

            var weatherData = new WeatherData();
            weatherData.Timestamp = DateTimeOffset.UtcNow;
            weatherData.CelsiusSMHI = await context.CallActivityAsync<double>(nameof(ActivityTriggerSMHI.GetWeatherDataSMHI), "Tokyo");
            weatherData.CelsiusYR = await context.CallActivityAsync<double>(nameof(ActivityTriggerSMHI.GetWeatherDataSMHI), "Tokyo");
            
            await context.CallActivityAsync<WeatherData>(nameof(ActivityTriggerQueue.SaveToQueue), weatherData);
            // Replace "hello" with the name of your Durable Activity Function.
            //outputs.Add(await context.CallActivityAsync<double>(nameof(ActivityTriggerSMHI.GetWeatherDataSMHI), "Tokyo"));
            //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
            //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            //return weatherData;
        }

        [FunctionName(nameof(SayHello))]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
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