using Microsoft.Azure.Devices.Client;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Threading;
using System.Text.Json;

namespace AlertProcessing
{
    internal class AlertProcessingModule
    {
        private readonly IModuleClient moduleClient;
        private readonly IAlertEvaluator alertEvaluator;
        int counter;

        public AlertProcessingModule(IModuleClient moduleClient, IAlertEvaluator alertEvaluator)
        {
            this.moduleClient = moduleClient;
            this.alertEvaluator = alertEvaluator;
        }

        public async Task InitializeAsync()
        {
            await this.moduleClient.OpenAsync();
            Console.WriteLine("{0} client initialized.", typeof(AlertProcessingModule));

            await this.moduleClient.SetInputMessageHandlerAsync("input1", EvaluateAlert, null);
        }

        async Task<MessageResponse> EvaluateAlert(Message message, object userContext)
        {
            int counterValue = Interlocked.Increment(ref counter);

            var moduleClient = userContext as ModuleClient;

            byte[] messageBytes = message.GetBytes();
            string messageString = Encoding.UTF8.GetString(messageBytes);
            Console.WriteLine($"Received message: {counterValue}, Body: [{messageString}]");

            SensorReading sensorReading = null;
            if (!string.IsNullOrEmpty(messageString))
            {
                try
                {
                    sensorReading = JsonSerializer.Deserialize<SensorReading>(messageString);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error", ex.Message);
                    throw;
                }

                var evaluatorResult = alertEvaluator.Evaluate(sensorReading);

                using (var pipeMessage = new Message(messageBytes))
                {
                    foreach (var prop in message.Properties)
                    {
                        pipeMessage.Properties.Add(prop.Key, prop.Value);
                    }
                    await this.moduleClient.SendEventAsync("output1", pipeMessage);

                    Console.WriteLine("Received message sent");
                }
            }
            return MessageResponse.Completed;
        }
    }
}
