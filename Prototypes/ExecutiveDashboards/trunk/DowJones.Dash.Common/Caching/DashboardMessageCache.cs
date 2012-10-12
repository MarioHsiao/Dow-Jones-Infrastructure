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
            if (message == null)
            {
                return;
            }

            if (_cache.ContainsKey(message.Source))
            {
                _cache[message.Source] = message;
            }
            else
            {
                _cache.Add(message.Source, message);
            }
        }

        public virtual IEnumerable<DashboardMessage> Get(params string[] dataSources)
        {
            if (dataSources == null || dataSources.Length == 0)
            {
                return _cache.Values;
            }

            var list = new List<DashboardMessage>();
            foreach (var dataSource in dataSources)
            {
                DashboardMessage message;
                if (_cache.TryGetValue(dataSource, out message))
                {
                    list.Add(message);
                }
            }
            return list;
        }
    }
}