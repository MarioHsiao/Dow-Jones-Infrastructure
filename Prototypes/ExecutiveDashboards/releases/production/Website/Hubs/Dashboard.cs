using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowJones.Dash.Caching;
using Newtonsoft.Json;
using SignalR;
using SignalR.Hubs;
using log4net;

namespace DowJones.Dash.Website.Hubs
{
    public class Dashboard : Hub, IConnected
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Dashboard));

        private static IDashboardMessageCache Cache
        {
            get
            {
                return DependencyInjection.ServiceLocator.Resolve<IDashboardMessageCache>();
            }
        }

        public Task Connect()
        {
            Log.DebugFormat("Connect: {0}:{1}", 
                            Context.ConnectionId, Context.User.Identity.Name);
            return null;
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            Log.DebugFormat("Reconnect: {0}:{1} ({2})", 
                            Context.ConnectionId, Context.User.Identity.Name, string.Join(", ", groups));
            return null;
        }
        
        public Task<dynamic> Refresh(IEnumerable<string> groups)
        {
            return Task.Factory.StartNew(() =>
                (dynamic)
                Cache
                    .Get((groups ?? Enumerable.Empty<string>()).ToArray())
                    .Select(x => new ClientEvent(x))
            );
        }

        public static void Publish(DashboardMessage message)
        {
            Publish(message, GlobalHost.ConnectionManager.GetHubContext<Dashboard>().Clients);

            if (!(message is DashboardErrorMessage))
                Cache.Add(message);
        }

        private static void Publish(DashboardMessage message, dynamic context)
        {
            var clientEvent = new ClientEvent(message);
            Log.DebugFormat("Publishing {0}", clientEvent.EventName);
            context.publish(clientEvent.EventName, clientEvent.Data);
        }

        class ClientEvent
        {
            [JsonProperty("eventName")]
            public string EventName { get; private set; }

            [JsonProperty("data")]
            public object Data { get; private set; }

            public ClientEvent(string eventName = null, object data = null)
            {
                EventName = eventName;
                Data = data;
            }

            public ClientEvent(DashboardMessage message)
            {
                Data = message.Data;
                
                var isError = message is DashboardErrorMessage;
                var prefix = isError ? "dataError." : "data.";
                EventName = prefix + message.DataSource;
            }
        }
    }
}