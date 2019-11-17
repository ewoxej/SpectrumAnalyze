using System;
using System.Runtime.Serialization;

namespace SpectrumAnalyzer
{
    [DataContract]
    public class PlotEntity
    {
        [DataMember]
        public double[] BuildData { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double Duration { get; set; }
        [DataMember]
        public DateTime CreationDate { get; set; }
        public string AudioFilePath { get; set; }
    }
}
