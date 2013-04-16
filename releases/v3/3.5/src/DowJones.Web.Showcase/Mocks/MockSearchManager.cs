using System.Linq;
using DowJones.Ajax.HeadlineList;
using DowJones.Assemblers.Headlines;
using DowJones.Managers.Search;
using DowJones.Search;
using DowJones.Search.Navigation;
using Factiva.Gateway.Messages.Search.V2_0;
using Keyword = DowJones.Search.Navigation.Keyword;
using Navigator = DowJones.Search.Navigation.Navigator;

namespace DowJones.Web.Showcase.Mocks
{
    public class MockSearchManager : ISearchService
    {
        private static volatile SyndicationDataResult _cachedResponse;

        private readonly SyndicationManager _feedReader;

        public MockSearchManager(SyndicationManager feedReader)
        {
            _feedReader = feedReader;
        }

        public SearchResponse PerformSearch(AbstractBaseSearchQuery query)
        {
            AbstractSearchQuery searchQuery = query as AbstractSearchQuery;
            if (_cachedResponse == null)
            {
                _cachedResponse =
                    _feedReader
                        .GetFeeds(new[] { "http://search.twitter.com/search.atom?q=obama" })
                        .Single();
            }

            return new SearchResponse
                       {
                           ContentServerAddress = 1,
                           ContextId = "MOCK",
                           Navigators = new ResultNavigator(new SourceGroups(), Enumerable.Empty<Navigator>(), Enumerable.Empty<Keyword>(), new CompositeNavigatorGroup()),
                           Response = new PerformContentSearchResponse(),
                           Results = _cachedResponse.result,
                           Query = searchQuery,
                       };
        }
    }
}