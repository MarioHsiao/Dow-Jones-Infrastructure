using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Eds.Api_1_0.DeliverySettings.Transactions
{
    [ServicePath("1.0/DeliverySettings")]
    [DataContract(Name = "GetDeliverySettings", Namespace = "")]
    [JsonObject(Title = "UpdateSettings")]
    public class GetDeliverySettingsRequest : IPostJsonRestRequest
    {
        [JsonProperty(PropertyName = "returnType", Required = Required.Always)]
        public ReturnType ReturnType
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryId", Required = Required.Always)]
        public string DeliveryId
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryType", Required = Required.Always)]
        public DeliveryType? DeliveryType { get; set; }

        public string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}
