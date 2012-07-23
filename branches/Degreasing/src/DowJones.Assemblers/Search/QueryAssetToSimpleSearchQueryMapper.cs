using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Mapping;
using DowJones.Search;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;

namespace DowJones.Assemblers.Search
{
    public class QueryAssetToSimpleSearchQueryMapper : TypeMapper<Query, SimpleSearchQuery>
    {
        private readonly QueryAssetToSearchQueryMapper _baseMapper;

        public QueryAssetToSimpleSearchQueryMapper(QueryAssetToSearchQueryMapper baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public override SimpleSearchQuery Map(Query source)
        {
            Group[] groups = source.Groups.ToArray();
            var target = new SimpleSearchQuery();

            var baseObject = target as AbstractSearchQuery;
            _baseMapper.Map(groups, baseObject);

            IEnumerable<QueryFilter> filters = QueryAssetToSearchQueryMapper.GetQueryFilterCollection(groups);
            if (filters == null)
            {
                return target;
            }

            //Keywords
            FreeTextFilter textFilters = filters.OfType<FreeTextFilter>().FirstOrDefault();
            if (textFilters != null)
            {
                target.Keywords = String.Concat(textFilters.Texts);
            }
            
            //Source list
            SourceEntityListIDFilter entityListIdFilter = filters.OfType<SourceEntityListIDFilter>().FirstOrDefault();
            if (entityListIdFilter != null && entityListIdFilter.IdCollectionCollection.Any())
            {
                target.Source = entityListIdFilter.IdCollectionCollection[0];
            }
            else
            {
                SourceEntityFilter sf = filters.OfType<SourceEntityFilter>().FirstOrDefault();
                if (sf != null && sf.SourceEntitiesCollection != null)
                {
                    var firstOne = sf.SourceEntitiesCollection.FirstOrDefault();
                    if (firstOne != null)
                    {
                        target.Source = firstOne.SourceEntityCollection.Where(d => d.Type == SourceEntityType.PDF).First().Value;
                    }
                }
            }
            return target;
        }
    }
}