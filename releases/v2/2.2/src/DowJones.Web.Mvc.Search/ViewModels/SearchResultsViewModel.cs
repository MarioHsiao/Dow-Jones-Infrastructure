using DowJones.Converters;
using DowJones.Web.Mvc.Search.UI.Components.Builders;
using DowJones.Web.Mvc.Search.UI.Components.Filters;
using DowJones.Web.Mvc.Search.UI.Components.Results;
using DowJones.Web.Mvc.Search.UI.Components.Results.Headlines;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using DowJones.Web.Mvc.UI.Components.Models.CompanySparkline;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Newtonsoft.Json;
using SortOrder = DowJones.Search.SortOrder;

namespace DowJones.Web.Mvc.Search.Results
{
    [JsonConverter(typeof(GeneralJsonEnumConverter))]
    public enum ResultsLayout
    {
        Full,
        Split,
    } 

    public class SearchResultsOptions
    {                               
        public ResultsLayout? Layout { get; set; }
        
        public DisplayOptions? ArticleDisplayOption { get; set; }
    }  

    public class SearchResultsViewModel
    {
        public SearchBuilder SearchBuilder { get; set; }
                                                           
        public SortOrder? HeadlineSort { get; set; }
        
        public ShowDuplicates? ShowDuplicates { get; set; }

        public CompaniesSparklinesComponentModel CompanyProfiles { get; set; }

        public SearchNavigator Navigator { get; set; }

        public SearchResults Results { get; set; }

        public SearchFilters Filters { get; set; }

        public bool HasResults
        {
            get { return Results != null && Results.HasResults; }
        }

        [ClientProperty("layout")]
        public ResultsLayout Layout { get; set; }

        [ClientProperty("searchResultsUrl")]
        public string SearchResultsServiceUrl { get; set; }

        [ClientProperty("relatedConceptsUrl")]
        public string RelatedConceptsServiceUrl { get; set; }

        public SearchResultsViewModel()
        {
            CompanyProfiles = new CompaniesSparklinesComponentModel();
            Navigator = new SearchNavigator();   
            RelatedConceptsServiceUrl = "/search/related";
            SearchResultsServiceUrl = "/search/results";
        }
    }
}
