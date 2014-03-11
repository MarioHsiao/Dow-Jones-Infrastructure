using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "AddSubItemsToItemResponse")]
    public class AddSubItemsToItemResponse
    {
        [DataMember(Name = "id")]
        public List<long> Id { get; set; }

        public AddSubItemsToItemResponse()
        {
            Id = new List<long>();
        }
    }
}