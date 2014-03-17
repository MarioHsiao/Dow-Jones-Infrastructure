using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing
{
    [DataContract(Name = "ShareScope", Namespace = "")]
    public enum ShareScope
    {
        [EnumMember] Personal = 0,

        [EnumMember] AccountAdmin = 1,

        [EnumMember] Group = 2,

        [EnumMember] Account = 3,

        [EnumMember] Everyone = 4,
    }
}