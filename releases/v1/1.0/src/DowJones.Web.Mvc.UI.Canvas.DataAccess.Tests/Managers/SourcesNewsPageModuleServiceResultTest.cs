// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourcesNewsPageModuleManagerTest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using Factiva.Gateway.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class SourcesNewsPageModuleServiceResultTest : AbstractUnitTest// , IPopulateTest, ISampleModule<SourcesNewspageModule>
    {
        private const string PageForGet = "13216";
        private const string ModuleForGet = "22477";
        
        [TestMethod]
        [TestCategory("Integration")]
        public void PopulateTest()
        {
            var serviceResult = new SourcesNewsPageModuleServiceResult();

            var request = new NewsPageHeadlineModuleDataRequest
            {
                PageId = "Not_Specified",
                ModuleId = ModuleForGet,
                MaxResultsToReturn = 5,
                FirstPartToReturn = 0,
                MaxPartsToReturn = 24
            };
            serviceResult.Populate(ControlData, request, Preferences);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
            var xml = SerializationUtility.SerializeObjectToStream(serviceResult);

            ControlData = ControlDataManager.GetLightWeightUserControlData("PCM_proxy", "passwd", "16");
            serviceResult.Populate(ControlData, request, Preferences);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
            var xml1 = SerializationUtility.SerializeObjectToStream(serviceResult);

            // Get source list
            var sourcesOnTheModule = SourcesNewsPageModuleServiceResult.GetSourceListByContentLanguages(serviceResult.GetModule(request, ControlData, Preferences).SourcesListCollection, Preferences);

            const int Iterations = 30;
            var sum = 0L;
            for (var i = 0; i < Iterations; i++)
            {
                serviceResult.Populate(ControlData, request, Preferences);
                if (serviceResult.ReturnCode != 0)
                {
                    Console.WriteLine(@"-{0}-FAILED-{1}", serviceResult.ElapsedTime, serviceResult.ReturnCode);
                    continue;
                }

                Console.WriteLine(@"-{0}", serviceResult.ElapsedTime);
                sum = sum + serviceResult.ElapsedTime;

                var partIndex = 0;
                foreach (var partResult in serviceResult.PartResults)
                {
                    // NN - validating order
                    Assert.AreEqual(sourcesOnTheModule[partIndex], partResult.Package.SourceCode, "Output order is incorrect on iteration " + i);
                    Assert.IsNotNull(partResult, "Part result is not null");
                    Assert.IsNotNull(partResult.Package, "Part result - Package is not null");
                    Assert.IsNotNull(partResult.Package.Result, "Part result - Result is not null");
                    Console.WriteLine(
                          @"id:{6} -- {0} -- {1} -- {2} -- HitCount: -- {3} -- Count -- {4} -- Duplicate Count -- {5}  ",
                          partResult.ElapsedTime,
                          partResult.Package.SourceCode,
                          partResult.Package.SourceName,
                          partResult.Package.Result.HitCount.Text.Value,
                          partResult.Package.Result.ResultSet.Count.Text.Value,
                          partResult.Package.Result.ResultSet.DuplicateCount.Text.Value,
                          partResult.Identifier);
                    partIndex++;
                }
            }

            Console.WriteLine(@"-----Avg: {0} -----", sum / Iterations);
        }
    }
}
