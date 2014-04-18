using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing
{
    [DataContract(Name = "ShareStatus", Namespace = "")]
    public enum ShareStatus
    {
        [EnumMember(Value = "Active")] Active = 0,

        [EnumMember(Value = "Inactive")] Inactive = 1,
    }
}