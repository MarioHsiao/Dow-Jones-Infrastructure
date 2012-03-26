using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext
{
    public class SourcesViewAllSearchContext : AbstractSearchContext<SourcesViewAllSearchContext>
    {
        [JsonProperty(PropertyName = SearchContextConstants.SourceCode)]
        public string SourceCode;
    }
}
