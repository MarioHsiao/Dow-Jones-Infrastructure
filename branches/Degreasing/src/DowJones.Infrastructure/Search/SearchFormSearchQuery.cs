namespace DowJones.Search
{
    public class SearchFormSearchQuery : AdvancedSearchQuery
    {
        public string Keywords { get; set; }

        public string AnyWords { get; set; }

        public string ExactPhrase { get; set; }

        public string NotWords { get; set; }

        public override bool IsValid()
        {
            //TODO: Validate filters
            return true;
        }
    }
}