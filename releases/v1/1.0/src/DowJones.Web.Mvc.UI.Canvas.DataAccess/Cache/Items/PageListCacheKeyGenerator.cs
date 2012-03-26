// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageListCacheKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Managers;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items
{
    public sealed class PageListCacheKeyGenerator : AbstractCacheKeyGenerator<PageListCacheKeyGenerator>
    {
        private const int DefaultCacheExpirationTime = 60;
        private const int DefaultCacheRefreshInterval = 30;
        private const CacheScope DefaultCacheScope = CacheScope.User;
        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "PageList";

        public PageListCacheKeyGenerator()
        {
            Name = DefaultName;
            CacheScope = DefaultCacheScope;
            CacheExpirationTime = DefaultCacheExpirationTime;
            CacheRefreshInterval = DefaultCacheRefreshInterval;
            CacheExiprationPolicy = DefaultCacheExiprationPolicy;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public PageListCacheKeyGenerator(Product product = Product.Np)
            : base(product)
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
