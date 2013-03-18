using System.Linq;
using DowJones.Assemblers.Search;
using DowJones.Search;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;

namespace DowJones.Web.Mvc.Search.Requests.Simple
{
    public class SimpleSearchRequestToQueryMapper //: TypeMapper<FreeTextSearchRequest, GroupCollection>
    {
        public static GroupCollection Map(SearchRequest source)
        {
            if (source is FreeTextSearchRequest)
            {
                var a = Mapper.Map<SimpleSearchQuery>(source);
                return Mapper.Map<GroupCollection>(a);
            }
            else
            {
                var a = Mapper.Map<SimpleSearchQuery>(source);
                return Mapper.Map<GroupCollection>(a);
            }
        }

        public static SimpleSearchRequest MapSimpleSearchRequest(Query source)
        {
            var searchQueries = Mapper.Map<SimpleSearchQuery>(source);
            return Mapper.Map<SimpleSearchRequest>(searchQueries);
        }
    }
}