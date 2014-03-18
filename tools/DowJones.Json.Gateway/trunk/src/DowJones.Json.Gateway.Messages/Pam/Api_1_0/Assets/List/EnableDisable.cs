using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "EnableDisable", Namespace = "")]
    public enum EnableDisable
    {
        [EnumMember(Value = "Enable")] Enable = 0,

        [EnumMember(Value = "Disable")] Disable = 1,
    }
}