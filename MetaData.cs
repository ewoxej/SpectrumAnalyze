using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumAnalyzer
{
    [DataContract]
    class MetaData
    {
        [DataMember]
        public String Duration { get; set; }
        [DataMember]
        public DateTime DateOfCreation { get; set; }
        [DataMember]
        public String originName { get; set; }
    }
}
