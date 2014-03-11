using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "SyndicationItemProperties")]
    public class SyndicationItemProperties : ItemProperties
    {
        public SyndicationItemProperties()
        {
            Aggregation = new Aggregation();
            Category = new List<string>();
        }

        [DataMember(Name = "type")]
        public SyndicationItemValueType Type { get; set; }

        [DataMember(Name = "syndicationItemType")]
        public SyndicationItemType SyndicationItemType { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "category")]
        public List<string> Category { get; set; }

        [DataMember(Name = "aggregation")]
        public Aggregation Aggregation { get; set; }

        [DataMember(Name = "syndicationId")]
        public long SyndicationId { get; set; }
    }
}