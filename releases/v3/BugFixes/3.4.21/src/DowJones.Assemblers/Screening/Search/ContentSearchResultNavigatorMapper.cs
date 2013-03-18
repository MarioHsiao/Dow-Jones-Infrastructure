using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.DependencyInjection;
using DowJones.Managers.Search;
using DowJones.Mapping;
using DowJones.Search.Navigation;
using Factiva.Gateway.Messages.Assets.Lists.V1_0;
using Factiva.Gateway.Messages.Search.V2_0;
using Keyword = DowJones.Search.Navigation.Keyword;
using Navigator = DowJones.Search.Navigation.Navigator;

namespace DowJones.Assemblers.Search
{
    public class ProductContentSearchResultNavigatorMapper : TypeMapper<ProductContentSearchResult, ResultNavigator>
    {
        [Inject("Cant inject in constructor")]
        public ProductSourceGroupConfigurationManager ProductSourceGroup { get; set; }


        public override ResultNavigator Map(ProductContentSearchResult source)
        {
            var contentSearchResult = source.ContentSearchResult;

            var keywords =
                from keyword in contentSearchResult.KeywordSet.KeywordCollection
                select new Keyword(keyword.Value, keyword.Weight);

            var navigators =
                (from codeNavigator in contentSearchResult.CodeNavigatorSet.NavigatorCollection
                where codeNavigator.BucketCollection != null
                select new Navigator
                           {
                               Code = codeNavigator.Id,
                               Items = from bucket in codeNavigator.BucketCollection
                                       select new NavigatorItem
                                                  {
                                                      Count = bucket.HitCount,
                                                      Name = bucket.Value,
                                                      Value = bucket.Id,
                                                  }
                           }).ToList();

            var groups = GetSourceGroups(source, source.ProductId);

            CompositeNavigatorGroup compositeNavigatorGroup = GetCompositeNavigatorGroup(source.ProductId, source.PrimarySourceGroupId);

            //Add source navigator as group if leaf level secondary group is selected.
            Navigator sourceNavigator =
                (navigators.Where(
                    navigator =>
                    navigator.Code.Equals(Navigator.Codes.Source, StringComparison.InvariantCultureIgnoreCase))).
                    FirstOrDefault();
            if (sourceNavigator != null)
            {

                AppendSourceNavigatorToLeafGroup(ref compositeNavigatorGroup, sourceNavigator, source.SecondarySourceGroupId);
            }

            return new ResultNavigator(groups, navigators, keywords, compositeNavigatorGroup);
        }

        private void AppendSourceNavigatorToLeafGroup(ref CompositeNavigatorGroup compositeNavigatorGroup, Navigator navigators, string sourceGroupId)
        {
            if (string.IsNullOrEmpty(sourceGroupId) || compositeNavigatorGroup == null || navigators == null)
            {
                return;
            }
            
            // Updating object using LINQ, use ToList()
            var a = compositeNavigatorGroup.Groups.ToList();
            for (int index = 0; index < a.Count; index++)
            {
                var navigatorGroup = a[index] as CompositeNavigatorGroup;
                var clone = navigatorGroup;
                var b = FindGroupAndAppendNavigator(ref clone, navigators, sourceGroupId);
                if (b != null)
                {
                    b.Navigator = navigators;
                    a[index] = b;
                    return;
                }
            }
        }

        private CompositeNavigatorGroup FindGroupAndAppendNavigator(ref CompositeNavigatorGroup navigatorGroup, Navigator navigators, string id)
        {
            if (navigatorGroup == null)
            {
                return null;
            }
            if (navigatorGroup.Code.Equals(id))
            {
                return navigatorGroup;
            }

            if (navigatorGroup.Groups != null)
            {
                var subGroups = navigatorGroup.Groups.ToArray();
                return (from CompositeNavigatorGroup bClone in subGroups select FindGroupAndAppendNavigator(ref bClone, navigators, id)).FirstOrDefault();
            }
            return null;
        }


        private CompositeNavigatorGroup GetCompositeNavigatorGroup(string productId, string primarySourceGroupId)
        {
            if (primarySourceGroupId == null)
            {
                return null;
            }
            var group = new CompositeNavigatorGroup { Code = primarySourceGroupId };
            var productSourceGroups = ProductSourceGroup.SourceGroups(productId, primarySourceGroupId);
            group.Groups = productSourceGroups.Select(SourceGroupToNavigatorGroup).ToList();
            return group;
        }

        private static NavigatorGroup SourceGroupToNavigatorGroup(SourceGroup sourceGroup)
        {
            if (sourceGroup.SourceGroupCollection == null)
            {
                return new NavigatorGroup {Name = sourceGroup.Descriptor, Code = sourceGroup.PdfCode};
            }
            else
            {
                var groups = sourceGroup.SourceGroupCollection.Select(SourceGroupToNavigatorGroup).ToList();
                return new CompositeNavigatorGroup
                           {
                               Name = sourceGroup.Descriptor,
                               Code = sourceGroup.PdfCode,
                               Groups = groups
                           };
            }
        }


        private SourceGroups GetSourceGroups(ProductContentSearchResult source, string productId)
        {
            ContentSearchResult contentSearchResult = source.ContentSearchResult;

            IEnumerable<SourceGroup> productSourceGroups = ProductSourceGroup.SourceGroups(productId);
            
            var sourceGroups = 
                from productSourceGroup in productSourceGroups
                let psts = ProductSourceGroup.PrimarySourceTypes(productId, productSourceGroup.PdfCode)
                let count = (from pst in psts
                                from collectionCount in contentSearchResult.CollectionCountSet.CollectionCountCollection
                                where collectionCount.SourceTypeCollection != null && collectionCount.SourceTypeCollection.Equals(pst, StringComparison.InvariantCultureIgnoreCase)
                                select collectionCount.HitCount).Sum()
                select new NavigatorGroup(productSourceGroup.PdfCode, count, productSourceGroup.Descriptor);

            return new SourceGroups(sourceGroups) { PrimaryGroupId = source.PrimarySourceGroupId };
        }
    }
}