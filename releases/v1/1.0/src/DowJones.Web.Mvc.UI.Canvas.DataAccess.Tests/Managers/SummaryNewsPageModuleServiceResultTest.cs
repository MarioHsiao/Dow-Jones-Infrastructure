using System.Collections.Generic;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class SummaryNewsPageModuleServiceResultTest : AbstractUnitTest
    {
        [TestMethod, TestCategory("Integration")]
        public void TestPopulate()
        {
            var result = new SummaryNewsPageModuleServiceResult();

            var request = new SummaryNewsPageModuleDataRequest
            {
                //useCustomDateRange=true&startDate=01032011&endDate=01162011
                PageId = 13053.ToString(),
                ModuleId = 22479.ToString(),
                FirstResultToReturn = 0,
                MaxResultsToReturn = 5,
                MaxEntitiesToReturn = 10,
                Parts = new List<SummaryParts>
                            {
                                SummaryParts.Chart,
                                SummaryParts.RecentArticles,
                                SummaryParts.RecentVideos,
                                SummaryParts.RegionalMap,
                                SummaryParts.Trending
                            }
            };

            ControlData = Factiva.Gateway.Managers.ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            
            result.Populate(ControlData, request, Preferences);

            SerializationUtility.SerializeObjectToStream(result, "XML");
            //SerializationUtility.SerializeObjectToStream(result, "JSON");
        }

        //[TestMethod, TestCategory("Integration")]
        //public void TestGetRegionalMapPart()
        //{

        //    //ControlData = Factiva.Gateway.Managers.ControlDataManager.GetLightWeightUserControlData("snapshot5", "passwd", "16");
        //    var pageAssetManager = new PageAssetsManager(ControlData, Preferences);
        //    var module = pageAssetManager.GetModuleById("22748") as SummaryNewspageModule;

        //    var summaryServiceResult = new SummaryNewsPageModuleServiceResult();
        //    var summaryParts = summaryServiceResult.GetParts(module,
        //                                new SummaryNewsPageModuleDataRequest
        //                                    {
        //                                        Parts = new List<SummaryParts>(new[] { SummaryParts.RegionalMap })
        //                                    },
        //                                ControlData,
        //                                Preferences);

        //    SerializationUtility.SerializeObjectToStream(summaryParts, "XML");
        //    //SerializationUtility.SerializeObjectToStream(result, "JSON");
        //}
    }
}
