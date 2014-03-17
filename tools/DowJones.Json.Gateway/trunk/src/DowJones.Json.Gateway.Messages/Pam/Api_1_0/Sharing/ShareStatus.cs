using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing
{
    [DataContract(Name = "ShareStatus", Namespace = "")]
    public enum ShareStatus
    {
        [EnumMember] Active = 0,

        [EnumMember] Inactive = 1,
    }
}