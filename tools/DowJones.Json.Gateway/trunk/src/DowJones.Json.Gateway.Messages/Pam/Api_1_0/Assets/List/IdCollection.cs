using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [CollectionDataContract(Name = "IdCollection", Namespace = "", ItemName = "id")]
    public class IdCollection : List<string>
    {
    }
}