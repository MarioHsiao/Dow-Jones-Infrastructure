using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace DowJones.Factiva.Currents.Aggregrator
{
    public class CacheManager
    {
        static ObjectCache defaultCache = MemoryCache.Default;
        private bool cacheEnabled = DowJones.Factiva.Currents.Common.Utilities.Web.IsCacheEnabled();

        public CacheManager()
        {
        }

        public void Add(string key, object value)
        {
            if (!cacheEnabled)
                return;
            Remove(key);
            int cacheExpiration = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CacheExpiration"].ToString());
            defaultCache.Set(key, value, new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddMinutes(cacheExpiration) });
        }

        public object GetCache(string key)
        {
            return defaultCache[key];
        }

        public void Update(string key,object value)
        {
            defaultCache[key] = value;
        }

        public void Remove(string key)
        {
            if(defaultCache.Contains(key))
                defaultCache.Remove(key);
        }

        public void Flush()
        {
            foreach (var key in defaultCache.Select(c=>c.Key).ToList())
            {
                defaultCache.Remove(key);
            }
        }
    }
}
