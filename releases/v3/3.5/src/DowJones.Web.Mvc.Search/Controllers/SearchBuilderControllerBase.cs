using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Managers.Search;
using DowJones.Search;
using DowJones.Search.Core;
using DowJones.Topic;
using DowJones.Utilities.Search.Core;
using DowJones.Web.Mvc.Search.Requests;
using DowJones.Web.Mvc.Search.Requests.Filters;
using DowJones.Web.Mvc.Search.Requests.Freetext;
using DowJones.Web.Mvc.Search.ViewModels;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using System.Web.Configuration;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using DowJones.Web.Mvc.UI.Components.SearchBuilder;
using DowJones.Web.Mvc.UI.Components.Search;
using QueryFilter = DowJones.Web.Mvc.Search.Requests.Filters.QueryFilter;

namespace DowJones.Web.Mvc.Search.Controllers
{
    public abstract class SearchBuilderControllerBase : ControllerBase
    {
        [Inject("Injecting abstract base class properties")]
        protected internal ProductSourceGroupConfigurationManager ProductSourceGroupConfigurationManager { get; set; }


        public virtual ActionResult Index(FreeTextSearchRequest freeTextRequest, string alertId, string topicId)
        {
            var model = new SearchBuilderViewModel();

            var sourceGroups = GetProductSourceGroups().ToList();
            var searchBuilderData = new SearchBuilderData { SourceGroup = sourceGroups };

            var hasAlertId = alertId.IsNotEmpty();
            var hasFreeTextRequest = Request["freeTextRequest"] != null;  // TODO: Fix MAJOR code smell
            var hasTopicId = topicId.IsNotEmpty();

            if (hasAlertId)
            {
                //Get Alert details and populate search builder data
            }
            else if (hasTopicId)
            {
                Query topicDetails = GetTopicDetails(topicId);
                if (topicDetails != null)
                {
                    freeTextRequest = FreeTextSearchRequestToQueryMapper.MapFreeTextSearchRequest(topicDetails);
                    model.TopicProperties = Mapper.Map<TopicProperties>(topicDetails);
                }
            }

            if (hasFreeTextRequest || hasAlertId || hasTopicId)
            {
                MapRequest(freeTextRequest, searchBuilderData);
            }
            else
            {
                PopulateFromPreferences(searchBuilderData);
            }

            model.SearchBuilder = new SearchBuilderModel
            {
                Preferences = Preferences,
                TaxonomyServiceUrl = WebConfigurationManager.AppSettings["TAXONAMY_REST_URL"],
                QueriesServiceUrl = WebConfigurationManager.AppSettings["QUERIES_REST_URL"],
                Data = searchBuilderData,
                ProductId = Product.SourceGroupConfigurationId
            };

            return View("Index", model);
        }

        protected virtual void MapRequest(FreeTextSearchRequest freeTextRequest, SearchBuilderData sbData)
        {
            sbData.FreeText = freeTextRequest.FreeText;
            sbData.SearchIn = freeTextRequest.FreeTextIn;
            sbData.DateRange = freeTextRequest.DateRange.HasValue ? freeTextRequest.DateRange.Value : SearchDateRange.LastWeek;
            if(!string.IsNullOrEmpty(freeTextRequest.Languages))
            {
                sbData.ContentLanguages = freeTextRequest.Languages.Split(',');
            }
            sbData.Duplicates = (freeTextRequest.ShowDuplicates != ShowDuplicates.Off);
            sbData.ExclusionFilter = freeTextRequest.Exclusions;
            if (freeTextRequest.DateRange == SearchDateRange.Custom)
            {
                sbData.StartDate = freeTextRequest.StartDate;
                sbData.EndDate = freeTextRequest.EndDate;
            }
            sbData.IncludeSocialMedia = (freeTextRequest.Inclusions != null
                                         && freeTextRequest.Inclusions.ToList().Contains(InclusionFilter.SocialMedia));
            if (freeTextRequest.Sort.HasValue)
            {
                sbData.SortBy = freeTextRequest.Sort.Value;
            }

            sbData.Filters = new SearchChannelFilters
            {
                Author = GetFilterListItem(freeTextRequest.Author),
                Company = GetFilterListItem(freeTextRequest.Company),
                Executive = GetFilterListItem(freeTextRequest.Executive),
                Subject = GetFilterListItem(freeTextRequest.Subject),
                Industry = GetFilterListItem(freeTextRequest.Industry),
                Region = GetFilterListItem(freeTextRequest.Region),
                Source = GetSourceFilterListItem(freeTextRequest.Source)
            };

            sbData.NewsFilters = GetNewsFilter(freeTextRequest.Filters);
        }

        protected abstract Query GetTopicDetails(string topicId);

        protected abstract void PopulateFromPreferences(SearchBuilderData sbData);


        private IEnumerable<SourceGroupItem> GetProductSourceGroups()
        {
            var sourceGroups = ProductSourceGroupConfigurationManager.SourceGroups(Product.SourceGroupConfigurationId);
            return sourceGroups.Select(Mapper.Map<SourceGroupItem>);
        }

        private FilterList<FilterItem> GetFilterListItem(SearchFilter searchFilter)
        {
            if (searchFilter != null && (searchFilter.Include != null || searchFilter.Exclude != null))
            {
                var list = new FilterList<FilterItem>();
                if (searchFilter.Include != null)
                {
                    list.Include = searchFilter.Include.WhereNotNull().Select(filter => new FilterItem { Code = filter.Code, Desc = filter.Name });
                }

                if (searchFilter.Exclude != null)
                {
                    list.Exclude = searchFilter.Exclude.WhereNotNull().Select(filter => new FilterItem { Code = filter.Code, Desc = filter.Name });
                }

                list.Operator = searchFilter.Operator.HasValue ? searchFilter.Operator.Value : SearchOperator.And;

                return list;
            }
            return null;
        }

        private FilterList<SourceFilterItem> GetSourceFilterListItem(SourceSearchFilter searchFilter)
        {
            if (searchFilter != null && (searchFilter.Include != null || searchFilter.Exclude != null))
            {
                var list = new FilterList<SourceFilterItem>();
                if (searchFilter.Include == null)
                {
                    return list;
                }

                var lstFilterItem = new List<SourceFilterItem>();
                foreach (var filter in searchFilter.Include)
                {
                    if (filter == null)
                    {
                        continue;
                    }

                    var item = new SourceFilterItem();
                    item.AddRange(from f in filter where f != null select new FilterItem {Code = f.Code, Desc = f.Name});
                    lstFilterItem.Add(item);
                }
                list.Include = lstFilterItem;
                return list;
            }
            return null;
        }

        private SearchNewsFilters GetNewsFilter(ICollection<QueryFilter> newsFilters)
        {
            if (newsFilters == null || newsFilters.Count <= 0)
            {
                return null;
            }

            var searchNewsFilters = new SearchNewsFilters();
            foreach (var newsFilter in newsFilters)
            {
                var filterItem = new FilterItem { Code = newsFilter.Code, Desc = newsFilter.Name };
                switch (newsFilter.Category)
                {
                    case NewsFilterCategory.Company:
                        searchNewsFilters.Company = (searchNewsFilters.Company ?? Enumerable.Empty<FilterItem>()).Concat(new[] { filterItem });
                        break;
                    case NewsFilterCategory.Author:
                        searchNewsFilters.Author = (searchNewsFilters.Author ?? Enumerable.Empty<FilterItem>()).Concat(new[] { filterItem });
                        break;
                    case NewsFilterCategory.Executive:
                        searchNewsFilters.Executive = (searchNewsFilters.Executive ?? Enumerable.Empty<FilterItem>()).Concat(new[] { filterItem });
                        break;
                    case NewsFilterCategory.Subject:
                        searchNewsFilters.Subject = (searchNewsFilters.Subject ?? Enumerable.Empty<FilterItem>()).Concat(new[] { filterItem });
                        break;
                    case NewsFilterCategory.Industry:
                        searchNewsFilters.Industry = (searchNewsFilters.Industry ?? Enumerable.Empty<FilterItem>()).Concat(new[] { filterItem });
                        break;
                    case NewsFilterCategory.Region:
                        searchNewsFilters.Region = (searchNewsFilters.Region ?? Enumerable.Empty<FilterItem>()).Concat(new[] { filterItem });
                        break;
                    case NewsFilterCategory.Source:
                        searchNewsFilters.Source = (searchNewsFilters.Source ?? Enumerable.Empty<FilterItem>()).Concat(new[] { filterItem });
                        break;
                    case NewsFilterCategory.DateRange:
                        searchNewsFilters.DateRange = filterItem;
                        break;
                    case NewsFilterCategory.Keyword:
                        searchNewsFilters.Keyword = (searchNewsFilters.Keyword ?? Enumerable.Empty<string>()).Concat(new[] { newsFilter.Name });
                        break;
                }
            }
            return searchNewsFilters;
        }
    }
}
