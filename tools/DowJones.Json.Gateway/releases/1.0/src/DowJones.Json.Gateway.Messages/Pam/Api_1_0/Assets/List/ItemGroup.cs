using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "ItemGroup", Namespace = "")]
    public class ItemGroup
    {
        [DataMember(Name = "id", IsRequired = true)]
        public long Id { get; set; }

        [DataMember(Name = "groupType", IsRequired = true, EmitDefaultValue = true)]
        public ItemGroupType GroupType { get; set; }

        [DataMember(Name = "itemCollection", IsRequired = true, EmitDefaultValue = true)]
        public ItemCollection ItemCollection { get; set; }
    }
}