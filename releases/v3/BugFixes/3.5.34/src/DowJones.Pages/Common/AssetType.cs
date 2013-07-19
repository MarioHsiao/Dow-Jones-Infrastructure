using System.Runtime.Serialization;

namespace DowJones.Pages.Common
{
    [DataContract(Name = "assetType", Namespace = "")]
    public enum AssetType
    {
        [EnumMember]
        Nothing,
        [EnumMember]
        Page,
        [EnumMember]
        Module,
        [EnumMember]
        Topic
    }
}
