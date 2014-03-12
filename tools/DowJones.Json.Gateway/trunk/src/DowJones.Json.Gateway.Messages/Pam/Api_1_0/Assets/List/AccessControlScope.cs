using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "AccessControlScope", Namespace = "")]
    public enum AccessControlScope
    {
        [EnumMember] Personal = 0,

        [EnumMember] Account = 1,

        [EnumMember] AccountAdmin = 2,

        [EnumMember] Everyone = 3,

        [EnumMember] PreviousScope = 4,
    }
}