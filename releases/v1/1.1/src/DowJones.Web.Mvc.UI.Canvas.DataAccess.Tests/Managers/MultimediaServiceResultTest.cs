using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Multimedia;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Multimedia;
using Factiva.Gateway.Managers;
using DowJones.Session;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class MultimediaServiceResultTest : AbstractUnitTest
    {
        [TestMethod]
        public void MultimediaServiceResult_Populate_Test()
        {
            var serviceResult = new MultimediaServiceResult();

            var controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var basePreferences = new BasePreferences { InterfaceLanguage = "en" };

            var request = new VideoRequest
            {
                AccessionNo = "MMSAJW0020110224e72o00001"
            };

            serviceResult.Populate(controlData, request, basePreferences);
            SerializationUtility.SerializeObjectToStream(serviceResult);

            request = new VideoRequest
            {
                AccessionNo = "MMSAUW0020110120e71k0005l"
            };

            serviceResult.Populate(controlData, request, basePreferences);
            SerializationUtility.SerializeObjectToStream(serviceResult);

            request = new VideoRequest
            {
                AccessionNo = "MMSAJB0020110405e7450025t"
            };

            serviceResult.Populate(controlData, request, basePreferences);
            SerializationUtility.SerializeObjectToStream(serviceResult);

            request = new VideoRequest
            {
                AccessionNo = "MMSAKQ0020110405e74500001"
            };

            serviceResult.Populate(controlData, request, basePreferences);
            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }
    }
}
