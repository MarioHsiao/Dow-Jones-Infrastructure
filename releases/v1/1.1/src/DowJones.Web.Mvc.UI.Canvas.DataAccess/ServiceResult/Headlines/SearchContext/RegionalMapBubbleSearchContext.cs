using DowJones.Utilities.Managers.Search;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext
{
    public class RegionalMapBubbleSearchContext : AbstractSearchContext<RegionalMapBubbleSearchContext>
    {
        [JsonProperty(PropertyName = SearchContextConstants.Code)]
        public string Code;

        [JsonProperty(PropertyName = SearchContextConstants.TimeFrame)]
        public TimeFrame TimeFrame;

    }
}
