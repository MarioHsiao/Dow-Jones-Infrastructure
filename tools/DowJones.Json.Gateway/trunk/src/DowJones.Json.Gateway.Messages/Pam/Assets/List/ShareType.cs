using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "ShareType", Namespace = "")]
    public enum ShareType
    {
        [EnumMember] Personal = 0,

        [EnumMember] Subscribed = 1,

        [EnumMember] Assigned = 2,

        [EnumMember] All = 3,
    }
}