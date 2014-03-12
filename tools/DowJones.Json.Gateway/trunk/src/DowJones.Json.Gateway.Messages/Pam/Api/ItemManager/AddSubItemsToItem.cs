using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "AddSubItemsToItem")]
    public class AddSubItemsToItem
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "subItem")]
        public List<SubItem> SubItem { get; set; }

        public AddSubItemsToItem()
        {
            SubItem = new List<SubItem>();
        }
    }
}