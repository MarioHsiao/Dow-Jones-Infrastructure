using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "GetSubscribableItems")]
    public class GetSubscribableItems
    {
        public GetSubscribableItems()
        {
            ShareScope = new List<ShareScope>();
        }

        [DataMember(Name = "itemType")]
        public ItemType ItemType { get; set; }

        [DataMember(Name = "shareScope")]
        public List<ShareScope> ShareScope { get; set; }

        [DataMember(Name = "excludeMyItems")]
        public bool ExcludeMyItems { get; set; }
    }
}