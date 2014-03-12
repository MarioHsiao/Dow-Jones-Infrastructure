using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "QueryReference")]
    public class QueryReference
    {
        [DataMember(Name = "queryType")]
        public QueryType QueryType { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "color")]
        public string Color { get; set; }
    }
}