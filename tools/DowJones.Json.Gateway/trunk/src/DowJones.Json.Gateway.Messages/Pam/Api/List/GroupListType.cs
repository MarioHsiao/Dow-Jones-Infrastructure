using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "GroupListType", Namespace = "")]
    public enum GroupListType
    {

        [EnumMember]
        Group = 0,

        [EnumMember]
        User = 1,
    }
}