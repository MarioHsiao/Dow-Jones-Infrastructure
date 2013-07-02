using DowJones.Prod.X.Models.Search;

namespace DowJones.Prod.X.Core.DataTransferObjects
{
    public class SearchServiceDTO
    {
        public string Query { get; set; }
        public int FirstResult { get; set; }
        public int MaxResults { get; set; }
        public SearchMode SearchMode { get; set; }
        public SortOrder? SortOrder { get; set; }
        public DeduplicationMode? DeduplicationMode { get; set; }
    }
}
