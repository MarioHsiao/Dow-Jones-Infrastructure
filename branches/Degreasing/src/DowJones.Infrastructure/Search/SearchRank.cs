using System.Collections.Generic;

namespace DowJones.Search
{
    public class SearchRank
    {
        public IEnumerable<string> Up { get; set; }

        public IEnumerable<string> Down { get; set; }
    }
}