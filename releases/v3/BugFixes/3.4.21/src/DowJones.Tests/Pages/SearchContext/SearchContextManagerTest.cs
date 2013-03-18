//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using DowJones.Managers.Search.Requests;
//using DowJones.Session;
//using Factiva.Gateway.Messages.Search.FreeSearch.V1_0;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace DowJones.Pages.SearchContext
//{
//    [TestClass]
//    public class SearchContextManagerTest : AbstractUnitTest
//    {
//        [TestMethod]
//        public void GetSearchRequestTest()
//        {
//            var controlData = new ControlData
//                                  {
//                                      SessionID = "27139ZzZKJHEQUSCAAAGUAYAAAAADEPKAAAAAABSGAYTCMJQGEZDCMRSGQZTKMRU",
//                                      //UserID = "apichecker",
//                                      //UserPassword = "apichecker",
//                                      //ProductID = "16"
//                                  };
//            var manager = new SearchContextManager(controlData, Preferences);
//            var searchContextString = "{\"pn\":\"NP\",\"sct\":\"CompanyOverviewTrendingBubbleSearchContext\",\"j\":\"{\\\"kw\\\":\\\"network media\\\",\\\"ucdr\\\":false,\\\"sd\\\":null,\\\"ed\\\":null}\",\"pid\":\"Np_V1_13596_13596_000000\",\"mid\":\"23188\"}";
//            var searchRequest1 = manager.CreateCodedStructuredSearchRequest(searchContextString, 0, 10);

//            searchContextString = "{\"pn\":\"NP\",\"sct\":\"CustomTopicsViewAllSearchContext\",\"j\":\"{\\\"cti\\\":1,\\\"ctn\\\":\\\"Pittsburgh Steelers\\\"}\",\"pid\":\"Np_V1_13596_13596_000000\",\"mid\":\"23181\"}";
//            var searchRequest2 = manager.CreateSearchRequest<PerformContentSearchRequest>(searchContextString, 0, 10);

//            searchContextString = "{\"pn\":\"NP\",\"sct\":\"NewsstandDiscoveredEntitiesSearchContext\",\"j\":\"{\\\"et\\\":3,\\\"c\\\":\\\"reut\\\"}\",\"pid\":\"Np_V1_13596_13596_000000\",\"mid\":\"23187\"}";
//            var searchRequest3 = manager.CreateNewsstandHeadlinesRequest(searchContextString, 0, 10);
//        }

//        [TestMethod]
//        public void GetHeadlinesTest()
//        {
//            var controlData = new ControlData
//            {
//                SessionID = "27139ZzZKJHEQUSCAAAGUAYAAAAADEH4AAAAAABSGAYTCMJQGEZDAOJRHE2TQMJY",
//                //UserID = "apichecker",
//                //UserPassword = "apichecker",
//                //ProductID = "16"
//            };
//            var manager = new SearchContextManager(controlData, Preferences);
//            var searchContextString = "{\"pn\":\"NP\",\"sct\":\"CompanyOverviewTrendingBubbleSearchContext\",\"j\":\"{\\\"kw\\\":\\\"network media\\\",\\\"ucdr\\\":false,\\\"sd\\\":null,\\\"ed\\\":null}\",\"pid\":\"Np_V1_13596_13596_000000\",\"mid\":\"23188\"}";
//            var headlines1 = manager.GetHeadlines(searchContextString, 0, 10);

//            searchContextString = "{\"pn\":\"NP\",\"sct\":\"CustomTopicsViewAllSearchContext\",\"j\":\"{\\\"cti\\\":1,\\\"ctn\\\":\\\"Pittsburgh Steelers\\\"}\",\"pid\":\"Np_V1_13596_13596_000000\",\"mid\":\"23181\"}";
//            var headlines2 = manager.GetHeadlines(searchContextString, 0, 10);

//            searchContextString = "{\"pn\":\"NP\",\"sct\":\"NewsstandDiscoveredEntitiesSearchContext\",\"j\":\"{\\\"et\\\":3,\\\"c\\\":\\\"reut\\\"}\",\"pid\":\"Np_V1_13596_13596_000000\",\"mid\":\"23187\"}";
//            var headlines3 = manager.GetHeadlines(searchContextString, 0, 10);
//        }
//    }
//}
