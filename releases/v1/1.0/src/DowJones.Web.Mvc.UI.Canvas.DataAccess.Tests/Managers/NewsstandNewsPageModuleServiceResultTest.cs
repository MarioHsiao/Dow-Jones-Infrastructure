// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsstandNewsPageModuleDataManager.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the NewsstandNewsPageModuleDataManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlDataManager = Factiva.Gateway.Managers.ControlDataManager;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class NewsstandNewsPageModuleServiceResultTest : AbstractUnitTest
    {
        private const string PageForGet = "13216";
        private const string ModuleForGet = "22475";

        [TestMethod]
        //// TODO:  This needs a better name.  What is it testing?
        public void PopulateTest()
        {
            var serviceResult = new NewsstandNewsPageModuleServiceResult();

            var request = new NewsstandNewsPageModuleDataRequest
            {
                PageId = PageForGet,
                ModuleId = ModuleForGet,
                MaxResultsToReturn = 10,
                Parts = new List<NewsstandPart>
                                              {
                                                   NewsstandPart.DiscoveredEntities,
                                                   NewsstandPart.Counts,
                                                   NewsstandPart.Headlines
                                              }
            };

            serviceResult.Populate(ControlData, request, Preferences);

            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }
    }
}
