using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [DataContract(Name = "AddItemsToList", Namespace = "")]
    public class AddItemsToList
    {
        [DataMember(Name = "listId", IsRequired = true)]
        public long ListId { get; set; }

        [DataMember(Name = "type", IsRequired = true)]
        public ItemGroupType Type { get; set; }

        [DataMember(Name = "itemCollection", IsRequired = true)]
        public ItemCollection ItemCollection { get; set; }
    }
}