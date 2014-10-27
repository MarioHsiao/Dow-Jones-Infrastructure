using System;
using System.Globalization;
using System.Threading;
using DowJones.Json.Gateway.Common;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Extensions;
using DowJones.Json.Gateway.Interfaces;
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
    public class ODEServiceUnitTests : AbstractUnitTests
    {
        [TestMethod]
        public void GenerateOdeRequest_Passing_Valid_Request_To_Execute_Method_Should_Return_Valid_Response()
        {

            var r = new RestRequest<GenerateOdeRequest>
            {
                Request = new GenerateOdeRequest()
                {
                  TicketCollection  = TestStubs.GetTicketCollection(),
                  requestMode = RequestMode.ASync
                },
                ControlData = GetControlData()
            };

            UpdateRoutingData(r.ControlData.RoutingData);

            var response = Execute(r);

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

        [TestMethod]
        public void GenerateOdeRequest_Passing_Request_With_Empty_Recipient_Property_To_Execute_Method_Should_Return_Error_Response()
        {
            var ticketCol = TestStubs.GetTicketCollection();
            ticketCol[0].Recepient = null;

            var r = new RestRequest<GenerateOdeRequest>
            {
                Request = new GenerateOdeRequest()
                {
                    TicketCollection = ticketCol,
                    requestMode = RequestMode.ASync
                },
                ControlData = GetControlData()
            };

            UpdateRoutingData(r.ControlData.RoutingData);

            var response = Execute(r);

            if (response.ReturnCode == 0)
            {
                Assert.Fail(string.Concat("failed valid response w/rc:= ", response.ReturnCode.ToString(CultureInfo.InvariantCulture)));
                Assert.IsNotNull(response);
            }
            else
            {
                Console.WriteLine(response.Error.Message);
                Assert.AreEqual(response.ReturnCode, 88103);
            }
        }

        [TestMethod]
        public void GenerateOdeRequest_Passing_Request_With_Empty_TicketCollection_Property_To_Execute_Method_Should_Return_Error_Response()
        {
            var ticketCol = TestStubs.GetTicketCollection();
            ticketCol[0].Recepient = null;

            var r = new RestRequest<GenerateOdeRequest>
            {
                Request = new GenerateOdeRequest()
                {
                    TicketCollection = null,
                    requestMode = RequestMode.ASync
                },
                ControlData = GetControlData()
            };

            UpdateRoutingData(r.ControlData.RoutingData);

            var response = Execute(r);

            if (response.ReturnCode == 0)
            {
                Assert.Fail(string.Concat("failed valid response w/rc:= ", response.ReturnCode.ToString(CultureInfo.InvariantCulture)));
                Assert.IsNotNull(response);
            }
            else
            {
                Console.WriteLine(response.Error.Message);
                Assert.AreEqual(response.ReturnCode, 88220);
            }

        }

        //[TestMethod]
        //public void GenereateOdeRequest_Property_ToEmailAddress_Is_Never_Null()
        //{
        //    var odeResponse = new GenerateOdeRequest() { ToEmailAddress = null };
        //    var toEmailAddressIsNotNull = odeResponse.ToEmailAddress != null;
        //    Assert.IsTrue(toEmailAddressIsNotNull, "GenerateOdeResponse's ToEmailAddress property is null");
        //}

        [TestMethod]
        public void GenereateOdeResponse_Property_ToEmailAddress_Is_Never_Null()
        {
            var odeResponse = new GenerateOdeResponse(){ToEmailAddress = null};
            odeResponse.__ToEmailAddress = null;
            var toEmailAddressIsNotNull = odeResponse.ToEmailAddress != null;
            Assert.IsTrue(toEmailAddressIsNotNull, "GenerateOdeResponse's ToEmailAddress property is null");
        }

        private static void UpdateRoutingData(IRoutingData routingData)
        {
            // ReSharper disable StringLiteralTypo
            // ReSharper disable CommentTypo
            // routingData.HttpEndPoint = "http://sktfrtutil01.dev.us.factiva.net:9097";
            // ReSharper restore CommentTypo
            // ReSharper restore StringLiteralTypo

            // ReSharper disable StringLiteralTypo
            //routingData.ServerUri = "http://utilities.int.dowjones.com/restserviceproxy";
            // ReSharper restore StringLiteralTypo
            routingData.ServiceUrl = "http://edsapi.int.dowjones.net/";
            routingData.TransportType = "HTTP";
            routingData.Environment = Environment.Proxy;
            routingData.Serializer = JsonSerializer.JsonDotNet;
        }

        private RestResponse<GenerateOdeResponse> Execute(RestRequest<GenerateOdeRequest> r)
        {
            try
            {
                var rm = new RestManager();
                var t = rm.Execute<GenerateOdeRequest, GenerateOdeResponse>(r);

                Console.Write(r.Request.ToJson(new DataContractJsonConverter()));
                Console.WriteLine(t.Data.ToJson(true));
 
                return t;
            }
            catch (JsonGatewayException gatewayException)
            {
                Console.Write(gatewayException.Message);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            Assert.Fail("unable to generate ODE");

            return null;
        }
    }
}
