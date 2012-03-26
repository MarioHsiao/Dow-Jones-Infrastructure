// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Managers;
using Factiva.Gateway.Utils.V1_0;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache
{
    public enum Product
    {
        Np,
        Md,
    }

    public enum CacheTimePeriod
    {
        /// <summary>
        /// current cache time period
        /// </summary>
        Current,

        /// <summary>
        /// previous cache time period
        /// </summary>
        Previous,
    }

    internal interface ICacheKeyGenerator<out T>
    {
        /// <summary>
        /// Froms the cache key.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>The object of the base type.</returns>
        T FromCacheKey(string cacheKey);

        /// <summary>
        /// Toes the JSON.
        /// </summary>
        /// <returns>A JSON encoded string.</returns>
        string ToCacheKey();
    }

    public abstract class AbstractModuleCacheKeyGenerator<T> : AbstractCacheKeyGenerator<T>
    {
        protected AbstractModuleCacheKeyGenerator(string moduleId, Product product = Product.Np)
            : base(product)
        {
            ModuleId = moduleId;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.ModuleId)]
        public string ModuleId { get; set; }
    }

    public abstract class AbstractCacheKeyGenerator<T> : ICacheKeyGenerator<T>
    {
        protected AbstractCacheKeyGenerator(Product product = Product.Np)
        {
            Product = product;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.Product)]
        protected internal Product Product { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.Name, NullValueHandling = NullValueHandling.Ignore)]
        protected internal string Name { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.Version, NullValueHandling = NullValueHandling.Ignore)]
        protected internal string Version { get; set; }

        [JsonProperty(PropertyName = CacheKeyConstants.Enviroment, NullValueHandling = NullValueHandling.Ignore)]
        protected internal string Environment { get; set; }

        [JsonIgnore]
        protected internal virtual CacheScope CacheScope { get; set; }

        [JsonIgnore]
        protected internal virtual int CacheExpirationTime { get; set; }

        [JsonIgnore]
        protected internal virtual int CacheRefreshInterval { get; set; }

        [JsonIgnore]
        protected internal virtual CacheExiprationPolicy CacheExiprationPolicy { get; set; }

        [JsonIgnore]
        protected internal virtual bool CacheForceCacheRefresh { get; set; }

        [JsonIgnore]
        protected internal virtual string CacheApplication { get; set; }

        /// <summary>
        /// From the string.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>A cache key object</returns>
        public virtual T FromCacheKey(string cacheKey)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(cacheKey, new StringEnumConverter(), new IsoDateTimeConverter());
            }
            catch (Exception ex)
            {
                throw new DowJonesUtilitiesException(ex, DowJonesUtilitiesException.InvalidCacheKey);
            }
        }

        T ICacheKeyGenerator<T>.FromCacheKey(string cacheKey)
        {
            return FromCacheKey(cacheKey);
        }

        public ControlData GetCacheControlData(ControlData controlData)
        {
            var tempControlData = ControlDataManager.GetControlDataForTransactionCache(controlData, ToCacheKey(), CacheScope, CacheExpirationTime, CacheRefreshInterval, CacheExiprationPolicy, CacheForceCacheRefresh);

            switch (Product)
            {
                case Product.Md:
                    tempControlData.CacheApplication = "MADE";
                    break;
                case Product.Np:
                    tempControlData.CacheApplication = "SNAPSHOT";
                    break;
            }

            return tempControlData;
        }

        public virtual string ToCacheKey()
        {
            Version = Properties.Settings.Default.Version;
            Environment = Properties.Settings.Default.Environment;
            return JsonConvert.SerializeObject(this, new StringEnumConverter(), new IsoDateTimeConverter());
        }
    }

    internal class CacheKeyConstants
    {
        public const string Name = "n";
        public const string Product = "p";
        public const string PageId = "pid";
        public const string ModuleId = "mid";
        public const string RootId = "rid";
        public const string InterfaceLanguage = "il";
        public const string RootAccessControlScope = "racs";
        public const string PageAccessControlScope = "pacs";
        public const string PageAccessQualifier = "paq";
        public const string CacheScope = "cs";
        public const string ContentLanguages = "cl";
        public const string NumberOfHeadlines = "nhr";
        public const string Guid = "guid";
        public const string MaxHeadlinesToReturn = "mhr";
        public const string TimeFrame = "tf";
        public const string TimePeriod = "tp";
        public const string SectionIds = "secids";
        public const string FCodes = "codes";
        public const string ReportType = "rt";
        public const string RegionCode = "rc";
        public const string Version = "v";
        public const string Enviroment = "e";
        public const string EntityType = "et";
        private static readonly int SettingsDefaultCacheExpirationTime = Properties.Settings.Default.DefaultCacheExpirationTime;
        private static readonly int SettingsDefaultCacheRefreshInterval = Properties.Settings.Default.DefaultCacheRefreshInterval;
        private static readonly CacheExiprationPolicy SettingsDefaultCacheExiprationPolicy = Properties.Settings.Default.DefaultCacheExpirationPolicy;

        public static int DefaultCacheExpirationTime
        {
            get { return SettingsDefaultCacheExpirationTime; }
        }

        public static int DefaultCacheRefreshInterval
        {
            get { return SettingsDefaultCacheRefreshInterval; }
        }

        public static CacheExiprationPolicy DefaultCacheExiprationPolicy
        {
            get { return SettingsDefaultCacheExiprationPolicy;  }
        }

    }
}
