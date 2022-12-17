﻿using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlertProcessing
{
    public static class ConfigurationExtensions
    {
        public static ServiceCollection AddModuleClient(this ServiceCollection serviceCollection, ITransportSettings transportSettings)
        {
            serviceCollection.AddSingleton<IModuleClient>((sp) => {
                ITransportSettings[] settings = { transportSettings };

                var ioTHubModuleClient = ModuleClient.CreateFromEnvironmentAsync(settings).GetAwaiter().GetResult();
                return new ModuleClientAdapter(ioTHubModuleClient);
            });

            return serviceCollection;
        }
    }
}
