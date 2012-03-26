using DowJones.Tools.Ajax.HeadlineList;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext
{
    public class SummaryTrendingSearchContext : AbstractSearchContext<SummaryTrendingSearchContext>
    {
        [JsonProperty(PropertyName = SearchContextConstants.EntityType)]
        public EntityType EntityType;

        [JsonProperty(PropertyName = SearchContextConstants.Code)]
        public string Code;

    }
}
