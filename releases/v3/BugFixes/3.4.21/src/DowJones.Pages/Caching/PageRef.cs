// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageRef.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Text;
using DowJones.Caching;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Infrastructure;
using DowJones.Infrastructure.Common;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Newtonsoft.Json;

namespace DowJones.Pages.Caching
{
    public sealed class PageRef : AbstractCacheKey
    {
        public static bool CachingEnabled = CacheKeyConstants.IncludeCacheKeyGeneration && Settings.Default.CachePageItems;

        internal const string VersionID = "V1";

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


        public PageRef(int pageId, Product product, bool forceCacheRefresh = DefaultCacheForceCacheRefresh) 
            : base(product)
        {
            Name = DefaultName;
            CacheForceCacheRefresh = forceCacheRefresh;
            IsValidCacheKey = false;
            PageId = pageId;
        }

        public PageRef(string pageRef, Product product, bool forceCacheRefresh = DefaultCacheForceCacheRefresh)
            : base(product)
        {
            Name = DefaultName;
            Guard.IsNotNullOrEmpty(pageRef, "pageRef");
            ParsePageRef(pageRef, forceCacheRefresh);
        }

        [JsonIgnore]
        public int PageId
        {
            get { return _pageId; }
            set { _pageId = value; }
        }
        private int _pageId;

        [JsonProperty(PropertyName = CacheKeyConstants.PageId)]
        public string ParentId { get; set; }

        [JsonIgnore]
        public string CachedPageId
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(ProductId);
                sb.Append("_");
                sb.Append(VersionID);
                sb.Append("_");
                sb.Append(PageId);
                sb.Append("_");
                sb.Append(ParentId);
                sb.Append("_");
                sb.Append(((int)PageAccessControlScope).ToString("X2"));
                sb.Append(((int)PageAccessQualifier).ToString("X2"));
                sb.Append(((int)RootAccessControlScope).ToString("X2"));
                return sb.ToString();
            }
            set
            {
                if (!value.HasValue())
                    return;

                ParsePageRef(value);
            }
        }

        [JsonProperty(PropertyName = CacheKeyConstants.RootAccessControlScope)]
        public AccessControlScope RootAccessControlScope
        {
            get { return _rootAccessControlScope; }
            set
            {
                _rootAccessControlScope = value;

                switch (_rootAccessControlScope)
                {
                    case AccessControlScope.Everyone:
                        CacheScope = DefaultGlobalCacheScope;
                        CacheExpirationTime = DefaultGlobalCacheExpirationTime;
                        CacheRefreshInterval = DefaultGlobalCacheRefreshInterval;
                        CacheExiprationPolicy = DefaultGlobalCacheExiprationPolicy;
                        break;
                    case AccessControlScope.Account:
                        CacheScope = DefaultAccountCacheScope;
                        CacheExpirationTime = DefaultAccountCacheExpirationTime;
                        CacheRefreshInterval = DefaultAccountCacheRefreshInterval;
                        CacheExiprationPolicy = DefaultAccountCacheExiprationPolicy;
                        break;
                    case AccessControlScope.Personal:
                        CacheScope = DefaultUserCacheScope;
                        CacheExpirationTime = DefaultUserCacheExpirationTime;
                        CacheRefreshInterval = DefaultUserCacheRefreshInterval;
                        CacheExiprationPolicy = DefaultUserCacheExiprationPolicy;
                        break;
                }
            }
        }
        private AccessControlScope _rootAccessControlScope;

        [JsonProperty(PropertyName = CacheKeyConstants.PageAccessControlScope)]
        public AccessControlScope PageAccessControlScope { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.PageAccessQualifier)]
        public Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier PageAccessQualifier { get; set; }

        [JsonIgnore]
        public bool IsValidCacheKey { get; private set; }

        public PageRef Clone(string pageRef)
        {
            var clone = new PageRef(CachedPageId, new Product(ProductId, ""), CacheForceCacheRefresh)
                            {
                                PageAccessControlScope = PageAccessControlScope,
                                PageAccessQualifier = PageAccessQualifier,
                                RootAccessControlScope = RootAccessControlScope,
                                CachedPageId = pageRef,
                            };

            return clone;
        }

        public override void Populate(string serializedCacheKey)
        {
            if (!ParseCachedPageRef(serializedCacheKey))
                base.Populate(serializedCacheKey);
        }

        public override string ToString()
        {
            return IsValidCacheKey ? CachedPageId : PageId.ToString();
        }

        private void ParseAccessPermissions(string accessPermissions)
        {
            if (accessPermissions == null || accessPermissions.Length != 6)
                return;

            PageAccessControlScope = (AccessControlScope)Int32.Parse(accessPermissions.Substring(0, 2), NumberStyles.HexNumber);
            PageAccessQualifier = (Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier)Int32.Parse(accessPermissions.Substring(2, 2), NumberStyles.HexNumber);
            RootAccessControlScope = (AccessControlScope)Int32.Parse(accessPermissions.Substring(4, 2), NumberStyles.HexNumber);
        }

        private bool ParseCachedPageRef(string pageId, bool forceCacheRefresh = DefaultCacheForceCacheRefresh)
        {
            if (pageId.Contains("_" + VersionID + "_"))
            {
                var temp = pageId.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (temp.Length == 5)
                {
                    TryParseNumericPageID(temp[2]);

                    ProductId = temp[0];
                    ParentId = temp[3];
                    ParseAccessPermissions(temp[4]);
                    
                    IsValidCacheKey = true;
                }
            }

            CacheForceCacheRefresh = forceCacheRefresh;

            return IsValidCacheKey;
        }

        private void ParsePageRef(string pageRef, bool forceCacheRefresh = DefaultCacheForceCacheRefresh)
        {
            try
            {
                IsValidCacheKey = false;
                
                if (TryParseNumericPageID(pageRef))
                    return;

                ParseCachedPageRef(pageRef, forceCacheRefresh);
            }
            catch (DowJonesUtilitiesException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DowJonesUtilitiesException(ex, DowJonesUtilitiesException.InvalidCacheKey);
            }
        }

        private bool TryParseNumericPageID(string pageId)
        {
            return Int32.TryParse(pageId, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out _pageId);
        }
    }
}
