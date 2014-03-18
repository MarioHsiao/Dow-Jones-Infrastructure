using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing
{
    [DataContract(Name = "ShareType", Namespace = "")]
    public enum ShareType
    {
        [EnumMember(Value = "Personal")] Personal = 0,

        [EnumMember(Value = "Subscribed")] Subscribed = 1,

        [EnumMember(Value = "Assigned")] Assigned = 2,

        [EnumMember(Value = "All")] All = 3,
    }
}