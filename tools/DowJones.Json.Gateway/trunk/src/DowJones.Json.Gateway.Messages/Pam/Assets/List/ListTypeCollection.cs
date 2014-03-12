using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [CollectionDataContract(Name = "ListTypeCollection", Namespace = "", ItemName = "type")]
    public class ListTypeCollection : System.Collections.Generic.List<ListType>
    {
    }
}