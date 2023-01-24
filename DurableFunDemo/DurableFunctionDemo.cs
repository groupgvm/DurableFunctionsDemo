using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableFunDemo
{
    public static class DurableFunctionDemo
    {
        /// <summary>
        /// Ochestation function for invoking order processing
        /// </summary>
        [FunctionName("InvokeOrderProcessing")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            outputs.Add(await context.CallActivityAsync<string>("CheckInventory", "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>("ProcessPayment", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>("CreateShipment", "London"));

            return outputs;
        }

        /// <summary>
        /// Activity function for inventory checking
        /// </summary>
        [FunctionName("CheckInventory")]
        public static string CheckInventory([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Checking inventory is done!";
        }

        /// <summary>
        /// Activity function for processing payments
        /// </summary>
        [FunctionName("ProcessPayment")]
        public static string ProcessPayment([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Processing Payment is done!";
        }

        /// <summary>
        /// Activity function for creating a shipment
        /// </summary>
        [FunctionName("CreateShipment")]
        public static string CreateShipment([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Creating shipment is done!";
        }

        /// <summary>
        /// Client function for process order management
        /// </summary>
        [FunctionName("ProcessOrderManagement")]
        public static async Task<HttpResponseMessage> ProcessOrderManagement(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("InvokeOrderProcessing", null);

            log.LogInformation($"Started order processing with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}