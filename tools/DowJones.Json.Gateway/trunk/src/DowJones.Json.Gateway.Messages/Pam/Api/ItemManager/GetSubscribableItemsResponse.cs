using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "GetSubscribableItemsResponse")]
    public class GetSubscribableItemsResponse
    {
        public GetSubscribableItemsResponse()
        {
            this.ItemDetailsItem = new List<ItemDetailsItem>();
        }

        [DataMember(Name = "itemDetailsItem")]
        public List<ItemDetailsItem> ItemDetailsItem { get; set; }
    }
}