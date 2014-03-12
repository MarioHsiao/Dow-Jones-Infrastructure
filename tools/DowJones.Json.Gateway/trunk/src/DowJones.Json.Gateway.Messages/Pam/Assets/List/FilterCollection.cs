using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [CollectionDataContract(Name = "FilterCollection", Namespace = "", ItemName = "filter")]
    public class FilterCollection : List<Filter>
    {
    }
}