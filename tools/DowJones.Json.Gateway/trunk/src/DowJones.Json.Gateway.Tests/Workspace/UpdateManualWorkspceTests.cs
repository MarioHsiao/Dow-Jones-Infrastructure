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
        public void GetWorkspaceByCodeRequest()
        {
            var r = new RestRequest<GetWorkspaceByCodeRequest>
            {
                Request = TestStubs.GetWorkspaceByCodeRequest(),
                ControlData = GetControlData(),
            };

            r.ControlData = new ControlData();
            UpdateControlData(r.ControlData);

            // Update Routing Data
            r.ControlData.RoutingData = new RoutingData();
            UpdateRoutingData(r.ControlData.RoutingData);


            try
            {
                var rm = new RestManager();
                var t = rm.Execute<GetWorkspaceByCodeRequest, GetWorkspaceByCodeResponse>(r);

                Console.Write(r.Request.ToJson(new JsonDotNetJsonConverter()));

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

        [TestMethod]
        public void GetWorkspacesDetailsListRequest()
        {
            var r = new RestRequest<GetWorkspacesDetailsListRequest>
            {
                Request = TestStubs.GetWorkspacesDetailsListRequest(),
                ControlData = GetControlData(),
            };
            r.ControlData = new ControlData();
            UpdateControlData(r.ControlData);

            // Update Routing Data
            r.ControlData.RoutingData = new RoutingData();
            UpdateRoutingData(r.ControlData.RoutingData);


            try
            {
                var rm = new RestManager();
                var t = rm.Execute<GetWorkspacesDetailsListRequest, GetWorkspacesDetailsListResponse>(r);

                Console.Write(r.Request.ToJson(new JsonDotNetJsonConverter()));

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



        [TestMethod]
        public void UpdateManualWorkspceRequest()
        {
            var r = new RestRequest<UpdateWorkspaceRequest>
                {
                    Request = TestStubs.GetUpdateRequest1(),
                    ControlData = GetControlData(),
                };
            r.ControlData = new ControlData();
            UpdateControlData(r.ControlData);

            // Update Routing Data
            r.ControlData.RoutingData = new RoutingData();
            UpdateRoutingData(r.ControlData.RoutingData);



            try
            {
                var rm = new RestManager();
                var t = rm.Execute<UpdateWorkspaceRequest, UpdateWorkspaceResponse>(r);

                Console.Write(r.Request.ToJson(new JsonDotNetJsonConverter()));

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
                SessionId = "27139XxX-JUYTIMJWGQ4TQNBYGUXVGRCFIJJU63DXJBHXI2KKIMZGC4CEOJVVOULTGBUUQ4TGFM3G2MCJMJWEOTLTJRMUGYSVG53GCNDKNZKHK4TGGZUXA2KJJJSGG3CKGA3UCQRLINAU2YKZHFJW2NLDG5YVOQTXKVCUOVJRMJCHKSCKK4YEG22VNVJGMRKQIVFGQVCYOAYHM3TFLEYDAL3YJBCEW3RQGQYGOS2MI4",
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
