using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Json.Gateway.Interfaces;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Eds.Api_1_0.DeliverySettings.Transactions
{
    public class GetDeliverySettingsResponse : IJsonRestResponse
    {
        [JsonProperty(PropertyName = "delivery", Required = Required.Always)]
        public List<Delivery> Delivery { get; set; }
    }
}
