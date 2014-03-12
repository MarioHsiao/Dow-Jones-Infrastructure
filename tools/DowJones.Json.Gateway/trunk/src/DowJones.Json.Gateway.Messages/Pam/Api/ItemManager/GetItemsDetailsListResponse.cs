using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "GetItemsDetailsListResponse")]
    public class GetItemsDetailsListResponse
    {
        public GetItemsDetailsListResponse()
        {
            ItemDetailsItem = new List<ItemDetailsItem>();
        }

        [DataMember(Name = "itemDetailsItem")]
        public List<ItemDetailsItem> ItemDetailsItem { get; set; }
    }
}