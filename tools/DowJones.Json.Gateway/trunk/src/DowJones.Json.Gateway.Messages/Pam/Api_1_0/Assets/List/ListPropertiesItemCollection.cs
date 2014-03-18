using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [CollectionDataContract(Name = "ListPropertiesItemCollection", Namespace = "", ItemName = "listPropertiesItem")]
    public class ListPropertiesItemCollection : List<ListPropertiesItem>
    {
        public ListPropertiesItemCollection()
        {
        }
        
        public ListPropertiesItemCollection(IEnumerable<ListPropertiesItem> collection) : base(collection)
        {
        }
    }
}