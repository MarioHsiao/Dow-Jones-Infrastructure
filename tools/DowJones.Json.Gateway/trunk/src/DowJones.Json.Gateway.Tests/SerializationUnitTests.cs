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
            var str = JsonDataConverterSingleton.Instance.Serialize(userCommerData);
            Assert.IsTrue(str == "{}");
        }

        [TestMethod]
        public void ControlDataSerializationEmptyObject()
        {
            var controlData = new ControlData();
            var str = JsonDataConverterSingleton.Instance.Serialize(controlData);
            Assert.IsTrue(str == "{}");
        }

        [TestMethod]
        public void UserCommerceDataSerializationEmptyObject()
        {
            var userCommerceData = new UserCommerceData();
            var str = JsonDataConverterSingleton.Instance.Serialize(userCommerceData);
            Assert.IsTrue(str == "{}");
        }
    }
}