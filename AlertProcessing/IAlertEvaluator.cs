namespace AlertProcessing
{
    internal interface IAlertEvaluator
    {
        AlertDto Evaluate(SensorReading sensorReading);
    }
}