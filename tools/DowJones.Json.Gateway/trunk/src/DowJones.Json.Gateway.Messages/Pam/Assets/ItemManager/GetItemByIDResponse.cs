using System.Runtime.Serialization;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "GetItemByIDResponse")]
    public class GetItemByIDResponse : IJsonRestResponse
    {
        [DataMember(Name = "item")]
        public Item Item { get; set; }
    }
}