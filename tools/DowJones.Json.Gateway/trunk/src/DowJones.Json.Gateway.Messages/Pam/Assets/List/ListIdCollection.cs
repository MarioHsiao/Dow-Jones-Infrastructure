using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [CollectionDataContract(Name = "ListIdCollection", Namespace = "", ItemName = "id")]
    public class ListIdCollection : System.Collections.Generic.List<string>
    {
    }
}