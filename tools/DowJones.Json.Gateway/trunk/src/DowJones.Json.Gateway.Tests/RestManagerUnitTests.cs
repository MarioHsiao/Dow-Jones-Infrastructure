using System.Collections.Generic;
using DowJones.Json.Gateway.Messages.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Json.Gateway.Tests
{
    [TestClass]
    public class RestManagerUnitTests
    {
        [TestMethod]
        public void Execute()
        {
            var r = new RestRequest<ControlData>
                    {
                        Method = Method.GET, 
                        ServerUri = "http://localhost", 
                        ResourcePath = "DowJones.Json.Gateway.Mvc/api/data?type=json",
                        ControlData = new ControlData()
                    };

            var rm = new RestManager();
            var t = rm.Execute<ControlData,List<string>>(r);
           

            Assert.IsNotNull(t.Data);
            Assert.IsInstanceOfType(t.Data, typeof(List<string>));
            Assert.IsTrue(t.Data.Count == 2);
            Assert.IsTrue(t.Data[0] == "value1");
            Assert.IsTrue(t.Data[1] == "value2");
        }
    }
}