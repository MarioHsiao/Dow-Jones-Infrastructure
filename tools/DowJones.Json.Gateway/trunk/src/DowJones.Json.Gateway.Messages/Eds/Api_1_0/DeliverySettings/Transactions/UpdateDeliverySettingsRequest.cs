using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Json.Gateway.Messages.Eds.Api_1_0.DeliverySettings.Transactions
{
    public class UpdateDeliverySettingsRequest : IPostJsonRestRequest
    {
        public UpdateDeliverySettingsRequest()
        {
            this.delivery = new List<Delivery>();
        }

        [JsonProperty(PropertyName = "delivery", Required = Required.Always)]
        public List<Delivery> delivery
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
