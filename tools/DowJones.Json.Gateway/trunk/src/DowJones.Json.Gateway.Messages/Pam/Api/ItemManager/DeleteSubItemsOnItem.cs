using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "DeleteSubItemsOnItem")]
    public class DeleteSubItemsOnItem
    {
        public DeleteSubItemsOnItem()
        {
            SubItemId = new List<long>();
        }

        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "subItemID")]
        public List<long> SubItemId { get; set; }
    }
}