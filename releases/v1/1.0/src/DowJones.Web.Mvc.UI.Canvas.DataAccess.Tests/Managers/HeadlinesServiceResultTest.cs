using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Headlines;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines;
using Factiva.Gateway.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class HeadlinesServiceResultTest : AbstractUnitTest
    {
        [TestMethod]
        public void TrendingItemTest()
        {
            var request = new HeadlinesRequest
            {
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                SearchContextRef = "{\"pn\":\"NewsPages\",\"sct\":\"TrendingItemSearchContext\",\"j\":\"{\\\"tf\\\":-9,\\\"et\\\":2,\\\"tt\\\":0,\\\"c\\\":\\\"c151\\\"}\",\"pid\":\"12622\",\"mid\":\"22248\"}"
            };

            var serviceResult = GetHeadlines(request);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void TopNewsAudioAndVideoViewAllTest()
        {
            var request = new HeadlinesRequest
            {
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                SearchContextRef = "{\"pn\":\"NewsPages\",\"sct\":\"TopNewsViewAllSearchContext\",\"j\":\"{\\\"tnmp\\\":1}\",\"pid\":\"12622\",\"mid\":\"22230\"}"
            };

            var serviceResult = GetHeadlines(request);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void TopNewsOpinionAndAnalysisViewAllTest()
        {
            var request = new HeadlinesRequest
            {
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                SearchContextRef = "{\"pn\":\"NewsPages\",\"sct\":\"TopNewsViewAllSearchContext\",\"j\":\"{\\\"tnmp\\\":2}\",\"pid\":\"12622\",\"mid\":\"22230\"}"
            };

            var serviceResult = GetHeadlines(request);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void TopNewsEditorsChoiceViewAllTest()
        {
            var request = new HeadlinesRequest
            {
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                SearchContextRef = "{\"pn\":\"NewsPages\",\"sct\":\"TopNewsViewAllSearchContext\",\"j\":\"{\\\"tnmp\\\":0}\",\"pid\":\"12622\",\"mid\":\"22230\"}"
            };

            var serviceResult = GetHeadlines(request);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void RegionalMapBubbleTest()
        {
            var request = new HeadlinesRequest
            {
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                SearchContextRef = "{\"pn\":\"NP\",\"sct\":\"RegionalMapBubbleSearchContext\",\"j\":\"{\\\"c\\\":\\\"NAMZ\\\",\\\"tf\\\":-9}\",\"pid\":\"12793\",\"mid\":\"22470\"}"
            };
            ControlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var serviceResult = GetHeadlines(request);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void SourcesViewAllTest()
        {
            var request = new HeadlinesRequest
            {
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                SearchContextRef = "{\"pn\":\"NewsPages\",\"sct\":\"SourcesViewAllSearchContext\",\"j\":\"{\\\"sc\\\":\\\"WFP\\\"}\",\"pid\":\"12622\",\"mid\":\"22227\"}"
            };

            var serviceResult = GetHeadlines(request);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void CustomTopicsViewAllTest()
        {
            var request = new HeadlinesRequest
            {
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                SearchContextRef = "{\"pn\":\"NewsPages\",\"sct\":\"CustomTopicsViewAllSearchContext\",\"j\":\"{\\\"cti\\\":0,\\\"ctn\\\":\\\"Barak Obama\\\"}\",\"pid\":\"12622\",\"mid\":\"22222\"}"
            };

            var serviceResult = GetHeadlines(request);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void RadarCellTest()
        {
            var request = new HeadlinesRequest
            {
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                SearchContextRef = "{\"pn\":\"NewsPages\",\"sct\":\"RadarCellSearchContext\",\"j\":\"{\\\"cc\\\":\\\"MCROST\\\",\\\"nsc\\\":\\\"ccat\\\",\\\"tf\\\":0}\",\"pid\":\"12622\",\"mid\":\"22903\"}"
            };

            var serviceResult = GetHeadlines(request);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void NewsstandViewAllTest()
        {
            var request = new HeadlinesRequest
            {
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                //SearchContextRef = "{\"pn\":\"NewsPages\",\"sct\":\"NewsstandSectionSearchContext\",\"j\":\"{\\\"sid\\\":2}\",\"pid\":\"12622\",\"mid\":\"22225\"}"
                SearchContextRef = "{\"pn\":\"NP\",\"sct\":\"NewsstandSectionSearchContext\",\"j\":\"{\\\"sid\\\":2}\",\"pid\":\"\",\"mid\":\"\"}"
            };

            var serviceResult = GetHeadlines(request);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void NewsstandDiscoveredEntitiesTest()
        {
            var request = new HeadlinesRequest
            {
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                SearchContextRef = "{\"pn\":\"NewsPages\",\"sct\":\"NewsstandDiscoveredEntitiesSearchContext\",\"j\":\"{\\\"et\\\":5,\\\"c\\\":\\\"npag\\\"}\",\"pid\":\"12622\",\"mid\":\"22225\"}"
            };

            var serviceResult = GetHeadlines(request);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void CompanyOverviewTrendingBubbleTest()
        {
            var request = new HeadlinesRequest
            {
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                SearchContextRef = "{\"pn\":\"NewsPages\",\"sct\":\"CompanyOverviewTrendingBubbleSearchContext\",\"j\":\"{\\\"kw\\\":\\\"business units\\\",\\\"ucdr\\\":false,\\\"sd\\\":null,\\\"ed\\\":null}\",\"pid\":\"12622\",\"mid\":\"22226\"}"
            };

            var serviceResult = GetHeadlines(request);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void CompanyOverviewRecentArticlesViewAll()
        {
            var request = new HeadlinesRequest
            {
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                SearchContextRef = "{\"pn\":\"NewsPages\",\"sct\":\"CompanyOverviewRecentArticlesViewAllSearchContext\",\"j\":\"{\\\"ucdr\\\":false,\\\"sd\\\":null,\\\"ed\\\":null}\",\"pid\":\"12622\",\"mid\":\"22226\"}"
            };

            var serviceResult = GetHeadlines(request);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void SummaryTrending()
        {
            var request = new HeadlinesRequest
            {
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                //SearchContextRef = "{\"pn\":\"NewsPages\",\"sct\":\"SummaryTrendingSearchContext\",\"j\":\"{\\\"et\\\":5,\\\"c\\\":\\\"gcat\\\"}\",\"pid\":\"12869\",\"mid\":\"22932\"}"
                SearchContextRef = "{\"pn\":\"NP\",\"sct\":\"SummaryTrendingSearchContext\",\"j\":\"{\\\"et\\\":9,\\\"c\\\":\\\"central bank\\\"}\",\"pid\":\"13167\",\"mid\":\"22479\"}"
            };
            ControlData = ControlDataManager.GetLightWeightUserControlData("snapshot5", "passwd", "16");

            var serviceResult = GetHeadlines(request);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        [TestMethod]
        public void SummaryRegionalMapBubble()
        {
            var request = new HeadlinesRequest
            {
                FirstResultToReturn = 0,
                MaxResultsToReturn = 10,
                SearchContextRef = "{\"pn\":\"NP\",\"sct\":\"SummaryRegionalMapBubbleSearchContext\",\"j\":\"{\\\"c\\\":\\\"EURZ\\\"}\",\"pid\":\"12793\",\"mid\":\"22463\"}"
            };

            ControlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            var serviceResult = GetHeadlines(request);
            Assert.IsTrue(serviceResult.ReturnCode == 0);
        }

        private PortalHeadlinesServiceResult GetHeadlines(HeadlinesRequest request)
        {
            var serviceResult = new PortalHeadlinesServiceResult();
            serviceResult.Populate(ControlData, request, Preferences);
            SerializationUtility.SerializeObjectToStream(serviceResult);
            return serviceResult;
        }
    }
}
