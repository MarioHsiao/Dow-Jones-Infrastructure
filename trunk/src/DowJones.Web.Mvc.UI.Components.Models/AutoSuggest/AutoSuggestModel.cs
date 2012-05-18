namespace DowJones.Web.Mvc.UI.Components.AutoSuggest
{
    public class AutoSuggestModel : ViewComponentModel
    {
        [ClientProperty("suggestServiceUrl")]
        public string SuggestServiceUrl { get; set; }


        [ClientProperty("autocompletionType")]
        public string AutocompletionType { get; set; }

        public AutoSuggestModel()
        {
            SuggestServiceUrl = "http://suggest.factiva.com/Search/1.0";
        }
    }
}