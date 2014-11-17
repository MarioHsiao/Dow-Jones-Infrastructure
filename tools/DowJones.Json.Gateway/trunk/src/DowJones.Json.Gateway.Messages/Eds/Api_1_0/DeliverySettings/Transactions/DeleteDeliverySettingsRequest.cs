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
    [DataContract(Name = "DeleteDeliverySettings", Namespace = "")]
    [JsonObject(Title = "DeleteDeliverySettings")]
    public class DeleteDeliverySettingsRequest : IPostJsonRestRequest
    {
        [JsonProperty(PropertyName = "deliveryId", Required = Required.Always)]
        public string DeliveryId
        {
            get;
            set;
        }

        public string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}
