// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RadarCacheKeyGnerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using DowJones.Utilities.Managers;
using DowJones.Utilities.Managers.Search;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Radar
{
    public abstract class AbstractRadarCacheKeyGnerator<T> : AbstractModuleCacheKeyGenerator<T>
    {
        protected AbstractRadarCacheKeyGnerator(string moduleId, Product product = Product.Np) : base(moduleId, product)
        {
            Guid = moduleId;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.Guid)]
        public string Guid { get; set; }
    }

    public sealed class RadarSearchCompanyNavigatorCacheKeyGnerator : AbstractRadarCacheKeyGnerator<RadarSearchCompanyNavigatorCacheKeyGnerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "Radar_Search";

        public RadarSearchCompanyNavigatorCacheKeyGnerator() 
            : base(string.Empty)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public RadarSearchCompanyNavigatorCacheKeyGnerator(string moduleId, TimeFrame timeFrame, Product product = Product.Np)
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

    public sealed class RadarSearchCompanyScreeningCacheKeyGnerator : AbstractRadarCacheKeyGnerator<RadarSearchCompanyScreeningCacheKeyGnerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "Radar_Screening";

        public RadarSearchCompanyScreeningCacheKeyGnerator() 
            : base(string.Empty)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public RadarSearchCompanyScreeningCacheKeyGnerator(string moduleId, TimeFrame timeFrame, Product product = Product.Np)
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

    public sealed class RadarSearchSymbologyCacheKeyGnerator : AbstractRadarCacheKeyGnerator<RadarSearchSymbologyCacheKeyGnerator>
    {
        private const int DefaultCacheExpirationTime = 4 * 60;
        private const int DefaultCacheRefreshInterval = 0;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private const CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheExiprationPolicy.Absolute;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "Radar_Symbology";

        public RadarSearchSymbologyCacheKeyGnerator() 
            : base(string.Empty)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public RadarSearchSymbologyCacheKeyGnerator(string moduleId, List<string> codes, string interfaceLangage = "en", Product product = Product.Np)
            : base(moduleId, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;

            Codes = codes;
            InterfaceLanguage = interfaceLangage;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.FCodes)]
        public List<string> Codes { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.InterfaceLanguage)]
        public string InterfaceLanguage { get; set; }
    }
}