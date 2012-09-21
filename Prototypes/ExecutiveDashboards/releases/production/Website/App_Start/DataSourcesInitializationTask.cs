using DowJones.Dash.DataSources;
using DowJones.Dash.Website.Hubs;
using DowJones.Infrastructure;

namespace DowJones.Dash.Website.App_Start
{
    public class DataSourcesInitializationTask : IBootstrapperTask
    {
        private readonly IDataSourceRepository _repository;

        public DataSourcesInitializationTask(IDataSourceRepository repository)
        {
            _repository = repository;
        }

        public void Execute()
        {
            var datasources = _repository.GetDataSources();
            foreach (var dataSource in datasources)
            {
                var name = dataSource.Name;

                dataSource.DataReceived += (sender, args) =>
                    Dashboard.Publish("data." + name, args.Data);

                dataSource.Error += (sender, args) =>
                    Dashboard.Publish(
                        "dataError." + name, 
                        args.Exception == null ? "Unknown error" : args.Exception.Message
                    );
                
                dataSource.Start();
            }
        }
    }
}