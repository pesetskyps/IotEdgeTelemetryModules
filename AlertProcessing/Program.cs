using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;


namespace AlertProcessing
{
    internal class Program
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Build the our IServiceProvider and set our static reference to it
            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Initialize module
            ServiceProvider.GetRequiredService<AlertProcessingModule>()
                .InitializeAsync()
                .GetAwaiter()
                .GetResult();

            // Wait until the app unloads or is cancelled
            var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();
            WhenCancelled(cts.Token).Wait();
        }
        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddModuleClient(new MqttTransportSettings(TransportType.Mqtt_Tcp_Only));
            serviceCollection.AddSingleton<AlertProcessingModule>();
            serviceCollection.AddSingleton<IAlertEvaluator,AlertEvaluator>();
        }

        /// <summary>
        /// Handles cleanup operations when app is cancelled or unloads
        /// </summary>
        public static Task WhenCancelled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }
    }
}
