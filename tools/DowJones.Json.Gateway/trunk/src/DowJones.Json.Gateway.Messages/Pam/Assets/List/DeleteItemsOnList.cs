using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [DataContract(Name = "DeleteItemsOnList", Namespace = "")]
    public class DeleteItemsOnList
    {
        [DataMember(Name = "listId", IsRequired = true)]
        public long ListId { get; set; }

        [DataMember(Name = "itemIdCollection", IsRequired = true, EmitDefaultValue = false, Order = 1)]
        public ItemIdCollection ItemIdCollection { get; set; }
    }
}