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

        private readonly IDataSourceRepository _repository;

        public DataSourcesInitializationTask(IDataSourceRepository repository)
        {
            _repository = repository;
        }

        public void Execute()
        {
            Log.Info("Initializing data sources...");

            var datasources = _repository.GetDataSources();
            foreach (var dataSource in datasources)
            {
                var name = dataSource.Name;

                dataSource.DataReceived += (sender, args) =>
                    Dashboard.Publish(new DashboardMessage(name, args.Data));

                dataSource.Error += (sender, args) => {
                    LogManager.GetLogger(sender.GetType()).Warn("Error retriving data", args.Exception);
                    Dashboard.Publish(new DashboardErrorMessage(name, args.Exception));
                };
                
                dataSource.Start();

                Log.DebugFormat("Started {0}", dataSource.GetType().Name);
            }

            Log.Info("Data sources initialized.");
        }
    }
}