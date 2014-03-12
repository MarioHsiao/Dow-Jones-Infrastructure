using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ItemGroupType", Namespace = "")]
    public enum ItemGroupType
    {
        [EnumMember] Default = 0,

        [EnumMember] Or = 1,

        [EnumMember] And = 2,

        [EnumMember] Not = 3,
    }
}