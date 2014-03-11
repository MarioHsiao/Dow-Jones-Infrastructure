using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "GetItemsByIDs")]
    public class GetItemsByIDs
    {
        public GetItemsByIDs()
        {
            Id = new List<long>();
        }

        [DataMember(Name = "id")]
        public List<long> Id { get; set; }
    }
}