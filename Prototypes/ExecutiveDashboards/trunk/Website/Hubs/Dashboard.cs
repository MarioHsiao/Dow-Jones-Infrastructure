using DowJones.Dash.Website.DataSources;
using SignalR;
using SignalR.Hubs;

namespace DowJones.Dash.Website.Hubs
{
    public class Dashboard : Hub
    {
        public static void OnDashboardDataReceived(object sender, DataReceivedEventArgs args)
        {
            var dashboardHub = GlobalHost.ConnectionManager.GetHubContext<Dashboard>();
            dashboardHub.Clients.dataReceived(sender.GetType().Name, args.Data);
        }
    }
}