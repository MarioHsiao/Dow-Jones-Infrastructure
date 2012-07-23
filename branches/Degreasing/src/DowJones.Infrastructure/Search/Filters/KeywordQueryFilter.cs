using System.Collections.Generic;
using System.Linq;

namespace DowJones.Search
{
    public class KeywordQueryFilter : List<string>, IQueryFilter
    {
        public KeywordQueryFilter()
        {
        }

        public KeywordQueryFilter(string keyword)
            : base(new [] { keyword })
        {
        }

        public KeywordQueryFilter(IEnumerable<string> keywords)
            : base(keywords ?? Enumerable.Empty<string>())
        {
        }
    }
}