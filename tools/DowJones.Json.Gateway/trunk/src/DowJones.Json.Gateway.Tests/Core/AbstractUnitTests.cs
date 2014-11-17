using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Json.Gateway.Common;
using DowJones.Json.Gateway.Extensions;
using DowJones.Json.Gateway.Messages.Core;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Exceptions;

namespace DowJones.Json.Gateway.Tests.Core
{
    public abstract class AbstractUnitTests
    {
        [TestInitialize]
        public void SetupLogger()
        {
            BasicConfigurator.Configure();
            var appender = LogManager.GetRepository()
                .GetAppenders()
                .OfType<ConsoleAppender>()
                .First();

            appender.Layout = new PatternLayout("%d %-5level %logger - %m%n"); // set pattern
        }

        public ControlData GetControlData()
        {
            // {"UserCredentialData":{"SessionId":"27139XxX_f336e3d6-a46c-c332-f5e3-a9e714bf0d49","IpAddress":"127.0.0.1","UUID":"3bb6cd20-968c-4def-805d-283710580dba","EID":"E6OO2HVCQHVPEXDO5LSIIKF7AY","AccountId":"9GEE001500","UserId":"E000008728"},"UserCommerceData":{"ClientType":"Q","AccessPointCode":"DX"},"RoutingData":{"ContentServerAddress":0,"TransportType":"HTTP"},"PlatformAdminData":{"TransactionTimeout":30}}
            //{"SessionId":"27139XxX_f336e3d6-a46c-c332-f5e3-a9e714bf0d49","IpAddress":"127.0.0.1","UUID":"3bb6cd20-968c-4def-805d-283710580dba","EID":"E6OO2HVCQHVPEXDO5LSIIKF7AY","AccountId":"9GEE001500","UserId":"E000008728"}
            var controlData = new ControlData
            {
                UserCommerceData = new UserCommerceData
                {
                    ClientType = "D",
                    AccessPointCode = "S"
                },
                UserCredentialData = new UserCredentialData
                {
                    SessionId = "27137XxX_JUYTIMJWGI2DMNBYGUXVA6CTHBFTESDDNJIHUULOPFEDORCXIJTG22CPOIXVORCROVWDG6TKOYVWYVKPOAZTKK2XGBVGYYLVG5RU63DHORWGQ32ZINDVOZKUOUZUKLZPKRZUQUTBGRIFE23JIZAVOQSSOZVU2TCUK53VOR2ZHB2UOWDFG5KWCMTPJ4YVU23DPBWFSQKQMFIXS2DDLJXDITSBMFZEML3UKNQXO6CUPJIUUVKKOZ3E4L3GMVLEG5SUORKFC33CJ5ZG4MCJMM4HKZ3DNRKEYZD2JM4WGPKH",
                    IpAddress = "127.0.0.1",
                    //UserGuidId = "3bb6cd20-968c-4def-805d-283710580dba",
                    //EncryptedUserId = "E6OO2HVCQHVPEXDO5LSIIKF7AY",
                    AccountId = "9PRO001900",
                    UserId = "nareshx",
                    Namespace = "16"
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
                    Serializer = JsonSerializer.JsonDotNet,
                    ServiceUrl = "http://edsapi.int.dowjones.net/",
                    Environment = DowJones.Json.Gateway.Common.Environment.Proxy
                },
                AuthorizationData = new AuthorizationData()
                {
                    AuthComponents = new List<AuthComponent>()
                    {
                        new AuthComponent
                        {
                            Name = "FTODE",
                            Service = "EMAIL",
                            Value = "1"
                        }

                    }
                }
            };
            return controlData;
        }

        protected RestResponse<TRes> Execute<TReq, TRes>(RestRequest<TReq> restRequest)
            where TReq : IJsonRestRequest, new()
            where TRes : IJsonRestResponse, new()
        {
            try
            {
                var rm = new RestManager();
                var t = rm.Execute<TReq, TRes>(restRequest);

                Console.Write(restRequest.Request.ToJson(new DataContractJsonConverter()));
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
