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


    }
}