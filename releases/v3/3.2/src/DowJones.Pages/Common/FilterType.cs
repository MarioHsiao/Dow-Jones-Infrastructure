using System.Runtime.Serialization;

namespace DowJones.Pages.Common
{
    [DataContract(Name = "filterType", Namespace = "")]
    public enum FilterType
    {
        [EnumMember]
        Industry,
        [EnumMember]
        Region,
        [EnumMember]
        Keyword,
        [EnumMember]
        Topic,
        [EnumMember]
        Company,
    }
}
