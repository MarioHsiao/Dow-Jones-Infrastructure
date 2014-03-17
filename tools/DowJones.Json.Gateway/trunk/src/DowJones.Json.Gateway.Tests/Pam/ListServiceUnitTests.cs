using System;
using System.Globalization;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Extensions;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List.Transactions;
using DowJones.Json.Gateway.Tests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = DowJones.Json.Gateway.Interfaces.Environment;

namespace DowJones.Json.Gateway.Tests.Pam
{
    [TestClass]
    public class ListServiceUnitTests : AbstractUnitTests
    {
        [TestMethod]
        public void GetListByIdRequest()
        {
            var r = new RestRequest<GetListByIdRequest>
                    {
                        Request = new GetListByIdRequest
                                  {
                                      Id = 10,
                                  },
                        ControlData = GetControlData(),
                    };

            r.ControlData.RoutingData.TransportType = "HTTP";
            // ReSharper disable StringLiteralTypo
            r.ControlData.RoutingData.ServerUri = "http://pamapi.dev.dowjones.net/";
            // ReSharper restore StringLiteralTypo
            r.ControlData.RoutingData.Environment = Environment.Development;
            r.ControlData.RoutingData.Environment = Environment.Development;
            r.ControlData.RoutingData.Serializer = JsonSerializer.DataContract;

            try
            {
                var rm = new RestManager();
                var t = rm.Execute<GetListByIdRequest, GetListByIdResponse>(r);

                if (t.ReturnCode == 0)
                {
                    Assert.IsNotNull(t);
                    
                }
                else
                {
                    Console.WriteLine(t.Error.Message);
                    Assert.Fail(string.Concat("failed w/rc:= ",t.ReturnCode.ToString(CultureInfo.InvariantCulture)));
                }
                Console.WriteLine(t.Data.ToJson(true));
                
            }
            catch (JsonGatewayException gatewayException)
            {
                Console.Write(gatewayException.Message);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        [TestMethod]
        public void CreateList()
        {
            var r = new RestRequest<CreateListRequest>
            {
                Request = new CreateListRequest
                {
                    List = new List {Name = "dave"}
                },
                ControlData = GetControlData(),
            };

            // ReSharper disable StringLiteralTypo
            r.ControlData.RoutingData.ServerUri = "http://pamapi.dev.dowjones.net/";
            // ReSharper restore StringLiteralTypo
            r.ControlData.RoutingData.TransportType = "RTS";
            r.ControlData.RoutingData.Environment = Environment.Development;

            try
            {
                var rm = new RestManager();
                var t = rm.Execute<CreateListRequest, CreateListResponse>(r);

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

            }
            catch (JsonGatewayException gatewayException)
            {
                Console.Write(gatewayException.Message);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}