namespace DowJones.Web.Mvc.Search.Requests
{
    public class SearchFormSearchRequest: AdvancedSearchRequest
    {   
        public string AnyWords { get; set; }

        public string ExactPhrase { get; set; }

        public string NotWords { get; set; }
    }
}