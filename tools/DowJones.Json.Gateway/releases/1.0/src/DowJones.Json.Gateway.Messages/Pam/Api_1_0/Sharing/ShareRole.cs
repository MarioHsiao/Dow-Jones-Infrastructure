using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing
{
    [DataContract(Name = "ShareRole", Namespace = "")]
    public enum ShareRole
    {
        [EnumMember(Value = "Admin")] Admin = 0,
    }
}