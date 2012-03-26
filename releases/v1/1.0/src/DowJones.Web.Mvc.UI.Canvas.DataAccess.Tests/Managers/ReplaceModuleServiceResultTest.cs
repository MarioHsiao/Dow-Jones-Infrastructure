using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Page;
using Factiva.Gateway.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class ReplaceModuleServiceResultTest : AbstractUnitTest
    {
        [TestMethod]
        public void ReplaceModuleTest()
        {
            var request = new ReplaceModuleRequest
                              {
                                  ModuleIdToAdd = "22472",
                                  ModuleIdToRemove = "22586",
                                  PageId = "13357"
                              };
            ControlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var result = new ReplaceModuleServiceResult();
            result.ReplaceModule(ControlData, request, Preferences);
            SerializationUtility.SerializeObjectToStream(result);
        }

        [TestMethod]
        public void ReplaceModuleTest1()
        {
            var request = new ReplaceModuleRequest
            {
                ModuleIdToAdd = "22586",
                ModuleIdToRemove = "22472",
                PageId = "13357"
            };
            ControlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var result = new ReplaceModuleServiceResult();
            result.ReplaceModule(ControlData, request, Preferences);
            SerializationUtility.SerializeObjectToStream(result);
        }
    }
}
