// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageCacheKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Text;
using DowJones.Utilities.Managers;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Newtonsoft.Json;
using AccessQualifier = Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items
{
    public sealed class PageCacheKeyGenerator : AbstractCacheKeyGenerator<PageCacheKeyGenerator>
    {
        private const string DefaultName = "Page";
        private const int DefaultUserCacheExpirationTime = 60;
        private const int DefaultUserCacheRefreshInterval = 30;
        private const CacheScope DefaultUserCacheScope = CacheScope.User;
        private static readonly CacheExiprationPolicy DefaultUserCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;
        private const bool DefaultCacheForceCacheRefresh = false;

        private const int DefaultAccountCacheExpirationTime = 60;
        private const int DefaultAccountCacheRefreshInterval = 30;
        private const CacheScope DefaultAccountCacheScope = CacheScope.Account;
        private static readonly CacheExiprationPolicy DefaultAccountCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;

        private const int DefaultGlobalCacheExpirationTime = 24 * 60;
        private const int DefaultGlobalCacheRefreshInterval = 12 * 60;
        private const CacheScope DefaultGlobalCacheScope = CacheScope.All;
        private static readonly CacheExiprationPolicy DefaultGlobalCacheExiprationPolicy = CacheKeyConstants.DefaultCacheExiprationPolicy;

        public PageCacheKeyGenerator(Product product = Product.Np) : base(product)
        {
            Name = DefaultName;
            CacheForceCacheRefresh = DefaultCacheForceCacheRefresh;
        }

        public PageCacheKeyGenerator(string cacheKey, Product product = Product.Np) : this(product)
        {
            var tempPageCache = Process(cacheKey);
            PageId = tempPageCache.PageId;
            ParentId = tempPageCache.ParentId;
            PageAccessControlScope = tempPageCache.PageAccessControlScope;
            PageAccessQualifier = tempPageCache.PageAccessQualifier;
            RootAccessControlScope = tempPageCache.RootAccessControlScope;
        }

        public PageCacheKeyGenerator()
        {
        }

        [JsonIgnore]
        public string PageId { get; set; }

        [JsonIgnore]
        public string ParentId { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.PageId)]
        public string CachedPageId { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.RootAccessControlScope)]
        public AccessControlScope RootAccessControlScope { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.PageAccessControlScope)]
        public AccessControlScope PageAccessControlScope { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.PageAccessQualifier)]
        public AccessQualifier PageAccessQualifier { get; set; }

        public string ToReference()
        {
            var sb = new StringBuilder();
            sb.Append(Product + "_");
            sb.Append("V1_");
            sb.Append(PageId);
            sb.Append("_");
            sb.Append(ParentId);
            sb.Append("_");
            sb.Append(((int)PageAccessControlScope).ToString("X2"));
            sb.Append(((int)PageAccessQualifier).ToString("X2"));
            sb.Append(((int)RootAccessControlScope).ToString("X2"));
            return sb.ToString();
        }

        public override string ToCacheKey()
        {
            switch (RootAccessControlScope)
            {
               case AccessControlScope.Everyone:
                    CacheScope = DefaultGlobalCacheScope;
                    CacheExpirationTime = DefaultGlobalCacheExpirationTime;
                    CacheRefreshInterval = DefaultGlobalCacheRefreshInterval;
                    CacheExiprationPolicy = DefaultGlobalCacheExiprationPolicy;
                    CachedPageId = ParentId;
                    break;
               case AccessControlScope.Account:
                    CacheScope = DefaultAccountCacheScope;
                    CacheExpirationTime = DefaultAccountCacheExpirationTime;
                    CacheRefreshInterval = DefaultAccountCacheRefreshInterval;
                    CacheExiprationPolicy = DefaultAccountCacheExiprationPolicy;
                    CachedPageId = ParentId;
                    break;
                case AccessControlScope.Personal:
                    CacheScope = DefaultUserCacheScope;
                    CacheExpirationTime = DefaultUserCacheExpirationTime;
                    CacheRefreshInterval = DefaultUserCacheRefreshInterval;
                    CacheExiprationPolicy = DefaultUserCacheExiprationPolicy;
                    CachedPageId = ParentId;
                    break;
            }

            return base.ToCacheKey();
        }

        public int GetPageId()
        {
            /*if (PageId == ParentId && RootAccessControlScope == AccessControlScope.Personal)
            {
                return Int32.Parse(PageId, CultureInfo.InvariantCulture.NumberFormat);
            }
            */
            return Int32.Parse(PageId, CultureInfo.InvariantCulture.NumberFormat);
        }

        private PageCacheKeyGenerator Process(string cacheKey)
        {
            if (cacheKey.Contains("_V1_"))
            {
                var tempCacheKey = new PageCacheKeyGenerator();
                var temp = cacheKey.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (temp.Length == 5)
                {
                    tempCacheKey.Product = (temp[0].ToLowerInvariant() == "np") ? Product.Np : Product.Md;
                    tempCacheKey.PageId = temp[2];
                    tempCacheKey.ParentId = temp[3];
                    var hexString = temp[4];

                    tempCacheKey.PageAccessControlScope = (AccessControlScope)Int32.Parse(hexString.Substring(0, 2), NumberStyles.HexNumber);
                    tempCacheKey.PageAccessQualifier = (AccessQualifier)Int32.Parse(hexString.Substring(2, 2), NumberStyles.HexNumber);
                    tempCacheKey.RootAccessControlScope = (AccessControlScope)Int32.Parse(hexString.Substring(4, 2), NumberStyles.HexNumber);
                    return tempCacheKey;
                }
            }

            return FromCacheKey(cacheKey);
        }
    }
}
