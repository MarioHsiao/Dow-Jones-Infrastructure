using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext
{
    public class TopNewsViewAllSearchContext : AbstractSearchContext<TopNewsViewAllSearchContext>
    {
        [JsonProperty(PropertyName = SearchContextConstants.TopNewsModulePart)]
        public TopNewsModulePart TopNewsModulePart;
    }
}
