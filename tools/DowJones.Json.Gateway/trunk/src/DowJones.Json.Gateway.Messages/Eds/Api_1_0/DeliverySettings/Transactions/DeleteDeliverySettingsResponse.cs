using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Eds.Api_1_0.DeliverySettings.Transactions
{
    public class DeleteDeliverySettingsResponse : IJsonRestResponse
    {
        public bool IsSuccess
        {
            get;
            set;
        }
    }
}
