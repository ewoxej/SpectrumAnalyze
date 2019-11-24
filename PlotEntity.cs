using System;
using System.Linq;
using System.Runtime.Serialization;

namespace SpectrumAnalyzer
{
    [DataContract]
    public class PlotEntity
    {
        public void calculatePeakFrequency()
        {
            PeakFrequency = BuildData.Max();
        }
        [DataMember]
        public double[] BuildData { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double Duration { get; set; }
        [DataMember]
        public DateTime CreationDate { get; set; }
        [DataMember]
        public string AudioFilePath { get; set; }
        [DataMember]
        public double PeakFrequency { get; set; }
    }
}
