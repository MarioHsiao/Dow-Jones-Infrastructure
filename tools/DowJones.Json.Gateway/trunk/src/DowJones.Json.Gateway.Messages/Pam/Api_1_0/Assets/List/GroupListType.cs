using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "GroupListType", Namespace = "")]
    public enum GroupListType
    {
        [EnumMember(Value = "Group")] Group = 0,

        [EnumMember(Value = "User")] User = 1,
    }
}