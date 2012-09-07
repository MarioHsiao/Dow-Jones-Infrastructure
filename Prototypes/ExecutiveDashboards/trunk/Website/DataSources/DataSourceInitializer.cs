using System.Collections.Generic;
using DowJones.Dash.Website.Hubs;
using DowJones.Infrastructure;
using SignalR;

namespace DowJones.Dash.Website.DataSources
{
    public class DataSourceInitializer : IBootstrapperTask
    {
        private readonly IEnumerable<IDataSource> _dataSources;

        public DataSourceInitializer(IEnumerable<IDataSource> dataSources)
        {
            _dataSources = dataSources;
        }

        public void Execute()
        {
            foreach (var dataSource in _dataSources)
            {
                dataSource.DataReceived += DataReceived;
            }
        }

        private static void DataReceived(object sender, DataReceivedEvent args)
        {
            var dashboardHub = GlobalHost.ConnectionManager.GetHubContext<Dashboard>();
            dashboardHub.Clients.dataReceived(sender.GetType().Name, args.Data);
        }
    }
}