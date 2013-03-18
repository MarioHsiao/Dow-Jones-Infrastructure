using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Globalization;
using DowJones.Infrastructure;
using DowJones.Search.Filters;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;

namespace DowJones.Search
{
    /// <summary>
    /// Base query class with common fields shared between search and track
    /// </summary>
    public abstract class AbstractBaseSearchQuery
    {
        public DeduplicationMode Duplicates { get; set; }

        public SearchQueryFilters Filters { get; set; }

        public SearchQueryPagination Pagination { get; set; }

        public string PrimarySourceGroupId { get; set; }

        public string ProductId { get; set; }

        public string SecondarySourceGroupId { get; set; }

        public SearchRank SourceRanks { get; set; }

        public CustomNavigationSetting CustomNavigation { get; set; }

        protected AbstractBaseSearchQuery()
        {
            Filters = new SearchQueryFilters();
            Pagination = new SearchQueryPagination();
            SourceRanks = new SearchRank();
        }

        public abstract bool IsValid();
    }


    public abstract class AbstractSearchQuery : AbstractBaseSearchQuery
    {
        public IEnumerable<string> ContentLanguages { get; set; }
        
        public uint ContentServerAddress { get; set; }

        public string ContextId { get; set; }

        public SearchDateRange DateRange { get; set; }

        public DateRange CustomDateRange { get; set; }

        public SortOrder Sort { get; set; }

        public IEnumerable<InclusionFilter> Inclusions { get; set; }

        protected AbstractSearchQuery()
        {
            ContentLanguages = Enumerable.Empty<string>();
            Filters = new SearchQueryFilters();
            Pagination = new SearchQueryPagination();
            DateRange = SearchDateRange.LastThreeMonths;
            SourceRanks = new SearchRank();
        }



        public virtual SearchSetupScreen GetSearchSetupScreen()
        {
          return SearchSetupScreen.SimpleSearch;
        }
    }

    public class CustomNavigationSetting
    {
        public bool KeywordNavigator { get; set; }

        public bool TimeNavigator { get; set; }

        public int MaxBuckets { get; set; }

        public bool ReturnCollectionCount { get; set; }

        public IList<string> CustomNavigationId { get; set; }
    }
}
