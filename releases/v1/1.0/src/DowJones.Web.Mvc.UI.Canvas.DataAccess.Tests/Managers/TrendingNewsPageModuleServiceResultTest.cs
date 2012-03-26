// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrendingNewsPageModuleManagerTest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using DowJones.Utilities.Managers.Search;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityType = DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.EntityType;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    /// <summary>
    /// The trending news page module service result test.
    /// </summary>
    [TestClass]
    public class TrendingNewsPageModuleServiceResultTest : AbstractUnitTest
    {
        private const string PageForGet = "13216";
        private const string ModuleForGet = "22474";

        [TestMethod, TestCategory("Integration")]
        public void PopulateTest()
        {
            var request = new TrendingNewsPageModuleRequest
                              {
                                  PageId = PageForGet,
                                  ModuleId = ModuleForGet, 
                                  EntityType = EntityType.Subjects,
                                  TimeFrame = TimeFrame.LastWeek,
                                  Parts = new List<TrendType>
                                              {
                                                  TrendType.TopEntities,
                                                  TrendType.TrendingUp,
                                                  TrendType.TrendingDown
                                              },
                              };

            var serviceResult = new TrendingNewsPageModuleServiceResult();

            serviceResult.Populate(ControlData, request, Preferences);
            SerializationUtility.SerializeObjectToStream(serviceResult);

            Assert.AreEqual(serviceResult.PartResults.Count(), 3, "There should be 3 PartResults");
            var topNewsPartResult = (from result in serviceResult.PartResults
                                     where result.PackageType == typeof(TrendingTopEntitiesPackage).Name
                                     select result).ToList();

            Assert.AreEqual(topNewsPartResult.Count, 1, "There should be only one Top News PartResult");
            Assert.AreEqual(topNewsPartResult.First().Package.TopNewsVolumeEntities.Count, 5, "There should be 5 companies for top news");

            foreach (var entity in topNewsPartResult.First().Package.TopNewsVolumeEntities)
            {
                Assert.IsNotNull(entity.Code);
                Assert.IsNotNull(entity.Descriptor);
                Assert.IsNotNull(entity.Type);
            }

            var trendingUpPartResult = (from result in serviceResult.PartResults
                                        where result.PackageType == typeof(TrendingUpPackage).Name
                                        select result).ToList();
            Assert.AreEqual(trendingUpPartResult.Count, 1);
           // Assert.AreEqual(5, trendingUpPartResult.First().Package.TrendingEntities.Count, "There should be 5 entities trending up");

            foreach (var entity in trendingUpPartResult.First().Package.TrendingEntities)
            {
                Assert.IsNotNull(entity.Code);
                Assert.IsNotNull(entity.Descriptor);
                Assert.IsNotNull(entity.Type);
                Assert.IsTrue(entity.CurrentTimeFrameNewsVolume.Value > entity.PreviousTimeFrameNewsVolume.Value);
            }

            var trendingDownPartResults = (from result in serviceResult.PartResults
                                           where result.PackageType == typeof(TrendingDownPackage).Name
                                            select result).ToList();

            Assert.AreEqual(trendingDownPartResults.Count, 1);
            //Assert.AreEqual(trendingDownPartResults.First().Package.TrendingEntities.Count, 5);

            foreach (var entity in trendingDownPartResults.First().Package.TrendingEntities)
            {
                Assert.IsNotNull(entity.Code);
                Assert.IsNotNull(entity.Descriptor);
                Assert.IsNotNull(entity.Type);
                Assert.IsTrue(entity.CurrentTimeFrameNewsVolume.Value < entity.PreviousTimeFrameNewsVolume.Value);
            }

            //write the xml
            SerializationUtility.SerializeObjectToStream(serviceResult);
        }
    }
}
