using DowJones.Search;

namespace DowJones.Managers.Search
{
    public interface ISearchService
    {
        SearchResponse PerformSearch(AbstractBaseSearchQuery request);
    }
} 