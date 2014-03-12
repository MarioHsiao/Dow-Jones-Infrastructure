using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "GetItemsByValueResponse")]
    public class GetItemsByValueResponse
    {
        public GetItemsByValueResponse()
        {
            ItemDetailsItem = new List<ItemDetailsItem>();
        }

        [DataMember(Name = "itemDetailsItem")]
        public List<ItemDetailsItem> ItemDetailsItem { get; set; }
    }
}