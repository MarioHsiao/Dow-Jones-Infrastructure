using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "GroupIdList", Namespace = "")]
    public class GroupIdList
    {
        [DataMember(Name = "idCollection", IsRequired = true, EmitDefaultValue = false)]
        public IdCollection IdCollection { get; set; }

        [DataMember(Name = "type", IsRequired = true)]
        [JsonConverter(typeof(StringEnumConverter))]
        public GroupListType Type { get; set; }
    }
}