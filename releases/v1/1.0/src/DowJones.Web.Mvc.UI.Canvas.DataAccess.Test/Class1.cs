// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Class1.cs" company="Dow Jones">
//   Dow Jones Incorporate
// </copyright>
// <summary>
//   Defines the SyndicationNewsPageModuleManagerTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.NewsPages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.PortalHeadlines;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using Factiva.Gateway.Tests;
using Factiva.Gateway.Utils.V1_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Test
{
    /// <summary>
    /// Test class for SyndicationNewsPageModuleManager
    /// </summary>
    [TestClass]
    public class SyndicationNewsPageModuleManagerTest : AbstractUnitTest
    {
        /// <summary>
        /// GetFeeds is a unit test for getting rss feeds
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void GetFeeds()
        {
            var manager = new SyndicationNewsPageModuleManager<SyndicationNewsPageModuleServiceResult<SyndicationNewsPageModuleServicePartResult<SyndicationPortalPackage>, SyndicationPortalPackage>, GenericNewsPageModuleDataRequest, SyndicationNewsPageModuleServicePartResult<SyndicationPortalPackage>, SyndicationPortalPackage>(new ControlData(), "en");

            var request = new GenericNewsPageModuleDataRequest
                              {
                                  ModuleID = 345.ToString(),
                                  PageID = 123.ToString(),
                                  NumberOfHeadlines = 500,
                                  ResultPageIndex = 1,
                                  ResultSize = 6
                              };
            var serviceResult = manager.GetData(request);
            var sum = 0l;
            const int Iterations = 60;
            for (var i = 0; i < Iterations; i++)
            {
                serviceResult = manager.GetData(request);
                Console.WriteLine(serviceResult.ElapsedTime);
                sum = sum + serviceResult.ElapsedTime;
                if (serviceResult.ElapsedTime > 500)
                {
                    foreach (var partResult in serviceResult.PartResults)
                    {
                        Console.WriteLine(@"	{0} - {1}", partResult.ElapsedTime, partResult.Package.FeedUri);
                        Console.WriteLine(@"-------------------------------");
                    }
                }

                Console.WriteLine(@"-------------------------------");
            }

            Console.WriteLine(@"============");
            Console.WriteLine(sum / Iterations);

            Assert.IsTrue(serviceResult.PartResults.Count() > 0);
            Assert.IsTrue(serviceResult.PartResults.First().ReturnCode == 0);
            Assert.IsTrue(serviceResult.PartResults.First().Package.Result.HitCount.value > 0);
        }
    }
}
