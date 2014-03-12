using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "GroupListType", Namespace = "")]
    public enum GroupListType
    {
        [EnumMember] Group = 0,

        [EnumMember] User = 1,
    }
}