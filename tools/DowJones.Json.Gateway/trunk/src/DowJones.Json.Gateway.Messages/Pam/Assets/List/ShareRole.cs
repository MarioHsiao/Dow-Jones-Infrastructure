using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "ShareRole", Namespace = "")]
    public enum ShareRole
    {
        [EnumMember] Admin = 0,
    }
}