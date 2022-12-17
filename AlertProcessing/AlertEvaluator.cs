using System;
using System.Collections.Generic;
using System.Text;

namespace AlertProcessing
{
    internal class AlertEvaluator : IAlertEvaluator
    {
        public AlertDto Evaluate(SensorReading sensorReading)
        {
            //logic
            if (sensorReading.machine.temperature < 0)
            {
                return new AlertDto { IsViolated = true, SensorName = nameof(sensorReading.machine.temperature), SensorValue = sensorReading.machine.temperature };
            }

            return new AlertDto { IsViolated = false, SensorName = nameof(sensorReading.machine.temperature), SensorValue = sensorReading.machine.temperature };
        }
    }
}
