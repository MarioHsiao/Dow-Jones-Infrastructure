using DowJones.Managers.Search;
using DowJones.Managers.Search.Preference;
using DowJones.Managers.SearchContext;
using DowJones.Preferences;
using DowJones.Search;
using DowJones.Session;
using DowJones.Token;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DeduplicationMode = DowJones.Search.DeduplicationMode;

namespace DowJones.Infrastructure.Web.Search
{
    [TestClass]
    public class PerformContentSearchServiceTest : UnitTestFixtureBase<ContentSearchService>
    {
        private IControlData _controlData = new ControlData { UserID = "commsu8", UserPassword = "passwd", ProductID = "16" };
        private ITokenRegistry _tokenRegistry = new Mock<ITokenRegistry>().Object;
        private IPreferences _preferences = new DowJones.Preferences.Preferences("en");

        protected ContentSearchService Provider
        {
            get { return UnitUnderTest; }
        }

        public void PerformSimpleSearch()
        {
           
            var query = new SimpleSearchQuery();
            query.Keywords ="Obama";
            query.DateRange = SearchDateRange.LastSixMonths;
            query.Duplicates = DeduplicationMode.NearExact;
            query.ProductId = "communicator";
            var searchResponse = Provider.PerformSearch(query);

            Assert.IsNotNull(searchResponse);
            System.Diagnostics.Debug.WriteLine(Serialize(searchResponse));
            Assert.IsTrue(searchResponse.Results.hitCount.Value > 0);
        }
       

        protected override ContentSearchService CreateUnitUnderTest()
        {
      
            var searchManager = new SearchManager(_controlData, _preferences);
            var sourceGroupConfigurationManager = new ProductSourceGroupConfigurationManager(_controlData, _tokenRegistry);
            var searchQueryResourceManager = new SearchQueryResourceManager(_controlData, sourceGroupConfigurationManager);
            var builder = new SearchQueryBuilder(_preferences, searchQueryResourceManager, new SearchPreferenceService(), new SearchContextManager(_controlData, _preferences));
            return new ContentSearchService(searchManager, builder);
        }
    }
}
