using Factiva.Gateway.Messages.Track.V1_0;
using Newtonsoft.Json;

namespace DowJones.AlertEditor
{
    public class AlertProperties
    {
        [JsonProperty("alertId")]
        public string AlertId { get; set; }

        [JsonProperty("alertName")]
        public string AlertName { get; set; }

        [JsonProperty("productType")]
        public ProductType ProductType { get; set; }

        [JsonProperty("isGroupAlert")]
        public bool IsGroupAlert { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("adjustToDaylightSavingsTime")]
        public bool AdjustToDaylightSavingsTime { get; set; }

        [JsonProperty("timeZoneOffset")]
        public string TimeZoneOffset { get; set; }

        [JsonProperty("documentType")]
        public DocumentType DocumentType { get; set; }

        [JsonProperty("documentFormat")]
        public DocumentFormat DocumentFormat { get; set; }

        [JsonProperty("deliveryMethod")]
        public DeliveryMethod DeliveryMethod { get; set; }

        [JsonProperty("deliveryTime")]
        public DeliveryTimes DeliveryTime { get; set; }

        [JsonProperty("removeDuplicate")]
        public DeduplicationLevel RemoveDuplicate { get; set; }

        [JsonProperty("dispositionType")]
        public DispositionType DispositionType { get; set; }
    }
}
