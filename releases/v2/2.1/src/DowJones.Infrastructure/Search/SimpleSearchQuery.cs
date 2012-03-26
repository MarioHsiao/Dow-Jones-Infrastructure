namespace DowJones.Search
{
    public class SimpleSearchQuery : AbstractSearchQuery
    {
        public string Keywords { get; set; }

        public string Source { get; set; }

        public override bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(ProductId))
            {
                return false;
            }
            return (!string.IsNullOrEmpty(Keywords) || (Filters != null && Filters.Count > 0));
        }
    }
}