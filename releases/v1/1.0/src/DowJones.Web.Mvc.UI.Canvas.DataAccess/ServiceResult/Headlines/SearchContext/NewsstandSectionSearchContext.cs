using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext
{
    public class NewsstandSectionSearchContext : AbstractSearchContext<NewsstandSectionSearchContext>
    {
        [JsonProperty(PropertyName = SearchContextConstants.SectionId)]
        public int SectionId;

        //[JsonProperty(PropertyName = SearchContextConstants.SourceCode)]
        //public string SourceCode;
    }
}
