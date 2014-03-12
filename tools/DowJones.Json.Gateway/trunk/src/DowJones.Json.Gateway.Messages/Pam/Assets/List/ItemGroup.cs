using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "ItemGroup", Namespace = "")]
    public class ItemGroup
    {
        [DataMember(Name = "id", IsRequired = true)]
        public long Id { get; set; }

        [DataMember(Name = "groupType", IsRequired = true, Order = 1)]
        public ItemGroupType GroupType { get; set; }

        [DataMember(Name = "itemCollection", IsRequired = true, EmitDefaultValue = false, Order = 2)]
        public ItemCollection itemCollection { get; set; }
    }
}