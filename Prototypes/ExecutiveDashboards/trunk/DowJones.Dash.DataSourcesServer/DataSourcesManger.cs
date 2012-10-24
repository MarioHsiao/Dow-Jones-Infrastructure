using System.Collections.Generic;
using System.Linq;
using DowJones.Dash.Caching;
using DowJones.Dash.Common.DataSources;
using DowJones.Dash.DataSourcesServer.Hub;
using log4net;

namespace DowJones.Dash.DataSourcesServer
{
    public class DataSourcesManger
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataSourcesManger));

        private readonly IEnumerable<IDataSource> _dataSources;

        public DataSourcesManger(IEnumerable<IDataSource> dataSources)
        {
            _dataSources = dataSources;


            Log.Info("Initializing data sources...");
            foreach (var dataSource in _dataSources)
            {
                var sourceName = dataSource.Name;

                dataSource.DataReceived += (sender, args) =>
                {
                    LogManager.GetLogger(sender.GetType()).DebugFormat("Successfull -> {0}", sourceName);
                    DataSourcesHub.Publish(new DashboardMessage(sourceName, args.Name, args.Data));
                };

                dataSource.Error += (sender, args) =>
                {
                    LogManager.GetLogger(sender.GetType()).Warn("Error retrieving data" + sourceName, args.Exception);
                    DataSourcesHub.Publish(new DashboardErrorMessage(sourceName, args.Name, args.Exception));
                };
            }
        }

        public void Start()
        {
            foreach (var dataSource in _dataSources)
            {
                dataSource.Start();
                Log.DebugFormat("Started {0}", dataSource.GetType().Name);
            }

            Log.Info("Data sources started.");
        }

        public void Suspend()
        {
            Log.Info("Suspending data sources...");
            foreach (var dataSource in from dataSource in _dataSources let sourceName = dataSource.Name select dataSource)
            {
                dataSource.Suspend();
                Log.DebugFormat("Suspended {0}", dataSource.GetType().Name);
            }

            Log.Info("Data sources Suspended.");
        }
    }
}