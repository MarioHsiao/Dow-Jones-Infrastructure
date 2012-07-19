using DowJones.Caching;
using DowJones.Infrastructure.Common;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Newtonsoft.Json;

namespace DowJones.Managers.Topics.Caching
{
    public sealed class SubscribableTopicsCacheKey : AbstractCacheKey
    {
        public static bool CachingEnabled = CacheKeyConstants.IncludeCacheKeyGeneration && Settings.Default.CacheSubscribableTopics;

        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;

        private const int DefaultCacheExpirationTime = 60;
        private const int DefaultCacheRefreshInterval = 30;
        private const CacheScope DefaultCacheScope = CacheScope.Account;
        private const bool DefaultCacheForceCacheRefresh = false;

        private const string DefaultName = "SubscribableTopics";

        public SubscribableTopicsCacheKey(Product product, ShareScopeCollection shareScopeCollection) : base(product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;

            ShareScopeCollection = shareScopeCollection;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.ShareScopeCollection)]
        public ShareScopeCollection ShareScopeCollection { get; set; }
    }
}
