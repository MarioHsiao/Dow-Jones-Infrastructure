using DowJones.Session;
using DowJones.Web.Session;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web
{
    [TestClass]
    public class GatewaySessionStateProviderTests : UnitTestFixtureBase<GatewaySessionStateProvider>
    {
        private IControlData _controlData;

        protected GatewaySessionStateProvider Provider
        {
            get { return UnitUnderTest; }
        }

        [TestMethod]
        public void ShouldStoreSerializedSessionData()
        {
//            Provider.SetAndReleaseItemExclusive(null, "Test", new SessionStateStoreData(new SessionStateItemCollection(), new HttpStaticObjectsCollection(), 20));
        }


        protected override GatewaySessionStateProvider CreateUnitUnderTest()
        {
            _controlData = new ControlData {UserID = "joyful", UserPassword = "joyful", ProductID = "16"};

            MockServiceLocator
                .Setup(x => x.Resolve<IControlData>())
                .Returns(_controlData);

            var unitUnderTest = new GatewaySessionStateProvider();
            return unitUnderTest;
        }
    }
}