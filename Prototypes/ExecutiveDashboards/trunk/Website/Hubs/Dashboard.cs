using System.Collections.Generic;
using System.Threading.Tasks;
using SignalR;
using SignalR.Hubs;

namespace DowJones.Dash.Website.Hubs
{
    public class Dashboard : Hub, IConnected
    {
        private static readonly IDictionary<string,object> MessageCache = new Dictionary<string, object>();

        public Task Connect()
        {
            return Task.Factory.StartNew(PushCachedMessages);
        }

        public static void Publish(string eventName, object data, dynamic context = null)
        {
            context = context ?? GlobalHost.ConnectionManager.GetHubContext<Dashboard>().Clients;
            context.publish(eventName, data);

            // Cache the latest version of the event for future clients
            MessageCache[eventName] = data;
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            return Task.Factory.StartNew(PushCachedMessages);
        }

        private void PushCachedMessages()
        {
            var dataCache = new Dictionary<string,object>(MessageCache);
            foreach (var eventName in dataCache.Keys)
            {
                Publish(eventName, dataCache[eventName], Caller);
            }
        }
    }
}