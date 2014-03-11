using System;
using DowJones.Json.Gateway.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Json.Gateway.Tests
{
    [TestClass]
    public class ExceptionsUnitTest
    {
        [TestMethod]
        public void ExpectedErrorParsing()
        {
            const string content = "{\"Error\":{\"Code\":589500,\"Message\":\"Generic ServiceProxy Error Cannot find module 'sax'\"}}";
            var ex = JsonGatewayException.ParseExceptionMessage(content);

            Assert.IsTrue(ex.ReturnCode == 589500);
            Assert.IsTrue(ex.Message == "Generic ServiceProxy Error Cannot find module 'sax'");
        }

        [TestMethod]
        public void UnExpectedErrorParsing()
        {
            const string content = "Generic ServiceProxy Error Cannot find module 'sax'";
            var ex = JsonGatewayException.ParseExceptionMessage(content);

            Assert.IsTrue(ex.ReturnCode == JsonGatewayException.GenericError);
            Assert.IsTrue(ex.Message == "Generic ServiceProxy Error Cannot find module 'sax'");
        }
    }
}
