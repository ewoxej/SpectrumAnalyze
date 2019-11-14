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
        String Duration { get; set; }
        [DataMember]
        DateTime DateOfCreation { get; set; }
        [DataMember]
        String originName { get; set; }
    }
}
