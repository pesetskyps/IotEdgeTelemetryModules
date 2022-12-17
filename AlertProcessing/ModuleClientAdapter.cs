using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using System.Threading.Tasks;
using System;

namespace AlertProcessing
{
    public class ModuleClientAdapter : IModuleClient
    {
        private readonly ModuleClient moduleClient;
        public ModuleClientAdapter(ModuleClient moduleClient)
        {
            this.moduleClient = moduleClient ?? throw new System.ArgumentNullException(nameof(moduleClient));
        }

        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.moduleClient.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<Twin> GetTwinAsync() => this.moduleClient.GetTwinAsync();
        public Task OpenAsync() => this.moduleClient.OpenAsync();

        public Task SetInputMessageHandlerAsync(string inputName, MessageHandler messageHandler, object userContext) => this.moduleClient.SetInputMessageHandlerAsync(inputName, messageHandler, userContext);

        public Task SendEventAsync(string outputName, Message message) => this.moduleClient.SendEventAsync(outputName, message);
    }
}
