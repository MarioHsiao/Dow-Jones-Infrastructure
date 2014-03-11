using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "GetItemsByIDsResponse")]
    public class GetItemsByIDsResponse
    {
        public GetItemsByIDsResponse()
        {
            Item = new List<Item>();
        }

        [DataMember(Name = "item")]
        public List<Item> Item { get; set; }
    }
}