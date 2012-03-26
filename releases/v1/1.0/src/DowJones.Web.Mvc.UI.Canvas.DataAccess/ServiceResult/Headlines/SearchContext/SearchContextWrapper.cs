using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext
{
    public class SearchContextWrapper
    {
        [JsonProperty(PropertyName = SearchContextConstants.ProductName)]
        public string ProductName = SearchContextConstants.NewspageProductName;

        [JsonProperty(PropertyName = SearchContextConstants.SearchContextType)]
        public string SearchContextType;

        [JsonProperty(PropertyName = SearchContextConstants.Json)]
        public string Json;

        [JsonProperty(PropertyName = SearchContextConstants.PageId)]
        public string PageId;

        [JsonProperty(PropertyName = SearchContextConstants.ModuleId)]
        public string ModuleId;
    }
}
