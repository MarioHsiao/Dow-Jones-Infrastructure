using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [ServicePath("pamapi/1.0/List.svc")]
    [DataContract(Name = "DeleteItemsOnList", Namespace = "")]
    public class DeleteItemsOnList
    {
        [DataMember(Name = "listId", IsRequired = true)]
        public long ListId { get; set; }

        [DataMember(Name = "itemIdCollection", IsRequired = true, EmitDefaultValue = false, Order = 1)]
        public ItemIdCollection ItemIdCollection { get; set; }
    }
}