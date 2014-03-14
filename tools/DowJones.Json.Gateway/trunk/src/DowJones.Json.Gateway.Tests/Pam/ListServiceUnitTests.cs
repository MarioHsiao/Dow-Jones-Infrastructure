using System;
using System.Globalization;
using DowJones.Json.Gateway.Exceptions;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List;
using DowJones.Json.Gateway.Tests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = DowJones.Json.Gateway.Interfaces.Environment;

namespace DowJones.Json.Gateway.Tests.Pam
{
    [TestClass]
    public class ListServiceUnitTests : AbstractUnitTests
    {
        [TestMethod]
        public void GetListById()
        {
            var r = new RestRequest<GetListById>
                    {
                        Request = new GetListById
                                  {
                                      Id = 10,
                                  },
                        ControlData = GetControlData(),
                    };

            r.ControlData.RoutingData.ServerUri = "http://pamapi.dev.dowjones.net/";
            r.ControlData.RoutingData.Environment = Environment.Development;

            try
            {
                var rm = new RestManager();
                var t = rm.Execute<GetListById, GetListByIdResponse>(r);

                if (t.ReturnCode == 0)
                {
                    Assert.IsNotNull(t);
                    
                }
                else
                {
                    Console.WriteLine(t.Error.Message);
                    Assert.Fail(string.Concat("failed w/rc:= ",t.ReturnCode.ToString(CultureInfo.InvariantCulture)));
                }
                Console.WriteLine();
                
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
            var r = new RestRequest<CreateList>
            {
                Request = new CreateList
                {
                    List = new List {Name = "dave"}
                },
                ControlData = GetControlData(),
            };

            r.ControlData.RoutingData.ServerUri = "http://pamapi.dev.dowjones.net/";
            r.ControlData.RoutingData.Environment = Environment.Development;

            try
            {
                var rm = new RestManager();
                var t = rm.Execute<CreateList, CreateListResponse>(r);

                if (t.ReturnCode == 0)
                {
                    Assert.IsNotNull(t);
                }
                else
                {
                    Console.WriteLine(t.Error.Message);
                    Assert.Fail(string.Concat("failed w/rc:= ", t.ReturnCode.ToString(CultureInfo.InvariantCulture)));
                }
                Console.WriteLine();

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