using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [CollectionDataContract(Name = "FilterCollection", Namespace = "", ItemName = "filter")]
    public class FilterCollection : List<Filter>
    {
        public FilterCollection()
        {
        }

        public FilterCollection(IEnumerable<Filter> collection) : base(collection)
        {
        }
    }
}