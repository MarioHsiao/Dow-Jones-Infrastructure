using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using Factiva.Gateway.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class AlertListServiceResultTest : AbstractUnitTest
    {
        private const string PageForGet = "13216";
        private const string ModuleForGet = "23233";

        [TestMethod]
        public void PopulateTest()
        {
            var serviceResult = new AlertListServiceResult();

            var request = new BaseModuleRequest
            {
                PageId = PageForGet,
                ModuleId = ModuleForGet,
            };

            serviceResult.Populate(ControlData, request, Preferences);

            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }
    }
}
