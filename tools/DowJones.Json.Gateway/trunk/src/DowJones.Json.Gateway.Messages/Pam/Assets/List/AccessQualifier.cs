using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "AccessQualifier", Namespace = "")]
    public enum AccessQualifier
    {
        [EnumMember] User = 0,

        [EnumMember] Account = 1,

        [EnumMember] Factiva = 2,
    }
}