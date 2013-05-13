using System.Linq;
using DowJones.Assemblers.Search;
using DowJones.Search;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;

namespace DowJones.Web.Mvc.Search.Requests.Freetext
{
    public class FreeTextSearchRequestToQueryMapper //: TypeMapper<FreeTextSearchRequest, GroupCollection>
    {
        public static GroupCollection Map(SearchRequest source)
        {
            if (source is FreeTextSearchRequest)
            {
                var a = Mapper.Map<FreeTextSearchQuery>(source);
                return Mapper.Map<GroupCollection>(a);
            }
            else
            {
                var a = Mapper.Map<SimpleSearchQuery>(source);
                return Mapper.Map<GroupCollection>(a);
            }
        }

        public static FreeTextSearchRequest MapFreeTextSearchRequest(Query source)
        {
            var searchQueries = Mapper.Map<FreeTextSearchQuery>(source);
            return Mapper.Map<FreeTextSearchRequest>(searchQueries);
        }
    }
}