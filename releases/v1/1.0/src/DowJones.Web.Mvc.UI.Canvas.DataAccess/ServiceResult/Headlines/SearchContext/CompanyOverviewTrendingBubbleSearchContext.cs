using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext
{
    public class CompanyOverviewTrendingBubbleSearchContext : AbstractSearchContext<CompanyOverviewTrendingBubbleSearchContext>
    {
        [JsonProperty(PropertyName = SearchContextConstants.Keyword)]
        public string Keyword;

        [JsonProperty(PropertyName = SearchContextConstants.UseCustomDateRange)]
        public bool UseCustomDateRange;

        [JsonProperty(PropertyName = SearchContextConstants.StartDate)]
        public string StartDate;

        [JsonProperty(PropertyName = SearchContextConstants.EndDate)]
        public string EndDate;
    }
}
