using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using WeatherAssignment;


namespace WeatherAssignment
{
    public static class Orchestration
    {
        [FunctionName("Orchestration")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>(nameof(ActivityTriggerSMHI.GetWeatherDataSMHI), "Tokyo"));
            //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
            //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
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