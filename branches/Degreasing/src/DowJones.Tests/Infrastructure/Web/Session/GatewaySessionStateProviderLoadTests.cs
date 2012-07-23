using System;
using System.Web;
using System.Web.SessionState;
using DowJones.Infrastructure.Common;
using DowJones.Session;
using DowJones.Web.Session;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure.Web.Session
{
    [TestClass]
    public class GatewaySessionStateProviderLoadTests : PerformanceTestFixture
    {
        private static readonly Product TestProduct = new Product("TEST", "TEST");

        // TODO: REPLACE THE VALUE OF THIS FOR EVERY RUN
        const string SessionID = "27137ZzZKJHEQUSCAAAGUAIAAAAABDUKAAAAAABSGAYTCMBXGI2TCNBQGU2DCNJX";
        const int TestIterations = 1;


        static IControlData ControlData 
        {
            get { return new ControlData {SessionID = SessionID}; }
        }

        [TestMethod, TestCategory(PerformanceTestCategory)]
        public void RetrieveSessionState()
        {
            var provider = new GatewaySessionStateProvider { Product = TestProduct };

            ExecuteWithTiming(TestIterations, () => {
                bool locked;
                TimeSpan lockAge;
                object lockId;
                SessionStateActions actions;

                provider.ControlData = ControlData;
                var sessionData = provider.GetItem(null, Guid.NewGuid().ToString(), out locked, out lockAge, out lockId, out actions);
                Assert.IsNotNull(sessionData);
            });
        }

        [TestMethod, TestCategory(PerformanceTestCategory)]
        public void SaveSessionState()
        {
            var sessionStateItemCollection = new SessionStateItemCollection();
            sessionStateItemCollection["Test1"] = "Value1";

            SessionStateStoreData sessionStateData = new SessionStateStoreData(sessionStateItemCollection, new HttpStaticObjectsCollection(), 20);

            var provider = new GatewaySessionStateProvider { Product = TestProduct };
            provider.OnSaveError += (sender, args) => { throw new ApplicationException("BOOM"); };

            ExecuteWithTiming(TestIterations, () => {
                provider.ControlData = ControlData;
                provider.SetAndReleaseItemExclusive(null, Guid.NewGuid().ToString(), sessionStateData, 1234, true);
            });
        }
    }
}