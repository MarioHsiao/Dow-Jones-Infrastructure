using System.Collections.Generic;
using DowJones.Dash.Caching;
using DowJones.Dash.DataSources;
using DowJones.Dash.Website.Hubs;
using DowJones.Infrastructure;
using log4net;

namespace DowJones.Dash.Website.App_Start
{
    public class DataSourcesInitializationTask : IBootstrapperTask
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataSourcesInitializationTask));

        private readonly IEnumerable<IDataSource> _dataSources;

        public DataSourcesInitializationTask(IEnumerable<IDataSource> dataSources)
        {
            _dataSources = dataSources;
        }

        public void Execute()
        {
            Log.Info("Initializing data sources...");

            foreach (var dataSource in _dataSources)
            {
                var sourceName = dataSource.Name;

                dataSource.DataReceived += (sender, args) =>
                    Dashboard.Publish(new DashboardMessage(sourceName, args.Name, args.Data));

                dataSource.Error += (sender, args) => {
                    LogManager.GetLogger(sender.GetType()).Warn("Error retriving data", args.Exception);
                    Dashboard.Publish(new DashboardErrorMessage(sourceName, args.Name, args.Exception));
                };
                
                dataSource.Start();

                Log.DebugFormat("Started {0}", dataSource.GetType().Name);
            }

            Log.Info("Data sources initialized.");
        }
    }
}