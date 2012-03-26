// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TopNewsCacheKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Managers;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Alerts
{
    public sealed class TopNewsCacheKeyGenerator : AbstractModuleCacheKeyGenerator<TopNewsCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.User;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "TopNews";

        public TopNewsCacheKeyGenerator()
            : base(string.Empty)
        {
        }

        public TopNewsCacheKeyGenerator(string moduleId, TopNewsModulePart part, int maxHeadlinesToReturn, string[] contentLanguages, string interfaceLanguage = "en", Product product = Product.Np)
            : base(moduleId, product)
        {
            ModulePart = part;
            MaxHeadlinesToReturn = maxHeadlinesToReturn;
            InterfaceLanguage = interfaceLanguage;
            ContentLanguages = contentLanguages;

            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.Guid)]
        public TopNewsModulePart ModulePart { get; set; }
        
        [JsonProperty(PropertyName = CacheKeyConstants.ContentLanguages)]
        public string[] ContentLanguages { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.MaxHeadlinesToReturn)]
        public int MaxHeadlinesToReturn { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.InterfaceLanguage)]
        public string InterfaceLanguage { get; set; }
    }
}
