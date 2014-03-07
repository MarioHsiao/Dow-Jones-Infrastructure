using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Json.Gateway.Tests
{
    [TestClass]
    public class RestManagerUnitTests
    {
        [TestMethod]
        public void Execute()
        {

            var r = new RestRequest
                    {
                        Method = Method.GET, 
                        ServerUri = "http://localhost", 
                        ResourcePath = "DowJones.Json.Gateway.Mvc/api/data?type=json"
                    };
            var rm = new RestManager();
            var t = rm.Execute<List<string>>(r);
            Console.Write(t);
            Assert.Fail();
        }
    }
}