using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowJones.Dash.Caching;
using SignalR;
using SignalR.Hubs;
using log4net;

namespace DowJones.Dash.Website.Hubs
{
    public class Dashboard : Hub
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Dashboard));

        private static IDashboardMessageCache Cache
        {
            get
            {
                return DependencyInjection.ServiceLocator.Resolve<IDashboardMessageCache>();
            }
        }

        public Task<IEnumerable<DashboardMessage>> Subscribe(IEnumerable<string> sources)
        {
            var clientId = Context.ConnectionId;

            return Task.Factory.StartNew(() => {
                var groups = (sources ?? Enumerable.Empty<string>()).ToArray();

                foreach (var @group in groups)
                {
                    Groups.Add(clientId, @group);
                }

                return Cache.Get(groups);
            });
        }

        public Task Unsubscribe(IEnumerable<string> sources)
        {
            var clientId = Context.ConnectionId;

            return Task.Factory.StartNew(() =>
            {
                var groups = (sources ?? Enumerable.Empty<string>()).ToArray();
                foreach (var @group in groups)
                {
                    Groups.Remove(clientId, @group);
                }
            });
        }

        public static void Publish(DashboardMessage message)
        {
            if (message == null)
                return;

            Log.DebugFormat("Publishing {0}", message.EventName);

            var context = GlobalHost.ConnectionManager.GetHubContext<Dashboard>();
            var subscribers = context.Clients;

            if (!string.IsNullOrWhiteSpace(message.Source))
            {
                subscribers = subscribers[message.Source];
                subscribers.messageReceived(message);
            }

            if (!(message is DashboardErrorMessage))
                Cache.Add(message);
        }
    }
}