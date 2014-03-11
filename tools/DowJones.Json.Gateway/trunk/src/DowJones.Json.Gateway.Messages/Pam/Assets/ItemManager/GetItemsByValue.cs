using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "GetItemsByValue")]
    public class GetItemsByValue
    {
        public GetItemsByValue()
        {
            Value = new List<string>();
        }

        [DataMember(Name = "type")]
        public ItemType Type { get; set; }

        [DataMember(Name = "sortBy")]
        public ItemSortBy SortBy { get; set; }

        [DataMember(Name = "sortOrder")]
        public SortOrder SortOrder { get; set; }

        [DataMember(Name = "Value")]
        public List<string> Value { get; set; }
    }
}