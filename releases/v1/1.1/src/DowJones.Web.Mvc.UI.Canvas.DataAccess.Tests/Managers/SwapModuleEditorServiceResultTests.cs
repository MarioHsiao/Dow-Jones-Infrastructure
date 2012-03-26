using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DowJones.Web.Mvc.UI.Models.NewsPages;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class SwapModuleEditorServiceResultTests : AbstractUnitTest
    {
        [TestMethod]
        public void ShouldReturnInvalidGetRequestCodeWhenRequestIsNotValid()
        {
            var request = new Mock<SwapModuleEditorRequest>();
            
            request
                .Setup(x => x.IsValid())
                .Returns(false);

            var result = new SwapModuleEditorServiceResult();
         
            result.Populate(new ControlData(), request.Object, new BasePreferences());
            
            Assert.IsNull(result.Package);
            Assert.AreEqual(result.ReturnCode, DowJonesUtilitiesException.InvalidGetRequest);
        }

        [TestMethod]
        public void TestPopulateWithTopNewsModule()
        {
            var serviceResult = new SwapModuleEditorServiceResult();

            var serviceRequest = new SwapModuleEditorRequest
                                     {
                                         ModuleType = ModuleType.TopNewsNewspageModule
                                     };

            serviceResult.Populate(ControlData, serviceRequest, Preferences);
            Assert.AreEqual(serviceResult.ReturnCode, 0);

            SerializationUtility.SerializeObjectToStream(serviceResult, "XML");
            SerializationUtility.SerializeObjectToStream(serviceResult, "JSON");
        }
    }
}
