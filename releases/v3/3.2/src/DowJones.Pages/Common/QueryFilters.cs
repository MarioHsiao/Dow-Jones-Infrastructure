using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Pages.Common
{
    [CollectionDataContract(Name = "queryFilters", ItemName = "queryFilter", Namespace = "")]
    public class QueryFilters : List<QueryFilter>
    {
    }
}
