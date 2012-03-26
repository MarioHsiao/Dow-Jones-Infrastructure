// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrendingCacheKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Managers;
using DowJones.Utilities.Managers.Search;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Trending
{
    public sealed class TrendingCacheKeyGenerator : AbstractModuleCacheKeyGenerator<TrendingCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "Trending";

        private const TimeFrame DefaultTimeFrame = TimeFrame.LastMonth;

        public TrendingCacheKeyGenerator(string moduleId, string interfaceLanguage, Requests.EntityType entityType, TimeFrame timeFrame = DefaultTimeFrame, CacheTimePeriod timePeriod = CacheTimePeriod.Current, Product product = Product.Np)
            : base(moduleId, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;

            EntityType = entityType;
            ModuleId = Guid = moduleId;
            TimeFrame = timeFrame;
            TimePeriod = timePeriod;
            InterfaceLanguage = interfaceLanguage;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.Guid)]
        public string Guid { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.TimeFrame)]
        public TimeFrame TimeFrame { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.TimePeriod)]
        public CacheTimePeriod TimePeriod { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.InterfaceLanguage)]
        public string InterfaceLanguage { get; set; }
        
        [JsonProperty(PropertyName = CacheKeyConstants.EntityType)]
        public Requests.EntityType EntityType { get; set; }
    }
}
