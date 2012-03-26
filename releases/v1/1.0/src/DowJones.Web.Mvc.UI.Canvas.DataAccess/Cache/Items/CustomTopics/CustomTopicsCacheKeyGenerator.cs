// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomTopicsCacheKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Managers;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Alerts
{
    public sealed class CustomTopicsCacheKeyGenerator : AbstractModuleCacheKeyGenerator<CustomTopicsCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.User;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "CustomTopics";

        public CustomTopicsCacheKeyGenerator() 
            : base(string.Empty)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTopicsCacheKeyGenerator"/> class.
        /// </summary>
        /// <param name="moduleId">The module id.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="title">The title.</param>
        /// <param name="maxHeadlinesToReturn">The max headlines to return.</param>
        /// <param name="contentLanguages">The content languages.</param>
        /// <param name="product">The product.</param>
        public CustomTopicsCacheKeyGenerator(string moduleId, string identifier, string title, int maxHeadlinesToReturn, string[] contentLanguages, Product product = Product.Np)
            : base(moduleId, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;

            CustomTopicId = string.Concat(identifier, "::", title);
            MaxHeadlinesToReturn = maxHeadlinesToReturn;
            ContentLanguages = contentLanguages;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.Guid)]
        public string CustomTopicId { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.MaxHeadlinesToReturn)]
        public int MaxHeadlinesToReturn { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.ContentLanguages)]
        public string[] ContentLanguages { get; set; }
    }
}
