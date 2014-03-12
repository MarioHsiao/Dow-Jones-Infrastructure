using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "ShareStatus", Namespace = "")]
    public enum ShareStatus
    {
        [EnumMember] Active = 0,

        [EnumMember] Inactive = 1,
    }
}