using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Extentions;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Messages.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Json.Gateway.Tests
{
    [TestClass]
    public class RestManagerUnitTests
    {

        [TestMethod]
        public void ExecuteBasic()
        {
            var r = new RestRequest<GetPageByName>
                    {
                        Method = Method.GET,
                        Request = new GetPageByName { Name = "new", Type = "DJXMaster" },
                        ControlData = GetControlData()
                    };

            var rm = new RestManager();
            
            var t = rm.Execute<GetPageByName, GetPageByNameResponse>(r);
           

          /*  
            Assert.IsNotNull(t.Data);
            Assert.IsInstanceOfType(t.Data, typeof(List<string>));
            Assert.IsTrue(t.Data.Count == 2);
            Assert.IsTrue(t.Data[0] == "value1");
            Assert.IsTrue(t.Data[1] == "value2");           
          */
        }

        [TestMethod]
        public void TestServicePath()
        {
            var test = new GetPageByName();
            Console.Write(test.GetServicePath());
        }

        private static ControlData GetControlData()
        {
            // {"UserCredentialData":{"SessionId":"27139XxX_f336e3d6-a46c-c332-f5e3-a9e714bf0d49","IpAddress":"127.0.0.1","UUID":"3bb6cd20-968c-4def-805d-283710580dba","EID":"E6OO2HVCQHVPEXDO5LSIIKF7AY","AccountId":"9GEE001500","UserId":"E000008728"},"UserCommerceData":{"ClientType":"Q","AccessPointCode":"DX"},"RoutingData":{"ContentServerAddress":0,"TransportType":"HTTP"},"PlatformAdminData":{"TransactionTimeout":30}}
            //{"SessionId":"27139XxX_f336e3d6-a46c-c332-f5e3-a9e714bf0d49","IpAddress":"127.0.0.1","UUID":"3bb6cd20-968c-4def-805d-283710580dba","EID":"E6OO2HVCQHVPEXDO5LSIIKF7AY","AccountId":"9GEE001500","UserId":"E000008728"}
            var controlData = new ControlData
                              {
                                  UserCommerceData = new UserCommerceData
                                                     {
                                                         ClientType = "Q",
                                                         AccessPointCode = "DX",
                                                     }, 
                                  UserCredentialData = new UserCredentialData
                                                       {
                                                           SessionId = "27139XxX_f336e3d6-a46c-c332-f5e3-a9e714bf0d49",
                                                           IpAddress = "127.0.0.1",
                                                           UserGuidId = "3bb6cd20-968c-4def-805d-283710580dba",
                                                           EncryptedUserId = "E6OO2HVCQHVPEXDO5LSIIKF7AY",
                                                           AccountId = "9GEE001500",
                                                           UserId = "E000008728"
                                                       }, 
                                  TransactionCacheData = new TransactionCacheData(),
                                  PlatformAdminData = new PlatformAdminData
                                                      {
                                                          TransactionTimeout = 30
                                                      },
                                  RoutingData = new RoutingData
                                                {
                                                    ContentServerAddress = 0,
                                                    TransportType = "HTTP",
                                                },
                                    
                              };
            Console.Write(controlData.ToJson());
            return controlData;
        }
        
    }

    [ServicePath("pamapi/1.0/DJXPages.svc")]
    [DataContract(Name = "GetPageByName")]
    public class GetPageByName : IGetJsonRestRequest
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    public class GetPageByNameResponse : IJsonRestResponse
    {
    }
}