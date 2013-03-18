using DowJones.Infrastructure;
using DowJones.Infrastructure.Common;
using DowJones.Session;
using Newtonsoft.Json;

namespace DowJones.Caching
{
    public interface ICacheKey
    {
        [JsonProperty(PropertyName = CacheKeyConstants.Name, NullValueHandling = NullValueHandling.Ignore)]
        string Name { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.Version, NullValueHandling = NullValueHandling.Ignore)]
        string Version { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.Enviroment, NullValueHandling = NullValueHandling.Ignore)]
        string Environment { get; set; }

        [JsonIgnore]
        CacheScope CacheScope { get; set; }

        [JsonIgnore]
        int CacheExpirationTime { get; set; }

        [JsonIgnore]
        int CacheRefreshInterval { get; set; }

        [JsonIgnore]
        CacheExiprationPolicy CacheExiprationPolicy { get; set; }

        [JsonIgnore]
        bool CacheForceCacheRefresh { get; set; }

        [JsonIgnore]
        string CacheApplication { get; set; }

        string Serialize();
    }

    public static class ICacheKeyExtensions
    {
        public static IControlData GetCacheControlData(this ICacheKey cacheKey, IControlData controlData, Product product)
        {
            Guard.IsNotNull(cacheKey, "cacheKey");
            Guard.IsNotNull(product, "product");

            return GetCacheControlData(cacheKey, controlData, product.CacheApplication);
        }

        public static IControlData GetCacheControlData(this ICacheKey cacheKey, IControlData controlData, string cacheApplication)
        {
            Guard.IsNotNull(cacheKey, "cacheKey");

            var tempControlData = ControlDataManager.GetControlDataForTransactionCache(
                controlData,
                cacheKey.Serialize(),
                cacheKey.CacheScope,
                cacheKey.CacheExpirationTime,
                cacheKey.CacheRefreshInterval,
                cacheKey.CacheExiprationPolicy,
                cacheKey.CacheForceCacheRefresh);

            tempControlData.CacheApplication = cacheApplication;  
            return tempControlData;
        }                                             
    }
}