using System;
using System.Globalization;
using System.Threading;
using DowJones.Json.Gateway.Common;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Extensions;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Messages.Eds.Api_1_0.DeliverySettings.Transactions;
using DowJones.Json.Gateway.Messages.Eds.Api_1_0.Ode;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing;
using DowJones.Json.Gateway.Tests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = DowJones.Json.Gateway.Common.Environment;
using DowJones.Json.Gateway.Messages.Eds.Api_1_0.Ode.Transactions;
using System.Collections.Generic;

namespace DowJones.Json.Gateway.Tests.Eds
{
    [TestClass]
    public class DeliverySettingsServiceTests : AbstractUnitTests
    {
        [TestMethod]
        public void Test_Method()
        {

            var r = new RestRequest<CreateDeliverySettingsRequest>
            {
                Request = new CreateDeliverySettingsRequest()
                {
                },
                ControlData = GetControlData()
            };

            UpdateRoutingData(r.ControlData.RoutingData);

            var response = Execute<CreateDeliverySettingsRequest, CreateDeliverySettingsResponse>(r);

            if (response.ReturnCode == 0)
            {
                Assert.IsNotNull(response);
            }
            else
            {
                Console.WriteLine(response.Error.Message);
                Assert.Fail(string.Concat("failed w/rc:= ", response.ReturnCode.ToString(CultureInfo.InvariantCulture)));
            }
        }

        private static void UpdateRoutingData(IRoutingData routingData)
        {
            routingData.ServiceUrl = "http://edsapi.int.dowjones.net/";
            routingData.TransportType = "HTTP";
            routingData.Environment = Environment.Proxy;
            routingData.Serializer = JsonSerializer.JsonDotNet;
        }
    }
}
