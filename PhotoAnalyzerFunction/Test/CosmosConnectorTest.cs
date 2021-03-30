using NUnit.Framework;
using PhotoAnalyzerFunction;
using System.Threading.Tasks;

namespace Test
{
    public class CosmosConnectorTest
    {
        [SetUp]
        public void Setup()
        {
            CosmosConnector.Start();
        }

        [Test]
        public async Task AddItemToContainerAsync()
        {
            var analyzeResult = new AnalyzeResult { PhotoId = "123456789" };

            var result = await CosmosConnector.AddItemToContainerAsync(analyzeResult);

            Assert.IsNotNull(result);
        }
    }
}