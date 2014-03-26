using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Json.Gateway.Messages.Core;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                                                         ClientType = "Q",
                                                         AccessPointCode = "DX",
                                                     }, 
                                  UserCredentialData = new UserCredentialData
                                                       {
                                                           SessionId = "27139XxX_f336e3d6-a46c-c332-f5e3-a9e714bf0d49",
                                                           IpAddress = "127.0.0.1",
                                                           UserGuidId = "3bb6cd20-968c-4def-805d-283710580dba",
                                                           EncryptedUserId = "E6OO2HVCQHVPEXDO5LSIIKF7AY",
                                                           AccountId = "9PRO001000",
                                                           UserId = "listuser1",
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
                                                },
                                    
                              };
            return controlData;
        }
        
    }
}
