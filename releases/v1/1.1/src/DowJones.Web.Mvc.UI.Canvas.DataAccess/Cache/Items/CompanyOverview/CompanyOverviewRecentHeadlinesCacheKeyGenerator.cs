// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyOverviewRecentHeadlinesCacheKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Managers;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.CompanyOverview
{

    public sealed class CompanyOverviewRecentHeadlinesCacheKeyGenerator : AbstractCompanyOverviewCacheKeyGenerator<CompanyOverviewRecentHeadlinesCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.User;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "CompanyOverview_RecentHeadlines";

        public CompanyOverviewRecentHeadlinesCacheKeyGenerator()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public CompanyOverviewRecentHeadlinesCacheKeyGenerator(string moduleId, string companyCode, int maxHeadlinesToReturn, string[] contentLanguages, Product product = Product.Np)
            : base(moduleId, companyCode, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;

            MaxHeadlinesToReturn = maxHeadlinesToReturn;
            ContentLanguages = contentLanguages;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.MaxHeadlinesToReturn)]
        public int MaxHeadlinesToReturn { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.ContentLanguages)]
        public string[] ContentLanguages { get; set; }
    }
}
