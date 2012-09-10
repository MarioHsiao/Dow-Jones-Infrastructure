using System.Collections.Generic;
using System.Threading.Tasks;
using DowJones.Dash.Website.DataSources;
using SignalR;
using SignalR.Hubs;

namespace DowJones.Dash.Website.Hubs
{
    public class Dashboard : Hub, IConnected
    {
        private static readonly IDictionary<string,object> DataCache = new Dictionary<string, object>();


        public Task Connect()
        {
            return Task.Factory.StartNew(PushCachedData);
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            return Task.Factory.StartNew(PushCachedData);
        }

        private void PushCachedData()
        {
            var dataCache = DataCache;
            foreach (var eventName in dataCache.Keys)
            {
                Caller.dataReceived(eventName, dataCache[eventName]);
            }
        }


        public static void OnDashboardDataReceived(object sender, DataReceivedEventArgs args)
        {
            var dashboardHub = GlobalHost.ConnectionManager.GetHubContext<Dashboard>();
            var eventName = sender.GetType().Name;
            var eventData = args.Data;

            // Publish the event to all clients
            dashboardHub.Clients.dataReceived(eventName, eventData);

            // Cache the latest version of the event for future clients
            DataCache[eventName] = eventData;
        }
    }
}