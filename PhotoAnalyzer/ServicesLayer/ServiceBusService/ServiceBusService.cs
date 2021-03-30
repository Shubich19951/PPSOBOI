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
        private string _connectionString;
        private string _queueName;

        public ServiceBusService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetValue<string>("serviceBusConnection");
            _queueName = _configuration.GetValue<string>("queueName");
        }

        public async Task<bool> SendMessage(AnalyzeResult analyzeResult)
        {
            try
            {
                await using (ServiceBusClient client = new ServiceBusClient(_connectionString))
                {
                    ServiceBusSender sender = client.CreateSender(_queueName);

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
