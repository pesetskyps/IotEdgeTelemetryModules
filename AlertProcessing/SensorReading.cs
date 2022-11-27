using System;
using System.Collections.Generic;
using System.Text;

namespace AlertProcessing
{
    internal class SensorReading
    {
        public Machine machine { get; set; }
        public Ambient ambient { get; set; }
        public DateTime timeCreated { get; set; }
    }

    public class Machine
    {
        public double temperature { get; set; }
        public double pressure { get; set; }

    }
    public class Ambient
    {
        public double temperature { get; set; }
        public int humidity { get; set; }

    }
}
