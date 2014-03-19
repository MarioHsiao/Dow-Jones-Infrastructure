using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ItemGroupType", Namespace = "")]
    public enum ItemGroupType
    {
        [JsonProperty("Default")] 
        [EnumMember(Value = "Default")] 
        Default = 0,
        
        [JsonProperty("Or")] 
        [EnumMember(Value = "Or")] 
        Or = 1,
        
        [JsonProperty("And")] 
        [EnumMember(Value = "And")] 
        And = 2,

        [JsonProperty("Not")] 
        [EnumMember(Value = "Not")] 
        Not = 3,
    }
}