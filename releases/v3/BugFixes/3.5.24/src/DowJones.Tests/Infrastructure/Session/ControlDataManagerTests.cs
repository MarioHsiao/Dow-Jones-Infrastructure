using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DowJones.Session;

using GatewayControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Infrastructure.Session
{
    [TestClass]
    public class ControlDataManagerTests : UnitTestFixture
    {
        [TestMethod]
        public void ShouldPopulateMetricsValuesWhenConvertingFromDomainToGateway()
        {
            const string MetricKey = "Test1";
            const string MetricValue = "Value1";

            var domainControlData = new ControlData
                                        {
                                            Metrics = new Dictionary<string, string>
                                                          {
                                                              { MetricKey, MetricValue }
                                                          }
                                        };

            var gatewayControlData = ControlDataManager.Convert(domainControlData);

            Assert.AreEqual(MetricValue, gatewayControlData.GetValue(MetricKey));
        }
    }
}