using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Json.Gateway.Tests.Core
{
    [TestClass]
    public class PropertiesUnitTest
    {
        [TestMethod]
        public void GetTransportType()
        {
            var settings = typeof (Gateway.Properties.Settings);
            Assert.IsTrue(settings.IsPublic);
        }
    }
}
