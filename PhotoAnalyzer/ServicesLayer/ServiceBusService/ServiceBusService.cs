using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServicesLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesLayer.ServiceBusService
{
    public class ServiceBusService : IServiceBusService
    {
        private readonly IConfiguration _configuration;

        public ServiceBusService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendMessage(AnalyzeResult analyzeResult)
        {
            try
            {
                var connectionString = _configuration.GetValue<string>("serviceBusConnection");
                var queueName = _configuration.GetValue<string>("queueName");

                await using (ServiceBusClient client = new ServiceBusClient(connectionString))
                {
                    ServiceBusSender sender = client.CreateSender(queueName);

                    analyzeResult.PhotoId = Guid.NewGuid().ToString().Split("-")[0];

                    string messageBody = JsonConvert.SerializeObject(analyzeResult);

                    ServiceBusMessage message = new ServiceBusMessage(messageBody);

                    await sender.SendMessageAsync(message);
                }

                return true;
            }
            catch (System.Exception)
            {

                return false;
            }

        }
    }
}
