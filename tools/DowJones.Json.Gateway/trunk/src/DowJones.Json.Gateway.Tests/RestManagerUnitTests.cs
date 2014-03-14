using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Extentions;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Messages.Core;
using DowJones.Json.Gateway.Tests.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Json.Gateway.Tests
{
    [TestClass]
    public class RestManagerUnitTests : AbstractUnitTests
    {

        [TestMethod]
        public void ExecuteBasic()
        {
            var r = new RestRequest<GetPageByName>
                    {
                        Request = new GetPageByName { Name = "new", Type = "DJXMaster" },
                        ControlData = GetControlData()
                    };

            var rm = new RestManager();
            
            var t = rm.Execute<GetPageByName, GetPageByNameResponse>(r);
           

          /*  
            Assert.IsNotNull(t.Data);
            Assert.IsInstanceOfType(t.Data, typeof(List<string>));
            Assert.IsTrue(t.Data.Count == 2);
            Assert.IsTrue(t.Data[0] == "value1");
            Assert.IsTrue(t.Data[1] == "value2");           
          */
        }

        [TestMethod]
        public void TestServicePath()
        {
            var test = new GetPageByName();
        }
    }

    [ServicePath("pamapi/1.0/DJXPages.svc")]
    [DataContract(Name = "GetPageByName")]
    public class GetPageByName : IGetJsonRestRequest
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    public class GetPageByNameResponse : IJsonRestResponse
    {
    }
}