using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext
{
    public class CustomTopicsViewAllSearchContext : AbstractSearchContext<CustomTopicsViewAllSearchContext>
    {
        [JsonProperty(PropertyName = SearchContextConstants.CustomTopicIndex)]
        public int CustomTopicIndex;

        [JsonProperty(PropertyName = SearchContextConstants.CustomTopicName)]
        public string CustomTopicName;
    }
}
