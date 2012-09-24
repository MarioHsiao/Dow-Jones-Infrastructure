using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalR;
using SignalR.Hubs;

namespace DowJones.Dash.Website.Hubs
{
    public class DashboardMessageCache
    {
        private readonly IDictionary<string, DashboardMessage> _cache = 
            new ConcurrentDictionary<string, DashboardMessage>();

        public void Add(DashboardMessage message)
        {
            if(_cache.ContainsKey(message.EventName))
                _cache[message.EventName] = message;
            else
                _cache.Add(message.EventName, message);
        }

        public IEnumerable<DashboardMessage> Get(params string[] eventNames)
        {
            if (eventNames == null || eventNames.Length == 0)
                return _cache.Values;

            return _cache.Keys.Select(key => _cache[key]);
        }
    }

    public class DashboardMessage
    {
        public string EventName { get; set; }
        public object Data { get; set; }

        public DashboardMessage(string eventName = null, object data = null)
        {
            EventName = eventName;
            Data = data;
        }
    }

    public class Dashboard : Hub, IConnected
    {
        private static readonly DashboardMessageCache Cache = new DashboardMessageCache();

        public Task Connect()
        {
            return Task.Factory.StartNew(() => PushCachedMessages());
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            return Task.Factory.StartNew(() => PushCachedMessages(groups));
        }

        public static void Publish(DashboardMessage message, dynamic context = null)
        {
            context = context ?? GlobalHost.ConnectionManager.GetHubContext<Dashboard>().Clients;
            context.publish(message.EventName, message.Data);

            // Cache the latest version of the event for future clients
            Cache.Add(message);
        }

        private void PushCachedMessages(IEnumerable<string> groups = null)
        {
            var messages = Cache.Get((groups ?? Enumerable.Empty<string>()).ToArray());
            foreach (var message in messages)
            {
                Publish(message, Caller);
            }
        }
    }
}