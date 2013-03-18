using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Infrastructure;
using DowJones.Infrastructure.Common;
using DowJones.Search;
using DowJones.Search.Filters;
using DowJones.Web.Mvc.Search.Requests.Filters;
using DowJones.Web.Mvc.UI.Components.HeadlineList;

namespace DowJones.Web.Mvc.Search.Requests
{
    public class SearchRequestMapper
    {
        protected Product _product { get; private set; }

        private readonly DateTimeFormatter _dateTimeFormatter;

        protected string DateFormat
        {
            get { return _dateTimeFormatter.CultureInfo.DateTimeFormat.ShortDatePattern; }
        }

        public SearchRequestMapper(Product product, DateTimeFormatter dateTimeFormatter)
        {
            _product = product;
            _dateTimeFormatter = dateTimeFormatter;
        }

        public void Map(AbstractSearchQuery query, SearchRequest source)
        {
            Map(((AbstractBaseSearchQuery)query), source);
            query.DateRange = source.DateRange.HasValue ? source.DateRange.Value : SearchDateRange.LastWeek;
            if (!string.IsNullOrEmpty(source.Languages)){
                query.ContentLanguages = source.Languages.Split(',');
            }
            query.ContentServerAddress = source.Server;
            query.ContextId = source.ContextId;
            if (source.DateRange == SearchDateRange.Custom)
            {
                query.CustomDateRange = DateRange.Deserialize(source.StartDate, source.EndDate);
            }
            if (source.Sort.HasValue)
            {
                query.Sort = source.Sort.Value;
            }
            query.Inclusions = source.Inclusions;
        }

        public void Map(AbstractBaseSearchQuery query, SearchRequest source)
        {
            query.Duplicates = (source.ShowDuplicates.HasValue && source.ShowDuplicates.Value == ShowDuplicates.On) ? DeduplicationMode.Similar : DeduplicationMode.Off;

            query.Filters = Mapper.Map<QueryFilters, SearchQueryFilters>(source.Filters ?? new QueryFilters());
            query.Pagination = new SearchQueryPagination { StartIndex = source.Start, MaxResults = source.PageSize };
            query.PrimarySourceGroupId = source.PrimaryGroup;
            query.ProductId = _product.SourceGroupConfigurationId;
            query.SecondarySourceGroupId = source.SecondaryGroup;
            query.SourceRanks = new SearchRank { Down = source.RankDown, Up = source.RankUp };
        }

        public void Map(SearchRequest searchRequest, AbstractBaseSearchQuery source)
        {
            searchRequest.ShowDuplicates = (source.Duplicates == DeduplicationMode.Off) ? ShowDuplicates.Off : ShowDuplicates.On;

            searchRequest.Filters = Mapper.Map<SearchQueryFilters, QueryFilters>(source.Filters ?? new SearchQueryFilters()); ;
            searchRequest.Start = source.Pagination.StartIndex;
            searchRequest.PageSize = source.Pagination.MaxResults;
            searchRequest.PrimaryGroup = source.PrimarySourceGroupId;
            searchRequest.SecondaryGroup = source.SecondarySourceGroupId;
            if (source.SourceRanks != null)
            {
                searchRequest.RankDown = source.SourceRanks.Down;
                searchRequest.RankUp = source.SourceRanks.Up;
            }
        }

        public void Map(SearchRequest searchRequest, AbstractSearchQuery source)
        {
            this.Map(searchRequest, (AbstractBaseSearchQuery)source);
            searchRequest.Server = source.ContentServerAddress;
            if (source.ContentLanguages != null && !source.ContentLanguages.IsEmpty())
            {
                searchRequest.Languages = string.Join(",", source.ContentLanguages);
            }
            searchRequest.ContextId = source.ContextId;
            searchRequest.DateRange = source.DateRange;
            if (source.DateRange == SearchDateRange.Custom && source.CustomDateRange != null)
            {
                searchRequest.StartDate = DateRange.Serialize(source.CustomDateRange.Start);
                searchRequest.EndDate = DateRange.Serialize(source.CustomDateRange.End);
            }
            searchRequest.ShowDuplicates = (source.Duplicates == DeduplicationMode.Off) ? ShowDuplicates.Off : ShowDuplicates.On;

            searchRequest.Sort = source.Sort;
            searchRequest.Inclusions = source.Inclusions;
        }
    }
}
