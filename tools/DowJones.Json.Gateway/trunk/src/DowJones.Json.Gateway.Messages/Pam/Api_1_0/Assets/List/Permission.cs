using System.Runtime.Serialization;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "Permission", Namespace = "")]
    public class Permission
    {
        [DataMember(Name = "groups", IsRequired = true, EmitDefaultValue = false)]
        public GroupList Groups { get; set; }

        [DataMember(Name = "scope", IsRequired = true)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ShareScope Scope { get; set; }

        [DataMember(Name = "shareRoleCollection", IsRequired = true, EmitDefaultValue = false)]
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public ShareRoleCollection ShareRoleCollection { get; set; }
    }
}