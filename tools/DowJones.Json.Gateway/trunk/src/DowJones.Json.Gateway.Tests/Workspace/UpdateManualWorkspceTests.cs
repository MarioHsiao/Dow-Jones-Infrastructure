using System;
using System.Collections.Generic;
using System.Globalization;
using DowJones.Json.Gateway.Common;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Extensions;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Messages.Core;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace.Transactions;
using DowJones.Json.Gateway.Tests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace DowJones.Json.Gateway.Tests.Workspace
{
    [TestClass]
    public class UpdateManualWorkspceTests : AbstractUnitTests
    {
        [TestMethod]
        public void UpdateManualWorkspceRequest()
        {
            var r = new RestRequest<UpdateWorkspaceRequest>
                {
                    Request = TestStubs.getUpdateRequest1(),
                    ControlData = GetControlData(),
                };

            UpdateControlData(r.ControlData);

            // Update Routing Data
            r.ControlData.RoutingData = new RoutingData();
            UpdateRoutingData(r.ControlData.RoutingData);



            try
            {
                var rm = new RestManager();
                var t = rm.Execute<UpdateWorkspaceRequest, UpdateWorkspaceResponse>(r);

                Console.Write(r.Request.ToJson(new DataContractJsonConverter()));

                if (t.ReturnCode == 0)
                {
                    Assert.IsNotNull(t);
                }
                else
                {
                    Console.WriteLine(t.Error.Message);
                    Assert.Fail(string.Concat("failed w/rc:= ", t.ReturnCode.ToString(CultureInfo.InvariantCulture)));
                }
                Console.WriteLine(t.Data.ToJson(true));
                return;
            }
            catch (JsonGatewayException gatewayException)
            {
                Console.Write(gatewayException.Message);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            Assert.Fail("unable delete list");
        }

        private static void UpdateControlData(IControlData controlData)
        {
            controlData.UserCredentialData = new UserCredentialData()
            {
                SessionId = "27139ZzZKJAUQUSCAAAGUAYAAAAAALORAAAAAABSGAYTIMJRGE4TAOJTGE2DCMJR",
            };
        }

        private static void UpdateRoutingData(IRoutingData routingData)
        {
            // ReSharper disable StringLiteralTypo
            // ReSharper disable CommentTypo
            // routingData.HttpEndPoint = "http://sktfrtutil01.dev.us.factiva.net:9097";
            // ReSharper restore CommentTypo
            // ReSharper restore StringLiteralTypo

            // ReSharper disable StringLiteralTypo
            routingData.ServerUri = "http://utilities.int.dowjones.com/restserviceproxy";
            routingData.ServiceUrl = "http://pamapi.int.dowjones.net/";//
                //"http://pamapi.local.dowjones.net/";
            // ReSharper restore StringLiteralTypo

            routingData.TransportType = "RTS";
            routingData.Environment = Common.Environment.Proxy;
            routingData.Serializer = JsonSerializer.JsonDotNet;
        }
    }
}
