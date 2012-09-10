using System.Collections.Generic;
using DowJones.Dash.Website.DataSources;
using DowJones.Dash.Website.Hubs;
using DowJones.Infrastructure;

namespace DowJones.Dash.Website.App_Start
{
    public class DataSourcesInitializationTask : IBootstrapperTask
    {
        private readonly IEnumerable<IDataSource> _dataSources;

        public DataSourcesInitializationTask(IEnumerable<IDataSource> dataSources)
        {
            _dataSources = dataSources;
        }

        public void Execute()
        {
            foreach (var dataSource in _dataSources)
            {
                dataSource.DataReceived += (sender, args) =>
                    Dashboard.Publish("data." + ((IDataSource)sender).Name, args.Data);
            }
        }
    }
}