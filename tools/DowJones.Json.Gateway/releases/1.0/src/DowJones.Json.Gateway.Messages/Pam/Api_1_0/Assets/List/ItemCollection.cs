using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [CollectionDataContract(Name = "ItemCollection", Namespace = "", ItemName = "item")]
    public class ItemCollection : List<Item>
    {
        public ItemCollection()
        {
        }
        
        public ItemCollection(IEnumerable<Item> collection) : base(collection)
        {
        }
    }
}