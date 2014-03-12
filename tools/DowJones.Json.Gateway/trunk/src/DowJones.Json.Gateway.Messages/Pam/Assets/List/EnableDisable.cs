using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "EnableDisable", Namespace = "")]
    public enum EnableDisable
    {

        [EnumMember()]
        Enable = 0,

        [EnumMember()]
        Disable = 1,
    }
}