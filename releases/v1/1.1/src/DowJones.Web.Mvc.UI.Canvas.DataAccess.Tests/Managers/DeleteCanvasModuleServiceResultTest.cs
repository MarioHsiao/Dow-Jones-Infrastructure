using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Create;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Create;
using Factiva.Gateway.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class DeleteCanvasModuleServiceResultTest : AbstractUnitTest
    {
        [TestMethod]
        public void DeleteCanvasModuleTest()
        {
            //First, get page by id and see if we've reached the max number of modules

            var controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var basePreferences = new BasePreferences { InterfaceLanguage = "en" };

            var pageId = 13775.ToString();
            int moduleId;

            var newsPageServiceResult = new NewsPageServiceResult();
            var request = new NewsPageRequest
            {
                PageId = pageId
            };
            newsPageServiceResult.Populate(controlData, request, basePreferences);
            Assert.IsTrue(newsPageServiceResult.ReturnCode == 0);

            var moduleCountBefore = newsPageServiceResult.Package.NewsPage.ModuleCollection.Count;

            if (moduleCountBefore < 10)
            {
                // add a new module
                var testRequest = new AlertsNewsPageModuleCreateRequest
                {
                    Description = "Test Data Description",
                    Title = "Test Data title",
                    PageId = pageId
                };

                var alert = new AlertID();
                alert.AlertId = "300232452";
                alert.IsPrivate = false;
                testRequest.AlertCollection.AddRange(new[] { alert});

                var alertNewsPageModuleCreateServiceResult = new AlertNewsPageModuleCreateServiceResult();
                alertNewsPageModuleCreateServiceResult.Create(controlData, testRequest, basePreferences);
                Assert.IsTrue(alertNewsPageModuleCreateServiceResult.ReturnCode == 0);
                moduleCountBefore++;
                moduleId = alertNewsPageModuleCreateServiceResult.ModuleId;
            }
            else
            {
                moduleId = newsPageServiceResult.Package.NewsPage.ModuleCollection[0].Id;
            }
            Assert.IsTrue(moduleId > 0);

            var serviceResult = new DeleteCanvasModuleServiceResult();
            var serviceRequest = new DeleteCanvasModuleRequest
            {
                PageId = pageId,
                ModuleId = moduleId.ToString()
            };

            serviceResult.DeleteModule(controlData, serviceRequest, basePreferences);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
            
            newsPageServiceResult.Populate(controlData, request, basePreferences);
            Assert.IsTrue(newsPageServiceResult.ReturnCode == 0);

            var moduleCountAfter = newsPageServiceResult.Package.NewsPage.ModuleCollection.Count;
            Assert.IsTrue(moduleCountAfter == moduleCountBefore - 1);
        }
    }
}
