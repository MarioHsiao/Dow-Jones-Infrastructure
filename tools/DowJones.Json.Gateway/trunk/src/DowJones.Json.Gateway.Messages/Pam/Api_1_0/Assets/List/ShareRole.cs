using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ShareRole", Namespace = "")]
    public enum ShareRole
    {
        [EnumMember] Admin = 0,
    }
}