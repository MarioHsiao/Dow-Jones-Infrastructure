using DowJones.Mapping;
using DowJones.Search;

namespace DowJones.Web.Mvc.Search.Requests.Modules
{
    public class ModuleSearchRequestMapper : TypeMapper<ModuleSearchRequest, AbstractSearchQuery>
    {
        private readonly SearchRequestMapper _baseMapper;

        public ModuleSearchRequestMapper(SearchRequestMapper baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public override AbstractSearchQuery Map(ModuleSearchRequest source)
        {
            var query = new ModuleSearchQuery
                            {
                                SearchContext = source.SearchContext,
                            };

            _baseMapper.Map(query, source);

            return query;
        }
    }
}