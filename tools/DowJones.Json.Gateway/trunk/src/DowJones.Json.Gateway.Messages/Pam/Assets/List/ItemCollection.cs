using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [CollectionDataContract(Name = "ItemCollection", Namespace = "", ItemName = "item")]
    public class ItemCollection : System.Collections.Generic.List<Item>
    {
    }
}