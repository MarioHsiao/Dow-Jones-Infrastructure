// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SourcesCacheKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Managers;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Alerts;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Sources
{
    public sealed class SourcesCacheKeyGenerator : AbstractModuleCacheKeyGenerator<TopNewsCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "Sources";

        public SourcesCacheKeyGenerator() 
            : base(string.Empty)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public SourcesCacheKeyGenerator(string moduleId, string sourceCode, int maxHeadlinesToReturn, string interfaceLanguage = "en", Product product = Product.Np)
            : base(moduleId, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;

            SourceCode = sourceCode;
            MaxHeadlinesToReturn = maxHeadlinesToReturn;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.Guid)]
        public string SourceCode { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.MaxHeadlinesToReturn)]
        public int MaxHeadlinesToReturn { get; set; }
    }
}
