using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "ChartFilter")]
    public class ChartFilter
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "chartFilterSearchType")]
        public ChartFilterSearchType? ChartFilterSearchType { get; set; }

        [DataMember(Name = "chartFilterOperator")]
        public ChartFilterOperator? ChartFilterOperator { get; set; }
    }
}