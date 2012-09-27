using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Dash.Caching
{
    public class DashboardMessageCache : IDashboardMessageCache
    {
        protected IDictionary<string, DashboardMessage> Cache
        {
            get { return _cache; }
        }
        private readonly IDictionary<string, DashboardMessage> _cache =
            new ConcurrentDictionary<string, DashboardMessage>();

        public virtual void Add(DashboardMessage message)
        {
            if (Cache.ContainsKey(message.DataSource))
                Cache[message.DataSource] = message;
            else
                Cache.Add(message.DataSource, message);
        }

        public virtual IEnumerable<DashboardMessage> Get(params string[] dataSources)
        {
            if (dataSources == null || dataSources.Length == 0)
                return Cache.Values;

            return Cache.Keys.Select(key => _cache[key]);
        }
    }
}