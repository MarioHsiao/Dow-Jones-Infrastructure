using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowJones.Dash.Caching;
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

        public Task Reconnect(IEnumerable<string> sources)
        {
            Log.DebugFormat("Reconnect: {0}:{1} ({2})", 
                            Context.ConnectionId, Context.User.Identity.Name, string.Join(", ", sources));
            return null;
        }

        public Task<IEnumerable<DashboardMessage>> Refresh(IEnumerable<string> groups)
        {
            return Task.Factory.StartNew(() =>
                Cache.Get((groups ?? Enumerable.Empty<string>()).ToArray())
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
            Log.DebugFormat("Publishing {0}", message.EventName);
            context.messageReceived(message);
        }
    }
}