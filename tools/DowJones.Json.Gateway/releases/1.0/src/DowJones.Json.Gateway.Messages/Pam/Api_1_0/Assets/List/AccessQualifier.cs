using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "AccessQualifier", Namespace = "")]
    public enum AccessQualifier
    {
        [EnumMember(Value = "User")] User = 0,

        [EnumMember(Value = "Account")] Account = 1,

        [EnumMember(Value = "Factiva")] Factiva = 2,
    }
}