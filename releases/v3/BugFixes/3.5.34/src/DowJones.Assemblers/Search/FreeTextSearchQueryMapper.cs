using System;
using DowJones.Extensions;
using DowJones.Mapping;
using DowJones.Search;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;

namespace DowJones.Assemblers.Search
{
    public class FreeTextQueryMapper : TypeMapper<FreeTextSearchQuery, GroupCollection>
    {
        private readonly SearchQueryToQueryAssetMapper _baseMapper;

        public FreeTextQueryMapper(SearchQueryToQueryAssetMapper baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public override GroupCollection Map(FreeTextSearchQuery source)
        {
            var baseObject = source as AdvancedSearchQuery;


            var groups = new GroupCollection {new Group {Operator = GroupOperator.And}};
            groups[0].FilterGroup = new AndFilterGroup();
            groups[0].FilterGroup.Filters = new QueryFilterCollection();

            _baseMapper.Map(baseObject, groups);

            FilterGroup andFilterGroup = SearchQueryToQueryAssetMapper.GetGroup(GroupOperator.And, groups).FilterGroup;


            if (!String.IsNullOrEmpty(source.FreeText))
            {
                var freeTextFilter = new FreeTextFilter
                                         {
                                             Operator = Operator.And,
                                             Texts = new TextCollection {source.FreeText},
                                             SearchMode = SearchMode.Traditional
                                         };
                andFilterGroup.Filters.Add(freeTextFilter);
            }

            var sectionFilter = new SearchSectionFilter();
            switch (source.FreeTextIn)
            {
                case SearchFreeTextArea.Author:
                    sectionFilter.SearchSection = SearchSection.Author;
                    break;
                case SearchFreeTextArea.Headline:
                    sectionFilter.SearchSection = SearchSection.Headline;
                    break;
                case SearchFreeTextArea.HeadlineAndLeadParagraph:
                    sectionFilter.SearchSection = SearchSection.HeadlineLeadParagraph;
                    break;
                case SearchFreeTextArea.FullArticle:
                    sectionFilter.SearchSection = SearchSection.FullArticle;
                    break;
            }
            andFilterGroup.Filters.Add(sectionFilter);
            return groups;
        }
    }
}