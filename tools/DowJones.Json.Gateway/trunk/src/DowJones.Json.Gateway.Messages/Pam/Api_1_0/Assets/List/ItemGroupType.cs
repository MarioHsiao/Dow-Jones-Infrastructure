using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ItemGroupType", Namespace = "")]
    public enum ItemGroupType
    {
        [EnumMember(Value = "Default")] Default = 0,

        [EnumMember(Value = "Or")] Or = 1,

        [EnumMember(Value = "And")] And = 2,

        [EnumMember(Value = "Not")] Not = 3,
    }
}