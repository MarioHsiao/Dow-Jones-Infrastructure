using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [ServicePath("pamapi/1.0/List.svc")]
    [DataContract(Name = "AddItemsToList", Namespace = "")]
    public class AddItemsToListRequest : PostJsonRestRequest
    {
        [DataMember(Name = "listId", IsRequired = true)]
        public long ListId { get; set; }

        [DataMember(Name = "type", IsRequired = true)]
        public ItemGroupType Type { get; set; }

        [DataMember(Name = "itemCollection", IsRequired = true)]
        public ItemCollection ItemCollection { get; set; }
    }
}