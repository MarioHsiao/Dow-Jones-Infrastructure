using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Dash.Caching
{
    public class DashboardMessageCache : IDashboardMessageCache
    {
        private readonly IDictionary<string, DashboardMessage> _cache =
            new ConcurrentDictionary<string, DashboardMessage>();

        public void Add(DashboardMessage message)
        {
            if (_cache.ContainsKey(message.DataSource))
                _cache[message.DataSource] = message;
            else
                _cache.Add(message.DataSource, message);
        }

        public IEnumerable<DashboardMessage> Get(params string[] eventNames)
        {
            if (eventNames == null || eventNames.Length == 0)
                return _cache.Values;

            return _cache.Keys.Select(key => _cache[key]);
        }
    }
}