using DowJones.Utilities.Managers;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Summary
{
    public sealed class SummaryRecentVideosCacheKeyGenerator : AbstractSummaryCacheKeyGenerator<SummaryRecentVideosCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.User;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "Summary_RecentVideos";

        public SummaryRecentVideosCacheKeyGenerator()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public SummaryRecentVideosCacheKeyGenerator(string moduleId, int maxHeadlinesToReturn, string[] contentLanguages, Product product = Product.Np) :
            base(moduleId, product)
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
