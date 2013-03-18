using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace DowJones.Pages.Charting
{
    [GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
    [DataContract(Name = "RegionType", Namespace = "http://schemas.datacontract.org/2004/07/Thunderball.Protocol")]
    public enum RegionType
    {
        [EnumMember]
        Premarket = 0,

        [EnumMember]
        Postmarket = 1,

        [EnumMember]
        Intersession = 2,

        [EnumMember]
        EndOfDay = 3,

        [EnumMember]
        Market = 4,
    }
}