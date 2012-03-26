using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using Factiva.Gateway.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class SyndicationItemsServiceResultTest : AbstractUnitTest
    {
        [TestMethod, TestCategory("Integration")]
        public void PopulateTest()
        {
            var serviceResult = new SyndicationItemsServiceResult();

            var request = new BaseModuleRequest
            {
                PageId = 13053.ToString(),
                ModuleId = 22885.ToString(),
            };

            ControlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            serviceResult.Populate(ControlData, request, Preferences);

            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }
    }
}
