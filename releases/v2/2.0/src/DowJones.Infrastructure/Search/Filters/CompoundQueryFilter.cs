using System.Collections.Generic;
using DowJones.Search.Core;

namespace DowJones.Search
{
    public class CompoundQueryFilter : IQueryFilter
    {
        public const SearchOperator DefaultOperator = SearchOperator.Or;


        public IEnumerable<IQueryFilter> Exclude { get; set; }

        public IEnumerable<IQueryFilter> Include { get; set; }

        public SearchOperator Operator { get; set; }

        public string ListId { get; set; }

        public string ListName { get; set; }

        public CompoundQueryListType ListType { get; set; }

        public CompoundQueryFilter()
        {
            Operator = DefaultOperator;
        }
    }

    public enum CompoundQueryListType
    {
        Unknown,
        Source,
        Company
    }
}