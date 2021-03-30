using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PhotoAnalyzerFunction
{
    public static class PhotoAnalyzerFunc
    {
        [FunctionName("PhotoAnalyzerFunc")]
        public static async Task Run([ServiceBusTrigger("result-queue", Connection = "ServiceBusConnectionString")]string analyzeResultString, ILogger log)
        {
            var analyzeResult = JsonConvert.DeserializeObject<AnalyzeResult>(analyzeResultString);
            await CosmosConnector.AddItemToContainerAsync(analyzeResult);
        }
    }
}
