using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [CollectionDataContract(Name = "ItemIdCollection", Namespace = "", ItemName = "id")]
    public class ItemIdCollection : System.Collections.Generic.List<string>
    {
    }
}