using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext
{
    public class CompanyOverviewRecentArticlesViewAllSearchContext : AbstractSearchContext<CompanyOverviewRecentArticlesViewAllSearchContext>
    {
        [JsonProperty(PropertyName = SearchContextConstants.UseCustomDateRange)]
        public bool UseCustomDateRange;

        [JsonProperty(PropertyName = SearchContextConstants.StartDate)]
        public string StartDate;

        [JsonProperty(PropertyName = SearchContextConstants.EndDate)]
        public string EndDate;
    }
}

