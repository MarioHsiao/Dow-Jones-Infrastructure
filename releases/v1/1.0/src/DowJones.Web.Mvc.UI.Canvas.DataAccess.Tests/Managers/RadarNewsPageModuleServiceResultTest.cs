// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RadarModuleServiceResultTest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Managers.Search;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    /// <summary>
    /// The radar module service result test.
    /// </summary>
    [TestClass]
    public class RadarNewsPageModuleServiceResultTest : AbstractUnitTest
    {
        private const string PageForGet = "13216";
        private const string ModuleForGet = "22476";

        [TestMethod]
        public void PopulateTest()
        {
            var request = new RadarNewsPageModuleDataRequest
                              {
                                  PageId = PageForGet,
                                  ModuleId = ModuleForGet, 
                                  TimeFrame = TimeFrame.LastWeek, 
                              };

            var result = new RadarNewsPageModuleServiceResult();
            result.Populate(ControlData, request, Preferences);

            Assert.AreEqual(result.ReturnCode, 0);
            SerializationUtility.SerializeObjectToStream(result);
        }

        [TestMethod]
        public void GetPartsTest()
        {
            var request = new RadarNewsPageModuleDataRequest
            {
                PageId = PageForGet,
                ModuleId = ModuleForGet,
                TimeFrame = TimeFrame.LastWeek,
            };

            var result = new RadarNewsPageModuleServiceResult();
            var pageAssetManager = new PageAssetsManager(ControlData, Preferences);
            var module = pageAssetManager.GetModuleById(ModuleForGet) as RadarNewspageModule;

            var parts = result.GetParts(module, request, ControlData, Preferences);

            Assert.AreEqual(result.ReturnCode, 0);
            SerializationUtility.SerializeObjectToStream(result);
        }
    }
}
