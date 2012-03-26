// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TopNewsNewsPageModuleServiceResultTest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class TopNewsNewsPageModuleServiceResultTest : AbstractUnitTest
    {
        private const string PageForGet = "13216";
        private const string ModuleForGet = "22472";

        [TestMethod]
        public void PopulateTest()
        {
            var serviceResult = new TopNewsNewsPageModuleServiceResult();

            var a = new TopNewsNewsPageModuleDataRequest
                        {
                            PageId = PageForGet,
                            ModuleId = ModuleForGet,
                            FirstResultToReturn = 0,
                            MaxResultsToReturn = 5,
                            Parts = new List<TopNewsModulePart>{TopNewsModulePart.VideoAndAudio, TopNewsModulePart.EditorsChoice, TopNewsModulePart.VideoAndAudio}
                        };

            serviceResult.Populate(ControlData, a, Preferences);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
            SerializationUtility.SerializeObjectToStream(serviceResult);

            var partResults = serviceResult.PartResults;
            Assert.IsTrue(partResults.Count() > 0, "PartResults count is > 0");
            Assert.AreEqual(partResults.Count(), 3, "Three PartResults");
            serviceResult.Populate(ControlData, a, Preferences);
        }
    }
}
