using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [CollectionDataContract(Name = "IdCollection", Namespace = "", ItemName = "id")]
    public class IdCollection : System.Collections.Generic.List<string>
    {
    }
}