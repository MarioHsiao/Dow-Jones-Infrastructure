using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [CollectionDataContract(Name = "ItemGroupCollection", Namespace = "", ItemName = "itemGroup")]
    public class ItemGroupCollection : System.Collections.Generic.List<ItemGroup>
    {
    }
}