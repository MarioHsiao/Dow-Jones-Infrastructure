using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace DowJones.Pages.Charting
{
    [GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
    [DataContract(Name = "SessionUnit", Namespace = "http://schemas.datacontract.org/2004/07/Thunderball.Protocol")]
    public enum SessionUnit
    {
        [EnumMember]
        Minute = 0,

        [EnumMember]
        Hour = 1,

        [EnumMember]
        Day = 2,

        [EnumMember]
        Month = 3,

        [EnumMember]
        Quarter = 4,

        [EnumMember]
        Year = 5,
    }
}