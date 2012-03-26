using DowJones.Utilities.Managers.Search;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext
{
    public class RadarCellSearchContext : AbstractSearchContext<RadarCellSearchContext>
    {
        [JsonProperty(PropertyName = SearchContextConstants.CompanyCode)]
        public string CompanyCode;

        [JsonProperty(PropertyName = SearchContextConstants.NewsSubjectCode)]
        public string NewsSubjectCode;

        [JsonProperty(PropertyName = SearchContextConstants.TimeFrame)]
        public TimeFrame TimeFrame;
    }
}
