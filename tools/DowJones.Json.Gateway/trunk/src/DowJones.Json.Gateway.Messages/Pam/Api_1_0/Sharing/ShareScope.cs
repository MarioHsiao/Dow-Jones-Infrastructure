using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing
{
    [DataContract(Name = "ShareScope", Namespace = "")]
    public enum ShareScope
    {
        [EnumMember(Value = "Personal")] Personal = 0,

        [EnumMember(Value = "AccountAdmin")] AccountAdmin = 1,

        [EnumMember(Value = "Group")] Group = 2,

        [EnumMember(Value = "Account")] Account = 3,

        [EnumMember(Value = "Everyone")] Everyone = 4,
    }
}