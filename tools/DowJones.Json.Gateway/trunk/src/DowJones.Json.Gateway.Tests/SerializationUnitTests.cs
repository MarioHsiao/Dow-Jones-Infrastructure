using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Messages.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Json.Gateway.Tests
{
    [TestClass]
    public class SerializationUnitTests
    {
        

        [TestMethod]
        public void CommerceDataJsonSerializationEmptyObject()
        {
            var userCommerData = new UserCommerceData();
            var str = JsonDotNetDataConverterSingleton.Instance.Serialize(userCommerData);
            Assert.IsTrue(str == "{}");
        }

        [TestMethod]
        public void ControlDataSerializationEmptyObject()
        {
            var controlData = new ControlData();
            var str = JsonDotNetDataConverterSingleton.Instance.Serialize(controlData);
            Assert.IsTrue(str == "{}");
        }

        [TestMethod]
        public void UserCommerceDataSerializationEmptyObject()
        {
            var userCommerceData = new UserCommerceData();
            var str = JsonDotNetDataConverterSingleton.Instance.Serialize(userCommerceData);
            Assert.IsTrue(str == "{}");
        }


        [TestMethod]

        public void ControlDataDeserialization()
        {
            var data = "{\"RoutingData\":{\"ChunkingFlag\":false,\"ContentServerAddress\":5042,\"ContentServerPortId\":0,\"ContextId\":0,\"DeliveryService\":\"Factiva.OCPChannelDS.1\",\"Flags\":0,\"FunctionType\":20205,\"MajorServiceVersion\":1,\"MinorServiceVersion\":0,\"MoreDataFlag\":false,\"SequenceNo\":1,\"ServiceType\":251,\"SourceAddress\":839,\"TimeStamp\":1248287501,\"TransactionId\":30725,\"Tokens\":[]},\"UserCommerceData\":{\"AccessPointCode\":\"9\",\"ClientType\":\"D\",\"CompositeId\":\"00000000000000000000\",\"Tokens\":[]},\"UserCredentialData\":{\"AccountId\":\"9FAC000700\",\"IpAddress\":\"172.26.33.51\",\"Namespace\":\"16\",\"SessionId\":\"27137ZzZKJAUQOKCAAAGUAIAAAAAAMI7AAAAAABSGAYTIMBUGIYTCNBRGIZTOOJW\",\"Tokens\":[],\"UserFlavor\":\"F\",\"UserId\":\"dacostad \"},\"PlatformAdminData\":{\"ArmRemainingPercentage\":\"100\",\"ArmRemainingTime\":\"60\",\"TransactionTimeout\":0,\"Tokens\":[]},\"TransactionCacheData\":{\"CacheExpirationTime\":0,\"CacheStatus\":3,\"ForceCacheRefresh\":false,\"Tokens\":[]},\"UserAdminData\":{\"Tokens\":[]}}";
            var cd = JsonDotNetDataConverterSingleton.Instance.Deserialize<ControlData>(data);

            Assert.IsTrue(cd != null);
        }


    }
}