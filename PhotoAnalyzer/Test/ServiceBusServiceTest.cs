using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using ServicesLayer.Models;
using ServicesLayer.ServiceBusService;
using System.Threading.Tasks;

namespace Test
{
    public class ServiceBusServiceTest
    {
        private readonly string serviceBusConnection = "Endpoint=sb://photo-azalyzer-servicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=tsY1/oRIBALDf+LfFW1VMTnt3E1xnAkXzKUIRT9sTXU=";
        private readonly string queueName = "result-queue";
        private IServiceBusService _serviceBusService;

        [SetUp]
        public void Setup()
        {
            var configuration = new Mock<IConfiguration>();

            var serviceBusConnectionSection = new Mock<IConfigurationSection>();
            serviceBusConnectionSection.Setup(a => a.Value).Returns(serviceBusConnection);

            var queueNameSection = new Mock<IConfigurationSection>();
            queueNameSection.Setup(a => a.Value).Returns(queueName);

            configuration.Setup(a => a.GetSection("serviceBusConnection")).Returns(serviceBusConnectionSection.Object);
            configuration.Setup(a => a.GetSection("queueName")).Returns(queueNameSection.Object);

            _serviceBusService = new ServiceBusService(configuration.Object);
        }

        [Test]
        public async Task SendMessage()
        {
            var analyzeResult = new AnalyzeResult();

            var result = await _serviceBusService.SendMessage(analyzeResult);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task SendMessage_GetException()
        {
            var configuration = new Mock<IConfiguration>();

            var serviceBusConnectionSection = new Mock<IConfigurationSection>();
            serviceBusConnectionSection.Setup(a => a.Value).Returns(string.Empty);

            var queueNameSection = new Mock<IConfigurationSection>();
            queueNameSection.Setup(a => a.Value).Returns(string.Empty);

            configuration.Setup(a => a.GetSection("serviceBusConnection")).Returns(serviceBusConnectionSection.Object);
            configuration.Setup(a => a.GetSection("queueName")).Returns(queueNameSection.Object);

            _serviceBusService = new ServiceBusService(configuration.Object);

            var result = await _serviceBusService.SendMessage(null);
            Assert.IsFalse(result);
        }
    }
}