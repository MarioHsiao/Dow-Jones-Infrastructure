using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "SyndicationItemResult")]
    public class SyndicationItemResult
    {
        public SyndicationItemResult()
        {
            Category = new List<string>();
        }

        [DataMember(Name = "itemID")]
        public long ItemId { get; set; }

        [DataMember(Name = "syndicationID")]
        public long SyndicationId { get; set; }

        [DataMember(Name = "category")]
        public List<string> Category { get; set; }

        [DataMember(Name = "status")]
        public ItemStatus Status { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}