﻿using DowJones.Infrastructure;
using DowJones.Infrastructure.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Caching
{
    public abstract class AbstractInfrastructureCacheKey : ICacheKey
    {
        private static JsonSerializerSettings jsonSerializerSettings;

        [JsonProperty(PropertyName = InfrastructureCacheKeyConstants.Product, NullValueHandling = NullValueHandling.Ignore)]
        public string ProductId { get; set; }

        [JsonProperty(PropertyName = InfrastructureCacheKeyConstants.Name, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = InfrastructureCacheKeyConstants.Version, NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty(PropertyName = InfrastructureCacheKeyConstants.Enviroment, NullValueHandling = NullValueHandling.Ignore)]
        public string Environment { get; set; }

        [JsonProperty(PropertyName = InfrastructureCacheKeyConstants.Guid)]
        public string Guid { get; set; }

        [JsonIgnore]
        public virtual CacheScope CacheScope { get; set; }

        [JsonIgnore]
        public virtual int CacheExpirationTime { get; set; }

        [JsonIgnore]
        public virtual int CacheRefreshInterval { get; set; }

        [JsonIgnore]
        public virtual CacheExiprationPolicy CacheExiprationPolicy { get; set; }

        [JsonIgnore]
        public virtual bool CacheForceCacheRefresh { get; set; }

        [JsonIgnore]
        public virtual string CacheApplication { get; set; }

        protected AbstractInfrastructureCacheKey()
        {
            Version = Settings.Default.Version;
            Environment = Settings.Default.Environment;
            InitializeJsonSerializer();
        }

        protected AbstractInfrastructureCacheKey(IProduct product) : this()
        {
            if (product != null && !string.IsNullOrEmpty(product.Id))
            {
                ProductId = product.Id;
            }
        }

        protected void InitializeJsonSerializer()
        {
            if (jsonSerializerSettings != null) return;
            jsonSerializerSettings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
            jsonSerializerSettings.Converters.Add(new StringEnumConverter());
            jsonSerializerSettings.Converters.Add(new IsoDateTimeConverter());
        }

        public virtual void Populate(string serializedCacheKey)
        {
            Guard.IsNotNull(serializedCacheKey, "serializedCacheKey");
            JsonConvert.PopulateObject(serializedCacheKey, this, jsonSerializerSettings);
        }

        public virtual string Serialize()
        {   
            // Replace invalid cache keys with _
            return JsonConvert.SerializeObject(this, Formatting.None, jsonSerializerSettings)
                .Replace('.', '_')
                .Replace('&', '_')
                .Replace('>', '_')
                .Replace('<', '_');
        }
    }
}