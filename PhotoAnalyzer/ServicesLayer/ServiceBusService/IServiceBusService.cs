using ServicesLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.ServiceBusService
{
    public interface IServiceBusService
    {
        Task<bool> SendMessage(AnalyzeResult analyzeResult);
    }
}
