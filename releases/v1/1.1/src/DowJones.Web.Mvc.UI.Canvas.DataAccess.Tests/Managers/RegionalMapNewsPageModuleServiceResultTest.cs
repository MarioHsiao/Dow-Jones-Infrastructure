// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegionalMapNewsPageModuleDataManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Utilities.Managers.Search;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class RegionalMapNewsPageModuleServiceResultTest : AbstractUnitTest
    {
        private const string PageForGet = "13216";
        private const string ModuleForGet = "22478";

        [TestMethod, TestCategory("Integration")]
        public void PopulateTest()
        {
            var serviceResult = new RegionalMapNewsPageModuleServiceResult();

            var request = new RegionalMapNewsPageModuleDataRequest
            {
                ModuleId = ModuleForGet,
                PageId = PageForGet,
                TimeFrame = TimeFrame.LastMonth,
            };

            serviceResult.Populate(ControlData, request, Preferences);

            SerializationUtility.SerializeObjectToStream(serviceResult);
            Assert.IsTrue(serviceResult.ReturnCode == 0);

            const int Iterations = 2;
            var sum = 0L;
            for (var i = 0; i < Iterations; i++)
            {
                serviceResult.Populate(ControlData, request, Preferences);

                SerializationUtility.SerializeObjectToStream(serviceResult, "XML");

                foreach (var transaction in serviceResult.Audit.TransactionCollection)
                {
                    Console.WriteLine(@"{0}::{1}::{2}", transaction.ElapsedTime, transaction.Name, transaction.Detail);
                }

                if (serviceResult.ReturnCode != 0)
                {
                    continue;
                }

                Console.WriteLine(@"-{0}", serviceResult.ElapsedTime);
                sum = sum + serviceResult.ElapsedTime;
            }

            Console.WriteLine(@"-----Avg: {0} -----", sum / Iterations);
        }
    }
}
