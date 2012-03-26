using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext
{
    public class SummaryRegionalMapBubbleSearchContext : AbstractSearchContext<SummaryRegionalMapBubbleSearchContext>
    {
        [JsonProperty(PropertyName = SearchContextConstants.Code)]
        public string Code;
    }
}
