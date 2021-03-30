using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace PhotoAnalyzerFunction
{
    public class CosmosConnector
    {
        private static readonly string EndpointUri = "https://photoanalyzerdb.documents.azure.com:443/";

        private static readonly string PrimaryKey = "4S25GAfuIvcdKSODqYqhk5GToHS66vDO2M75Kqr0G0pXfz0CUORM7lCpvNyaeBlMI0nCNoob6gC7YQV5u56ALw==";

        private static CosmosClient cosmosClient;

        private static Database database;

        private static Container container;

        private static string databaseId = "PhotoAnalyze";
        private static string containerId = "PhotoAnalyzeContainer";

        static CosmosConnector()
        {
            cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);

            CreateDatabaseAsync().GetAwaiter().GetResult();

            CreateContainerAsync().GetAwaiter().GetResult();
        }

        public static void Start()
        {
        }

        public static bool TestConnection() => database != null && container != null;

        private static async Task CreateDatabaseAsync()
        {
            database = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId).GetAwaiter().GetResult();
        }

        private static async Task CreateContainerAsync()
        {
            container = database.CreateContainerIfNotExistsAsync(containerId, "/PhotoId").GetAwaiter().GetResult();
        }

        public static async Task<ItemResponse<AnalyzeResult>> AddItemToContainerAsync(AnalyzeResult analyzeResult)
        {
            analyzeResult.id = Guid.NewGuid().ToString();
            var result = await container.CreateItemAsync<AnalyzeResult>(analyzeResult, new PartitionKey(analyzeResult.PhotoId));

            return result;
        }
    }
}
