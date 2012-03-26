// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SummaryRegionalMapItemCacheKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Managers;
using DowJones.Utilities.Managers.Search;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Summary
{
    public sealed class SummaryRegionalMapItemCacheKeyGenerator : AbstractSummaryCacheKeyGenerator<SummaryRegionalMapItemCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "Summary_RegionalMapItem";

        public SummaryRegionalMapItemCacheKeyGenerator() 
            : base(string.Empty)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public SummaryRegionalMapItemCacheKeyGenerator(string moduleId, string regionCode, Product product = Product.Np)
            : base(moduleId, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
            RegionCode = regionCode;
        }
        
        [JsonProperty(PropertyName = CacheKeyConstants.RegionCode)]
        public string RegionCode { get; set; }
    }

    public sealed class SummaryRegionalMapCacheKeyGenerator : AbstractSummaryCacheKeyGenerator<SummaryRegionalMapCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "Summary_RegionalMap";

        public SummaryRegionalMapCacheKeyGenerator()
            : base(string.Empty)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public SummaryRegionalMapCacheKeyGenerator(string moduleId, Product product = Product.Np)
            : base(moduleId, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }
    }
}
