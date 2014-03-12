using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [CollectionDataContract(Name = "ItemIdCollection", Namespace = "", ItemName = "id")]
    public class ItemIdCollection : List<string>
    {
    }
}