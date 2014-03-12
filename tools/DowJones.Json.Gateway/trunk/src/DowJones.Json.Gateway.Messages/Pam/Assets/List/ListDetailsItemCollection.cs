using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [CollectionDataContract(Name = "ListDetailsItemCollection", Namespace = "", ItemName = "listDetailsItem")]
    public class ListDetailsItemCollection : System.Collections.Generic.List<ListDetailsItem>
    {
    }
}