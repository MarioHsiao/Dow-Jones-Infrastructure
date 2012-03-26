// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyndicationCacheKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Managers;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Syndication
{
    public sealed class SyndicationCacheKeyGetHeadlinesGenerator : AbstractModuleCacheKeyGenerator<SyndicationCacheKeyGetHeadlinesGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "Syndication_Headlines";

        public SyndicationCacheKeyGetHeadlinesGenerator() : base(string.Empty)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public SyndicationCacheKeyGetHeadlinesGenerator(string moduleId, string feedId, int maxHeadlinesToReturn, Product product = Product.Np) : base(moduleId, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;

            FeedId = feedId;
            MaxHeadlinesToReturn = maxHeadlinesToReturn;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.Guid)]
        public string FeedId { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.MaxHeadlinesToReturn)]
        public int MaxHeadlinesToReturn { get; set; }
    }

    public sealed class SyndicationGetItemCacheKeyGenerator : AbstractModuleCacheKeyGenerator<SyndicationGetItemCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = 4 * CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private const CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheExiprationPolicy.Absolute;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "Syndication_GetItem";

        public SyndicationGetItemCacheKeyGenerator()
            : base(string.Empty)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public SyndicationGetItemCacheKeyGenerator(string moduleId, string feedId, Product product = Product.Np)
            : base(moduleId, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;

            FeedId = feedId;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.Guid)]
        public string FeedId { get; set; }
    }
}
