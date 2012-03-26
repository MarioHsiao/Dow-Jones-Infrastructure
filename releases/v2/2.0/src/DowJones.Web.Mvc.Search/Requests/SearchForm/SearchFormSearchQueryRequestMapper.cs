using DowJones.Mapping;
using DowJones.Search;
using DowJones.Web.Mvc.Search.Requests.Mappers;

namespace DowJones.Web.Mvc.Search.Requests.SearchForm
{
    public class SearchFormSearchQueryRequestMapper : TypeMapper<SearchFormSearchRequest, AbstractSearchQuery>
    {
        private readonly AdvancedSearchRequestMapper _baseMapper;

        public SearchFormSearchQueryRequestMapper(AdvancedSearchRequestMapper baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public override AbstractSearchQuery Map(SearchFormSearchRequest source)
        {
            var query = new SearchFormSearchQuery
                            {
                                AnyWords = source.AnyWords,
                                ExactPhrase = source.ExactPhrase,
                                Keywords = source.FreeText,
                                NotWords = source.NotWords
                            };
            _baseMapper.Map(query, source);

            return query;
        }
    }
}