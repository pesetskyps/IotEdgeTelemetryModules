namespace AlertProcessing
{
    public class AlertDto
    {
        public bool IsViolated { get; set; }
        public double SensorValue { get; set; }
        public string SensorName { get; set; }
    }
}