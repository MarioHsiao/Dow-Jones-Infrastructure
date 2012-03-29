using DowJones.Caching;
using DowJones.Properties;
using DowJones.Infrastructure.Common;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Newtonsoft.Json;

namespace DowJones.Managers.Topics.Caching
{
    public sealed class SubscribableTopicsCacheKey : AbstractInfrastructureCacheKey
    {
        public static bool CachingEnabled = InfrastructureCacheKeyConstants.IncludeCacheKeyGeneration && Settings.Default.CacheSubscribableTopics;

        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = InfrastructureCacheKeyConstants.DefaultCacheExiprationPolicy;

        private static int DefaultCacheExpirationTime = InfrastructureCacheKeyConstants.DefaultCacheExpirationTime;//60;
        private static int DefaultCacheRefreshInterval = InfrastructureCacheKeyConstants.DefaultCacheRefreshInterval;//30;
        private const CacheScope DefaultCacheScope = CacheScope.Account;
        private const bool DefaultCacheForceCacheRefresh = false;

        private const string DefaultName = "SubscribableTopics";

        //public SubscribableTopicsCacheKey(Product product, ShareScopeCollection shareScopeCollection) : base(product)
        public SubscribableTopicsCacheKey(Product product) : base(product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
            //ShareScopeCollection = shareScopeCollection;
        }

        [JsonProperty(PropertyName = InfrastructureCacheKeyConstants.ShareScopeCollection)]
        public ShareScopeCollection ShareScopeCollection { get; set; }
    }
}
