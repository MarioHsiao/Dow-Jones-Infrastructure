using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [CollectionDataContract(Name = "ListTypeCollection", Namespace = "", ItemName = "type")]
    public class ListTypeCollection : List<ListType>
    {
    }
}