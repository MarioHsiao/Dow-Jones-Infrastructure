using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions
{
    [ServicePath("pamapi/1.0/Lists.svc")]
    [DataContract(Name = "DeleteItemsOnList", Namespace = "")]
    public class DeleteItemsOnListRequest : IPostJsonRestRequest
    {
        [DataMember(Name = "listId", IsRequired = true)]
        public long ListId { get; set; }

        [DataMember(Name = "itemIdCollection", IsRequired = true, EmitDefaultValue = false, Order = 1)]
        public ItemIdCollection ItemIdCollection { get; set; }

        public virtual string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}