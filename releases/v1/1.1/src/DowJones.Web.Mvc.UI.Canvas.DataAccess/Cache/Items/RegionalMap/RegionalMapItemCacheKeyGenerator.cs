// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegionalMapItemCacheKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Managers;
using DowJones.Utilities.Managers.Search;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.RegionalMap
{
    public sealed class RegionalMapItemCacheKeyGenerator : AbstractModuleCacheKeyGenerator<RegionalMapItemCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "RegionalMapItem";

        private const TimeFrame DefaultTimeFrame = TimeFrame.LastMonth;

        public RegionalMapItemCacheKeyGenerator() 
            : base(string.Empty)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public RegionalMapItemCacheKeyGenerator(string moduleId, string regionCode, TimeFrame timeFrame = DefaultTimeFrame, CacheTimePeriod timePeriod = CacheTimePeriod.Current, Product product = Product.Np)
            : base(moduleId, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;

            ModuleId = moduleId;
            RegionCode = regionCode;
            TimeFrame = timeFrame;
            TimePeriod = timePeriod;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.Guid)]
        public string RegionCode { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.TimeFrame)]
        public TimeFrame TimeFrame { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.TimePeriod)]
        public CacheTimePeriod TimePeriod { get; set; }
    }

    public sealed class RegionalMapCacheKeyGenerator : AbstractModuleCacheKeyGenerator<RegionalMapCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "RegionalMap";

        private const TimeFrame DefaultTimeFrame = TimeFrame.LastMonth;

        public RegionalMapCacheKeyGenerator()
            : base(string.Empty)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public RegionalMapCacheKeyGenerator(string moduleId, TimeFrame timeFrame = DefaultTimeFrame, Product product = Product.Np)
            : base(moduleId, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;

            TimeFrame = timeFrame;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.TimeFrame)]
        public TimeFrame TimeFrame { get; set; }
    }
}
