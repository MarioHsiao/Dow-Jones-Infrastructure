using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [CollectionDataContract(Name = "ListPropertiesItemCollection", Namespace = "", ItemName = "listPropertiesItem")]
    public class ListPropertiesItemCollection : System.Collections.Generic.List<ListPropertiesItem>
    {
    }
}