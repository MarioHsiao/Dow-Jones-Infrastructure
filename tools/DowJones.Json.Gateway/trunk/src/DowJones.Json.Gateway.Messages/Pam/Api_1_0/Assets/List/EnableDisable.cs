using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "EnableDisable", Namespace = "")]
    public enum EnableDisable
    {
        [EnumMember] Enable = 0,

        [EnumMember] Disable = 1,
    }
}