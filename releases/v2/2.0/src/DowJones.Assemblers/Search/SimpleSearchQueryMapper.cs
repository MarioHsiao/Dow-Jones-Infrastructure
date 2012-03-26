using DowJones.Mapping;
using DowJones.Search;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using DowJones.Extensions;
using System.Linq;

namespace DowJones.Assemblers.Search
{
    public class SimpleSearchQueryMapper : TypeMapper<SimpleSearchQuery, GroupCollection>
    {
        private readonly SearchQueryToQueryAssetMapper _baseMapper;

        public SimpleSearchQueryMapper(SearchQueryToQueryAssetMapper baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public override GroupCollection Map(SimpleSearchQuery source)
        {
            var groups = new GroupCollection
                             {new Group {Operator = GroupOperator.And}};

            _baseMapper.Map(source, groups);

            FilterGroup filterGroup = SearchQueryToQueryAssetMapper.GetGroup(GroupOperator.And, groups).FilterGroup;

            //Keywords
            var freeTextFilter = new FreeTextFilter
            {
                Operator = Operator.Or,
                Texts = new TextCollection { source.Keywords },
                SearchMode = SearchMode.Simple
            };
            filterGroup.Filters.Add(freeTextFilter);


            //Source
            string selectedSource = source.Source;
            if (!string.IsNullOrEmpty(selectedSource))
            {
                long sourceId;
                if (long.TryParse(selectedSource, out sourceId)) // Assume ID means source list!
                {
                    var listIdFilter = new SourceEntityListIDFilter();
                    listIdFilter.Operator = Operator.Or;
                    listIdFilter.IdCollectionCollection = new IdCollectionCollection();
                    listIdFilter.IdCollectionCollection.Add(selectedSource);
                    filterGroup.Filters.Add(listIdFilter);
                }
                else //Else only once source code which is product define code!
                {
                    var sf = new SourceEntityFilter
                                 {
                                     SourceEntitiesCollection = new SourceEntitiesCollection
                                                                    {
                                                                        new SourceEntities
                                                                            {
                                                                                SourceEntityCollection =
                                                                                    new SourceEntityCollection
                                                                                        {
                                                                                            new SourceEntity
                                                                                                {
                                                                                                    Type =
                                                                                                        SourceEntityType
                                                                                                        .PDF,
                                                                                                    Value =
                                                                                                        selectedSource
                                                                                                }
                                                                                        }
                                                                            }
                                                                    }
                                 };
                    filterGroup.Filters.Add(sf);
                }
            }
            return groups;
        }
    }
}