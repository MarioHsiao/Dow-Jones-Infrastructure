// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageListCacheKey.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Infrastructure.Common;
using DowJones.Caching;

namespace DowJones.Pages.Caching
{
    public sealed class PageListCacheKey : AbstractCacheKey
    {
        public static bool CachingEnabled = CacheKeyConstants.IncludeCacheKeyGeneration && Settings.Default.CachePageListItems;

        private static readonly CacheExiprationPolicy DefaultCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const int DefaultCacheExpirationTime = 60;
        private const int DefaultCacheRefreshInterval = 30;
        private const CacheScope DefaultCacheScope = CacheScope.User;
        private const bool DefaultCacheForceCacheRefresh = false;
        private const string DefaultName = "PageList";

        public PageListCacheKey(Product product) : base(product)
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
