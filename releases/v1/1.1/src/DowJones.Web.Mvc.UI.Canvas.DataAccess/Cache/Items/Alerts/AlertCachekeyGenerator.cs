// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlertCachekeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Managers;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Alerts
{
    public sealed class AlertCacheKeyGenerator : AbstractModuleCacheKeyGenerator<AlertCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.User;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "Alert";

        public AlertCacheKeyGenerator() 
            : base(string.Empty)
        {
        }

        public AlertCacheKeyGenerator(string moduleId, string alertId, int maxHeadlinesToReturn, Product product = Product.Np)
            : base(moduleId, product)
        {
            AlertId = alertId;
            MaxHeadlinesToReturn = maxHeadlinesToReturn;

            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.Guid)]
        public string AlertId { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.MaxHeadlinesToReturn)]
        public int MaxHeadlinesToReturn { get; set; }
    }
}
