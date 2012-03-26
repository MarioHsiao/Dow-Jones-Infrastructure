// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsstandCacheKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using DowJones.Utilities.Managers;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Newsstand
{
    /// <summary>
    /// The newsstand headlines cache key generator.
    /// </summary>
    public sealed class NewsstandHeadlinesCacheKeyGenerator : AbstractModuleCacheKeyGenerator<NewsstandHeadlinesCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "Newsstand_Headlines";

        public NewsstandHeadlinesCacheKeyGenerator() 
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
        /// Initializes a new instance of the <see cref="NewsstandHeadlinesCacheKeyGenerator"/> class.
        /// </summary>
        /// <param name="moduleId">The module id.</param>
        /// <param name="source">The source.</param>
        /// <param name="sectionId">The section id.</param>
        /// <param name="maxHeadlinesToReturn">The max headlines to return.</param>
        /// <param name="product">The product.</param>
        public NewsstandHeadlinesCacheKeyGenerator(string moduleId, string source, string sectionId, int maxHeadlinesToReturn, Product product = Product.Np)
            : base(moduleId, product)
        {
            Guid = string.Concat(source, "::", sectionId);
            MaxHeadlinesToReturn = maxHeadlinesToReturn;

            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        /// <summary>
        /// Gets or sets GUID.
        /// </summary>
        [JsonProperty(PropertyName = CacheKeyConstants.Guid)]
        public string Guid { get; set; }

        /// <summary>
        /// Gets or sets MaxHeadlinesToReturn.
        /// </summary>
        [JsonProperty(PropertyName = CacheKeyConstants.MaxHeadlinesToReturn)]
        public int MaxHeadlinesToReturn { get; set; }
    }

    /// <summary>
    /// The newsstand discovery cache key generator.
    /// </summary>
    public abstract class AbstractNewsstandCacheKeyGenerator<T> : AbstractModuleCacheKeyGenerator<T>
    {
        protected AbstractNewsstandCacheKeyGenerator()
            : base(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractNewsstandCacheKeyGenerator&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="moduleId">The module id.</param>
        /// <param name="newsstands">The newsstands.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="product">The product.</param>
        protected AbstractNewsstandCacheKeyGenerator(string moduleId, IEnumerable<Factiva.Gateway.Messages.Assets.Pages.V1_0.Newsstand> newsstands, string interfaceLanguage, Product product = Product.Np)
            : base(moduleId, product)
        {
            Guid = moduleId;
            InterfaceLanguage = interfaceLanguage;
            SectionIds = new List<string>();
            foreach (var newsstand in newsstands)
            {
                SectionIds.Add(string.Concat(newsstand.SourceID, "::", newsstand.SectionID));
            }
        }

        /// <summary>
        /// Gets or sets GUID.
        /// </summary>
        [JsonProperty(PropertyName = CacheKeyConstants.Guid)]
        public string Guid { get; set; }

        /// <summary>
        /// Gets or sets InterfaceLanguage.
        /// </summary>
        [JsonProperty(PropertyName = CacheKeyConstants.InterfaceLanguage)]
        public string InterfaceLanguage { get; set; }

        /// <summary>
        /// Gets or sets SectionIds.
        /// </summary>
        [JsonProperty(PropertyName = CacheKeyConstants.SectionIds)]
        public List<string> SectionIds { get; set; }
    }

    /// <summary>
    /// The newsstand discovery cache key generator.
    /// </summary>
    public sealed class NewsstandCountsCacheKeyGenerator : AbstractNewsstandCacheKeyGenerator<NewsstandCountsCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;

        /// <summary>
        /// The default name.
        /// </summary>
        private const string DefaultName = "Newsstand_Counts";

        public NewsstandCountsCacheKeyGenerator() 
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsstandCountsCacheKeyGenerator"/> class.
        /// </summary>
        /// <param name="moduleId">The module id.</param>
        /// <param name="newsstands">The newsstands.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="product">The product.</param>
        public NewsstandCountsCacheKeyGenerator(string moduleId, IEnumerable<Factiva.Gateway.Messages.Assets.Pages.V1_0.Newsstand> newsstands, string interfaceLanguage, Product product = Product.Np)
            : base(moduleId, newsstands, interfaceLanguage, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }
    }

    /// <summary>
    /// The newsstand discovery cache key generator.
    /// </summary>
    public sealed class NewsstandDiscoveryCacheKeyGenerator : AbstractNewsstandCacheKeyGenerator<NewsstandDiscoveryCacheKeyGenerator>
    {
        private static readonly int DefaultCacheExpirationTime = CacheKeyConstants.DefaultCacheExpirationTime;
        private static readonly int DefaultCacheRefreshInterval = CacheKeyConstants.DefaultCacheRefreshInterval;
        private const CacheScope DefaultCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;

        /// <summary>
        /// The default name.
        /// </summary>
        private const string DefaultName = "Newsstand_Discovery";

        public NewsstandDiscoveryCacheKeyGenerator()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsstandDiscoveryCacheKeyGenerator"/> class.
        /// </summary>
        /// <param name="moduleId">The module id.</param>
        /// <param name="newsstands">The newsstands.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="product">The product.</param>
        public NewsstandDiscoveryCacheKeyGenerator(string moduleId, IEnumerable<Factiva.Gateway.Messages.Assets.Pages.V1_0.Newsstand> newsstands, string interfaceLanguage, Product product = Product.Np)
            : base(moduleId, newsstands, interfaceLanguage, product)
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }
    }
}