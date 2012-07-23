using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Infrastructure.Common;
using DowJones.Search;
using DowJones.Search.Core;
using DowJones.Web.Mvc.Search.Requests.Common;
using DowJones.Web.Mvc.Search.Requests.Filters;

namespace DowJones.Web.Mvc.Search.Requests.Mappers
{
    public class AdvancedSearchRequestMapper : SearchRequestMapper
    {
        public AdvancedSearchRequestMapper(Product product, DateTimeFormatter dateTimeFormatter)
            : base(product, dateTimeFormatter)
        {
        }

        public void Map(AdvancedSearchQuery query, AdvancedSearchRequest source)
        {
            base.Map(query, source);

            query.Author = MapSearchFilter(source.Author, EntityType.Author);
            query.Company = MapSearchFilter(source.Company, EntityType.Company);
            query.Executive = MapSearchFilter(source.Executive, EntityType.Executive);
            query.Industry = MapSearchFilter(source.Industry, EntityType.Industry);
            query.Region = MapSearchFilter(source.Region, EntityType.Region);
            query.Subject = MapSearchFilter(source.Subject, EntityType.Subject);
            query.Source = MapSourceSearchFilter(source.Source);
           
            query.Exclusions = source.Exclusions;
        }

        public void Map(AdvancedSearchRequest searchRequest,  AdvancedSearchQuery source)
        {
            base.Map(searchRequest, source);

            searchRequest.Author = MapCompoundQueryFilter(source.Author, NewsFilterCategory.Author);
            searchRequest.Company = MapCompoundQueryFilter(source.Company, NewsFilterCategory.Company);
            searchRequest.Executive = MapCompoundQueryFilter(source.Executive, NewsFilterCategory.Executive);
            searchRequest.Industry = MapCompoundQueryFilter(source.Industry, NewsFilterCategory.Industry);
            searchRequest.Region = MapCompoundQueryFilter(source.Region, NewsFilterCategory.Region);
            searchRequest.Subject = MapCompoundQueryFilter(source.Subject, NewsFilterCategory.Subject);
            searchRequest.Source = MapSourceSearchFilter(source.Source);
            searchRequest.Exclusions = source.Exclusions;
        }

        private CompoundQueryFilter MapSourceSearchFilter(SourceSearchFilter sourceSearchFilter)
        {
            if (sourceSearchFilter == null || ((sourceSearchFilter.Include == null || sourceSearchFilter.Include.Count == 0) && string.IsNullOrEmpty(sourceSearchFilter.ListId)))
            {
                return null;
            }
            var compoundQueryFilter = new CompoundQueryFilter
                                          {
                                              Include = sourceSearchFilter.Include.Select(MapSourceSourceFilter),
                                              ListId = sourceSearchFilter.ListId,
                                              ListName = sourceSearchFilter.ListName,
                                              ListType =  CompoundQueryListType.Source
                                          };
            return compoundQueryFilter;
        }

        public SourceSearchFilter MapSourceSearchFilter(CompoundQueryFilter compoundQueryFilter)
        {
            if (compoundQueryFilter == null || ((compoundQueryFilter.Include == null || compoundQueryFilter.Include.Count() == 0) && string.IsNullOrEmpty(compoundQueryFilter.ListId)))
            {
                return new SourceSearchFilter();
            }

            var includes = MapSourceQueryFilters(compoundQueryFilter.Include);
            var sourceSearchFilter = new SourceSearchFilter {
                                              Include = includes.ToList(),
                                              ListId = compoundQueryFilter.ListId,
                                              ListName = compoundQueryFilter.ListName
                                          };
            return sourceSearchFilter;
        }

        private IQueryFilter MapSourceSourceFilter(QueryFilters filters)
        {
            if (filters == null)
                return null;

            var entities = 
                from filter in filters
                where filter != null
                let type = filter.Type.GetValueOrDefault(SourceFilterType.SourceCode.ToString())
                let sourceType = SourceFilterTypeTypeMapper.Map(type)
                select new SourceQueryFilterEntity {
                                                       SourceCode = filter.Code,
                                                       SourceType = sourceType
                                                   };

            return new SourceQueryFilterEntities(entities);
        }

        private CompoundQueryFilter MapSearchFilter(SearchFilter searchFilter, EntityType type)
        {
            if (searchFilter == null || (searchFilter.Include == null && searchFilter.Exclude == null))
            {
                return null;
            }

            var compoundQueryFilter = new CompoundQueryFilter
                                          {
                                              Include = MapQueryFilters(searchFilter.Include, type),
                                              Exclude = MapQueryFilters(searchFilter.Exclude, type),
                                              Operator =
                                                  (searchFilter.Operator == SearchOperator.And)
                                                      ? SearchOperator.And
                                                      : SearchOperator.Or
                                          };
            return compoundQueryFilter;
        }

        private SearchFilter MapCompoundQueryFilter(CompoundQueryFilter queryFilter, NewsFilterCategory filterCategory)
        {
            if (queryFilter == null || (queryFilter.Include == null && queryFilter.Exclude == null))
            {
                return null;
            }

            var searchFilter = new SearchFilter
            {
                Include = MapEntityQueryFilters(queryFilter.Include, filterCategory) ?? new QueryFilters(),
                Exclude = MapEntityQueryFilters(queryFilter.Exclude, filterCategory) ?? new QueryFilters(),
                Operator =
                    (queryFilter.Operator == SearchOperator.And)
                        ? SearchOperator.And
                        : SearchOperator.Or
            };
            return searchFilter;
        }

        private IEnumerable<IQueryFilter> MapQueryFilters(IEnumerable<QueryFilter> include, EntityType type)
        {
            if (include != null)
            {
                var filters = include.Select(item => item.Code);
                yield return new EntitiesQueryFilter(type, filters);
            }
        }

        private QueryFilters MapEntityQueryFilters(IEnumerable<IQueryFilter> filters, NewsFilterCategory filterCategory)
        {
            if (filters == null)
            {
                return null;
            }

            var queryFilters =
                from entities in filters.OfType<EntitiesQueryFilter>()
                from entity in entities
                let category = filterCategory
                select new QueryFilter
                           {
                               Category = category,
                               Code = entity
                           };

            return new QueryFilters(queryFilters.ToList());
        }

        private IEnumerable<QueryFilters> MapSourceQueryFilters(IEnumerable<IQueryFilter> source)
        {
            foreach(var filter in source ?? Enumerable.Empty<IQueryFilter>())
            {
                var queryFilters = new QueryFilters();

                if (filter is SourceQueryFilterEntities)
                {
                    var sourceFilters = ((SourceQueryFilterEntities)filter);
                    var filterEntities = sourceFilters.Select(MapSourceQueryFilterEntity);
                    queryFilters.AddRange(filterEntities);
                }

                if (filter is SourceQueryFilterEntity)
                {
                    var entity = MapSourceQueryFilterEntity((SourceQueryFilterEntity)filter);
                    queryFilters.Add(entity);
                }

                yield return queryFilters;
            } 
        }

        private QueryFilter MapSourceQueryFilterEntity(SourceQueryFilterEntity item)
        {
            return new QueryFilter
                       {
                           Code = item.SourceCode,
                           Type = SourceFilterTypeTypeMapper.Map(item.SourceType)
                       };
        }
    }
}
