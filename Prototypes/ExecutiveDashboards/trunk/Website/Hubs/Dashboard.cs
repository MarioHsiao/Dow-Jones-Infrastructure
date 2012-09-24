using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalR;
using SignalR.Hubs;

namespace DowJones.Dash.Website.Hubs
{
    public class Dashboard : Hub, IConnected
    {
        private static readonly IDashboardMessageCache Cache =
            DowJones.DependencyInjection.ServiceLocator.Resolve<RavenDbMessageCache>();

        public Task Connect()
        {
            return Task.Factory.StartNew(() => PushCachedMessages());
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            return Task.Factory.StartNew(() => PushCachedMessages(groups));
        }

        private void PushCachedMessages(IEnumerable<string> groups = null)
        {
            var messages = Cache.Get((groups ?? Enumerable.Empty<string>()).ToArray());
            foreach (var message in messages)
            {
                Publish(message, Caller);
            }
        }

        public static void Publish(DashboardMessage message)
        {
            Publish(message, GlobalHost.ConnectionManager.GetHubContext<Dashboard>().Clients);

            if (!(message is DashboardErrorMessage))
                Cache.Add(message);
        }

        private static void Publish(DashboardMessage message, dynamic context)
        {
            var isError = message is DashboardErrorMessage;
            var prefix = isError ? "dataError." : "data.";

            context.publish(prefix + message.DataSource, message.Data);
        }
    }
}