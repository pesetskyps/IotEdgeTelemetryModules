using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using System.Threading.Tasks;
using System;

namespace AlertProcessing
{
    public interface IModuleClient : IDisposable
    {
        Task<Twin> GetTwinAsync();
        Task OpenAsync();
        Task SetInputMessageHandlerAsync(string inputName, MessageHandler messageHandler, object userContext);
        Task SendEventAsync(string outputName, Message message);
    }
}
