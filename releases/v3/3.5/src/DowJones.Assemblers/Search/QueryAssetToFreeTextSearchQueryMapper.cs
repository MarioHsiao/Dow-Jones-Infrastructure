using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Mapping;
using DowJones.Search;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;

namespace DowJones.Assemblers.Search
{
    public class QueryAssetToFreeTextSearchQueryMapper : TypeMapper<Query, FreeTextSearchQuery>
    {
        private readonly QueryAssetToSearchQueryMapper _baseMapper;

        public QueryAssetToFreeTextSearchQueryMapper(QueryAssetToSearchQueryMapper baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public override FreeTextSearchQuery Map(Query source)
        {
            Group[] groups = source.Groups.ToArray();
            var target = new FreeTextSearchQuery();

            var baseObject = target as AdvancedSearchQuery;
            _baseMapper.Map(groups, baseObject);

            IEnumerable<QueryFilter> filters = QueryAssetToSearchQueryMapper.GetQueryFilterCollection(groups);
            if (filters == null)
            {
                return target;
            }


            FreeTextFilter textFilters = filters.OfType<FreeTextFilter>().FirstOrDefault();
            if (textFilters != null)
            {
                target.FreeText = String.Concat(textFilters.Texts);
            }


            SearchSectionFilter searchSectionFilter = filters.OfType<SearchSectionFilter>().FirstOrDefault();
            if (searchSectionFilter != null)
            {
                switch (searchSectionFilter.SearchSection)
                {
                    case SearchSection.Author:
                        target.FreeTextIn = SearchFreeTextArea.Author;
                        break;
                    case SearchSection.Headline:
                        target.FreeTextIn = SearchFreeTextArea.Headline;
                        break;
                    case SearchSection.HeadlineLeadParagraph:
                        target.FreeTextIn = SearchFreeTextArea.HeadlineAndLeadParagraph;
                        break;
                    default:
                        target.FreeTextIn = SearchFreeTextArea.FullArticle;
                        break;
                }
            }

            return target;
        }
    }
}