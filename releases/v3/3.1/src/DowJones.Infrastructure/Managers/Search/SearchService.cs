using System.Linq;
using DowJones.Managers.Alert;
using DowJones.Search;
using DowJones.Search.Navigation;
using DowJones.Token;

namespace DowJones.Managers.Search
{
    /// <summary>
    /// Composite Search Service that acts as a facade
    /// to handle all Search Queries
    /// </summary>
    public class SearchService : ISearchService
    {
        private readonly IContentSearchService _contentSearchService;
        private readonly IAlertSearchService _alertSearchService;
        private readonly ITokenRegistry _tokenRegistry;

        public SearchService(IContentSearchService contentSearchService, IAlertSearchService alertSearchService, ITokenRegistry tokenRegistry)
        {
            _contentSearchService = contentSearchService;
            _alertSearchService = alertSearchService;
            _tokenRegistry = tokenRegistry;
        }

        public SearchResponse PerformSearch(AbstractBaseSearchQuery request)
        {
            ISearchService searchService = _contentSearchService;

            if (request is AlertSearchQuery)
            { 
                return PerformSearch((AlertSearchQuery)request);
            }
            
            var response = searchService.PerformSearch(request);
            if ( !(request is ModuleSearchQuery) )
            {
                AddStaticNavigators(response);
            }
            return response;
        }

        SearchResponse PerformSearch(AlertSearchQuery request)
        {
            return _alertSearchService.PerformSearch(request);
        }

        private void AddStaticNavigators(SearchResponse response)
        {
            if (response == null || response.Navigators == null || response.Navigators.SourceGroups == null) 
                return;

            var staticSourceGroups = new [] {
                    new { Name = _tokenRegistry.Get("authors"), Code = "author" },
                    new { Name = _tokenRegistry.Get("outlets"), Code = "outlet" },
                };

            var staticNavigators = staticSourceGroups.Select(x => new NavigatorGroup {Code = x.Code, Name = x.Name});
            response.Navigators.SourceGroups.AddRange(staticNavigators);
        }
    }
}