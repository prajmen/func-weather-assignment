// RootSMHI myDeserializedClass = JsonConvert.DeserializeObject<RootSMHI>(myJsonResponse);

using System;
using System.Collections.Generic;

namespace WeatherAssignment.DTOs;

    public class GeometrySMHI
    {
        public string type { get; set; }
        public List<List<double>> coordinates { get; set; }
    }

    public class Parameter
    {
        public string name { get; set; }
        public string levelType { get; set; }
        public int level { get; set; }
        public string unit { get; set; }
        public List<double> values { get; set; }
    }

    public class RootSMHI
    {
        public DateTime approvedTime { get; set; }
        public DateTime referenceTime { get; set; }
        public GeometrySMHI geometrySMHI { get; set; }
        public List<TimeSeries> timeSeries { get; set; }
    }

    public class TimeSeries
    {
        public DateTime validTime { get; set; }
        public List<Parameter> parameters { get; set; }
    }

