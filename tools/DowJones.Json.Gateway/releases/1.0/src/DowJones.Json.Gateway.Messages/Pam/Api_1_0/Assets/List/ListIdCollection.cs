using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [CollectionDataContract(Name = "ListIdCollection", Namespace = "", ItemName = "id")]
    public class ListIdCollection : List<string>
    {
        public ListIdCollection()
        {
        }

        public ListIdCollection(IEnumerable<string> collection) : base(collection)
        {
        }
    }
}