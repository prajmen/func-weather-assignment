using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using WeatherAssignment.DTOs;
using System;

namespace WeatherAssignment.Functions
{
    public static class QueueTrigger
    {
        [FunctionName("QueueTrigger")]
        [return: Table("Temperatures", Connection = "AzureWebJobsStorage")]
        public static TableData Run(
        [QueueTrigger("temperature-reports", Connection = "AzureWebJobsStorage")] WeatherData data,
        ILogger log)
        {
            if (data == null)
            {
                log.LogError($"Temperature null");
                throw new InvalidOperationException();
            }

            log.LogInformation("Saving to db");

            return new TableData(
                $"{(DateTimeOffset.MaxValue.Ticks - data.Timestamp.Ticks):d10}-{Guid.NewGuid():N}",
                data);

        }
    }
}

