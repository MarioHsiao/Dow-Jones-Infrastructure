using DowJones.Search;

namespace DowJones.Web.Mvc.Search.Requests
{
    public class FreeTextSearchRequest : AdvancedSearchRequest
    {
        public SearchFreeTextArea FreeTextIn { get; set; }
    }
}
