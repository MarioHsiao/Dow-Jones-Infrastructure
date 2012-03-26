using DowJones.Utilities.Managers;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Summary
{
    public sealed class SummaryTrendingDataCacheKeyGenerator : AbstractSummaryCacheKeyGenerator<SummaryTrendingDataCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "Summary_Trending";

        public SummaryTrendingDataCacheKeyGenerator()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public SummaryTrendingDataCacheKeyGenerator(string moduleId, string[] contentLanguages, string interfaceLanguage = "en", Product product = Product.Np) :
            base(moduleId, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;

            InterfaceLanguage = interfaceLanguage;
            ContentLanguages = contentLanguages;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.InterfaceLanguage)]
        public string InterfaceLanguage { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.ContentLanguages)]
        public string[] ContentLanguages { get; set; }
    }
}
