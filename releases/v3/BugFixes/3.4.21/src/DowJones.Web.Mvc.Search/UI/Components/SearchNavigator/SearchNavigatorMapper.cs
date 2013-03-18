using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Managers.Search;
using DowJones.Mapping;
using DowJones.Search;
using DowJones.Search.Navigation;
using DowJones.Token;

namespace DowJones.Web.Mvc.Search.UI.Components.Filters
{
    public class SearchNavigatorMapper 
        : ITypeMapper<SearchResponse, SearchNavigator>,
          ITypeMapper<ResultNavigator, SearchNavigator>
    {
        private static readonly IEnumerable<string> FilteredNavigatorCodes = new[] {
                Navigator.Codes.Author, Navigator.Codes.Company, Navigator.Codes.Executive,
                Navigator.Codes.Industry, Navigator.Codes.Region, Navigator.Codes.Subject,
            };


        private readonly ITokenRegistry _tokenRegistry;

        public SearchNavigatorMapper(ITokenRegistry tokenRegistry)
        {
            _tokenRegistry = tokenRegistry;
        }

        public object Map(object source)
        {
            if (source is ResultNavigator)
                return Map((ResultNavigator) source);

            if (source is SearchResponse)
                return Map((SearchResponse)source);

            throw new NotSupportedException();
        }

        public SearchNavigator Map(SearchResponse searchResponse)
        {
            if (searchResponse == null)
                return new SearchNavigator();

            var SearchNavigator = Map(searchResponse.Navigators);

            return SearchNavigator;
        }

        public SearchNavigator Map(ResultNavigator source)
        {
            if (source == null)
                return new SearchNavigator();

            var primaryGroupId = source.SourceGroups.PrimaryGroupId;
            var secondaryGroupId = source.SourceGroups.SecondaryGroupId;

            var primarySourceGroups = GetPrimarySourceGroups(source, primaryGroupId);
            var secondarySourceGroups = GetSecondarySourceGroups(source, secondaryGroupId);
            var entityFilters = GetEntityFilters(source);

            return new SearchNavigator
            {
                EntityFilters = entityFilters,
                PrimarySourceGroups = primarySourceGroups,
                PrimaryGroupId = primaryGroupId,
                SecondarySourceGroups = secondarySourceGroups,
                SecondaryGroupId = secondaryGroupId,
            };
        }

        private IEnumerable<SearchNavigatorNode> GetPrimarySourceGroups(ResultNavigator source, string primaryGroupId)
        {
            var primarySourceGroups = source.SourceGroups.Select(g => Map(g, primaryGroupId)).ToArray();

            var allSourceGroup = new SearchNavigatorNode {
                    DisplayName = _tokenRegistry.Get("all"),
                    GroupTag = "all",
                    // Select the All group if no other group was selected
                    IsSelected = !primarySourceGroups.Any(x => x.IsSelected)
                };

            return new[] { allSourceGroup }.Union(primarySourceGroups);
        }

        private SearchNavigatorNode GetSecondarySourceGroups(ResultNavigator source, string secondaryGroupId)
        {
            var secondarySourceGroups = new List<SearchNavigatorNode>();

            if (source.CompositeNavigatorGroup != null && source.CompositeNavigatorGroup.Groups != null)
            {
                var groups = source.CompositeNavigatorGroup.Groups;
                secondarySourceGroups.AddRange(groups.Select(g => Map(g, secondaryGroupId)));
            }

            if(!secondarySourceGroups.Any())
                return null;

            return new SearchNavigatorNode(secondarySourceGroups)
                       {
                           DisplayName = _tokenRegistry.Get("sources"),
                           IsSelectable = false,
                           IsSelected = true
                       };
        }

        private IEnumerable<SearchNavigatorNode> GetEntityFilters(ResultNavigator source)
        {
            var navigators =
                from code in FilteredNavigatorCodes
                from navigator in source.Navigators
                where string.Equals(navigator.Code, code, StringComparison.OrdinalIgnoreCase)
                select navigator;

            var entityFilters = navigators.Select(Map).ToList();
            entityFilters.ForEach(filter => filter.IsSelectable = false);
            return entityFilters;
        }

        public SearchNavigatorNode Map(NavigatorGroup group, string selectedGroupId)
        {
            List<SearchNavigatorNode> children = new List<SearchNavigatorNode>();
            var displayName = group.Name;

            // If this group contains sub-groups, add them to the list of children
            var compositeGroup = group as CompositeNavigatorGroup;
            if (compositeGroup != null)
            {
                children.AddRange(compositeGroup.Groups.Select(g => Map(g, selectedGroupId)));
            }

            var isSelectedGroup =
                string.Equals(group.Code, selectedGroupId, StringComparison.OrdinalIgnoreCase);

            // If the group contains items, add them as children
            if(group.Navigator != null)
            {
                children.AddRange(MapChildren(group.Navigator));

                // If the group has a Navigator that means it is the selected
                // Secondary Source.  So, select it!
                isSelectedGroup = true;
            }

            var groupTag = (group.Code ?? string.Empty).Replace("SG_", string.Empty).ToLower();

            var filter = new SearchNavigatorNode(children)
                             {
                                 DisplayName = displayName,
                                 GroupCode = group.Code,
                                 GroupTag = groupTag,
                                 IsSecondaryGroup = true,
                                 IsSelected = isSelectedGroup,
                                 ResultCount = group.Count,
                             };

            return filter;
        }

        public SearchNavigatorNode Map(Navigator navigator)
        {
            var children = MapChildren(navigator);
            var category = ToFilterCategory(navigator.Code);
            var displayName = _tokenRegistry.Get(category);

            var filter = new SearchNavigatorNode(children)
                             {
                                 GroupCode = ToGroupCode(category),
                                 DisplayName = displayName,
                                 IsSelected = true
                             };
            return filter;
        }

        private IEnumerable<SearchNavigatorNode> MapChildren(Navigator navigator)
        {
            var category = ToFilterCategory(navigator.Code);
            var groupCode = ToGroupCode(category);
            return
                from child in navigator.Items
                select new SearchNavigatorNode {
                               DisplayName = child.Name,
                               GroupCode = groupCode,
                               Ref = child.Value,
                               ResultCount = child.Count,
                           };
        }

        private string ToGroupCode(NewsFilterCategory? category)
        {
            return category.HasValue ? category.Value.ToString() : null;
        }

        private static NewsFilterCategory? ToFilterCategory(string navigatorCode)
        {
            switch(navigatorCode.ToUpper())
            {
                case(Navigator.Codes.Author):
                    return NewsFilterCategory.Author;

                case(Navigator.Codes.Company):
                    return NewsFilterCategory.Company;

                case(Navigator.Codes.Executive):
                    return NewsFilterCategory.Executive;

                case(Navigator.Codes.Industry):
                    return NewsFilterCategory.Industry;

                case(Navigator.Codes.Region):
                    return NewsFilterCategory.Region;

                case(Navigator.Codes.Source):
                    return NewsFilterCategory.Source;

                case(Navigator.Codes.Subject):
                    return NewsFilterCategory.Subject;

                default:
                    return null;
            }
        }
    }
}