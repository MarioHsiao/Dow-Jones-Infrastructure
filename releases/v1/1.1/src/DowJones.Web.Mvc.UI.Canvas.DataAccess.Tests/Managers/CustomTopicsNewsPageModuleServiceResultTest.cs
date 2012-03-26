// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomTopicsNewsPageModuleManagerTest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class CustomTopicsNewsPageModuleServiceResultTest : AbstractUnitTest
    {
        private const string PageForGet = "13216";
        private const string ModuleForGet = "22473";

        [TestMethod]
        public void PopulateTest()
        {
            var serviceResult = new CustomTopicsNewsPageModuleServiceResult();

            var request = new NewsPageHeadlineModuleDataRequest
            {
                PageId = PageForGet,
                ModuleId = ModuleForGet,
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                FirstPartToReturn = 0,
                MaxPartsToReturn = 3
            };

            serviceResult.Populate(ControlData, request, Preferences);

            SerializationUtility.SerializeObjectToStream(serviceResult);

            Console.WriteLine(serviceResult.ReturnCode);
            Assert.IsTrue(serviceResult.ReturnCode == 0);

            var groups = from partResult in serviceResult.PartResults
                        group partResult by partResult.Identifier into results
                         select new { ID = results.Key, Results = results };

            foreach (var group in groups)
            {
                Assert.AreEqual(group.Results.Count(), 1);
            }


            // Get alerts list
            var topicsOnTheModule = serviceResult.GetModule(request, ControlData, Preferences).CustomTopicCollection;

            const int Iterations = 2;
            var sum = 0L;
            for (var i = 0; i < Iterations; i++)
            {
                serviceResult.Populate(ControlData, request, Preferences);
                if (serviceResult.ReturnCode != 0)
                {
                    continue;
                }

                Console.WriteLine(@"-{0}", serviceResult.ElapsedTime);
                sum = sum + serviceResult.ElapsedTime;

                var partIndex = 0;
                foreach (var partResult in serviceResult.PartResults)
                {
                    Assert.AreEqual(topicsOnTheModule[partIndex].Name, partResult.Package.Title, "Output order is incorrect on iteration " + i);
                    if (partResult.ReturnCode == 0)
                    {
                        Console.WriteLine(
                            @"id: {5}-----{0} -- {1} -- HitCount: -- {2} -- Count -- {3} -- Duplicate Count -- {4}  ",
                            partResult.ElapsedTime,
                            partResult.Package.Title,
                            partResult.Package.Result.HitCount.Text.Value,
                            partResult.Package.Result.ResultSet.Count.Text.Value,
                            partResult.Package.Result.ResultSet.DuplicateCount.Text.Value,
                            partResult.Identifier);
                    }
                    partIndex++;
                }
            }

            Console.WriteLine(@"-----Avg: {0} -----", sum / Iterations);
        }
    }
}
