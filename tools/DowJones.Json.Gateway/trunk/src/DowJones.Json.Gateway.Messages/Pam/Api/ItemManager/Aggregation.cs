using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "Aggregation")]
    public class Aggregation
    {
        [DataMember(Name = "type")]
        public AggregationType Type { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}