using System;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Newtonsoft.Json;

namespace DowJones.Topic
{
    public class TopicProperties
    {
        [JsonProperty("topicId")]
        public string TopicId { get; set; }

        [JsonProperty("topicName")]
        public string TopicName { get; set; }

        [JsonProperty("topicShortName")]
        public string TopicShortName { get; set; }

        [JsonProperty("chartColor")]
        public string ChartColor { get; set; }

        public string CreatedBy { get; set; }

        public DateTime LastModifiedDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("topicCategory")]
        public MadeTopicCategory TopicCategory { get; set; }

        [JsonProperty("searchType")]
        public SearchSetupScreen SearchType { get; set; }

        public DateTime QueryUpdateDate { get; set; }
    }
}
