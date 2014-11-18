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
                    Request = TestStubs.getUpdateRequest(),
                    ControlData = GetControlData(),
                };

            UpdateControlData(r.ControlData);

            // Update Routing Data
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
            controlData.UserCommerceData =  new UserCommerceData()
                {
                    ClientType = "D",
                    AccessPointCode = "RS"
                };
            controlData.UserCredentialData = new UserCredentialData()
                {
                    SessionId = "27139ZzZKJAUQUSCAAAGUAYAAAAAAKWPAAAAAABSGAYTIMJRGE4DCMJTHAZDMOBW",
                    IpAddress = "127.0.0.1",
                    //UserGuidId = "3bb6cd20-968c-4def-805d-283710580dba",
                    //EncryptedUserId = "E6OO2HVCQHVPEXDO5LSIIKF7AY",
                    AccountId = "9FAC000700",
                    //UserId = "made5200",
                    UserId = "block0162",
                    Namespace = "16"
                };
            controlData.AuthorizationData = new AuthorizationData()
                {
                    AuthComponents = new List<AuthComponent>()
                        {
                            new AuthComponent
                                {
                                    Name = "ac4",
                                    Service = "membership",
                                    Value = "W"
                                },
                            new AuthComponent
                                {
                                    Name = "ac5",
                                    Service = "membership",
                                    Value = "EmailValid=30;EmailEnabled=Y;SelfReg=OFF;EditEmail=N;EditAddr=N;CB=N"
                                },
                            new AuthComponent
                                {
                                    Name = "AC1",
                                    Service = "PAM",
                                    Value = "NewsPage:25;INL:25;AUL:10;RGL:25"
                                },
                            new AuthComponent
                                {
                                    Name = "AC3",
                                    Service = "PAM",
                                    Value = "TS_ASIA,TS_AUNZ,TS_COMM,TS_DJIB,TS_DJN"
                                },
                            new AuthComponent
                                {
                                    Name = "AC4",
                                    Service = "pam",
                                    Value = "NLE"
                                },
                            new AuthComponent
                                {
                                    Name = "ADMIN_FLAG",
                                    Service = "PAM",
                                    Value = "G"
                                },
                            new AuthComponent
                                {
                                    Name = "GROUP_IDS",
                                    Service = "pam",
                                    Value = "12016,2622,2546,1902,699,22080,3663,3662,696,"
                                },

                        }
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
