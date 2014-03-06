using System;
using DowJones.Json.Gateway.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace DowJones.Json.Gateway.Tests
{
    [TestClass]
    public class SerializationUnitTests
    {
        

        [TestMethod]
        public void CommerceDataJsonSerializationEmptyObject()
        {
            var userCommerData = new UserCommerceData();
            var str = JsonConvert.SerializeObject(userCommerData, Formatting.None, new JsonSerializerSettings
                                                                                      {
                                                                                          NullValueHandling = NullValueHandling.Ignore,
                                                                                          DefaultValueHandling = DefaultValueHandling.Ignore,
                                                                                          ContractResolver = new DefaultContractResolver(),
                                                                                         
                                                                                      });

            Assert.IsTrue(str == "{}");
        }

        [TestMethod]
        public void ControlDataSerializationEmptyObject()
        {
            var controlData = new ControlData();
            var str = JsonConvert.SerializeObject(controlData, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver(),
            });

            Assert.IsTrue(str == "{}");
        }


        private UserCommerceData GetUserCommerDataObject()
        {
            var userCommerceData = new UserCommerceData();
            return null;
        }
    }
}