using System;
using System.Runtime.Serialization;

namespace SpectrumAnalyzer
{
    [DataContract]
    public class PlotEntity
    {
        private void calculatePeakFrequency()
        {
            PeakFrequency = 10;
        }
        public PlotEntity()
        {
            calculatePeakFrequency();
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
        public int PeakFrequency { get; set; }
    }
}
