using DowJones.Json.Gateway.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Json.Gateway.Messages.Eds.Api_1_0.DeliverySettings.Transactions
{
    public class CreateDeliverySettingsResponse : IJsonRestResponse
    {
        public CreateDeliverySettingsResponse()
        {
            this.delivery = new List<Delivery>();
        }

        public List<Delivery> delivery
        {
            get;
            set;
        }
    }
}
