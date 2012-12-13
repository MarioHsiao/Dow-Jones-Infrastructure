using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DowJones.Dash.Caching;
using DowJones.Dash.Common.DataSources;
using DowJones.Dash.DataSourcesServer.Hub;
using SignalR;
using SignalR.Hubs;
using log4net;

namespace DowJones.Dash.DataSourcesServer
{

    public class DataSourcesManger
    {
        private readonly static Lazy<DataSourcesManger> PrivateInstance = new Lazy<DataSourcesManger>(() => new DataSourcesManger());
        private static readonly string Url = Properties.Settings.Default.DataSourcesHubUrl;
        private static Timer _timer;
        private const int UpdateInterval = 250; //ms
        private IEnumerable<IDataSource> _dataSources;
        private static readonly ILog Log = LogManager.GetLogger(typeof(DataSourcesManger));
        private readonly Lazy<IHubContext> _clientsInstance = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<DataSourcesHub>());

        private DataSourcesManger()
        {
        }

        public static DataSourcesManger Instance
        {
            get
            {
                return PrivateInstance.Value;
            }
        }


        private IHubContext DataSourcesHubClients
        {
            get { return _clientsInstance.Value; }
        }
        

        public void Initialize(IEnumerable<IDataSource> dataSources)
        {
            _dataSources = dataSources;

            Log.Info("Initializing data sources...");
            foreach (var dataSource in _dataSources)
            {
                var sourceName = dataSource.Name;

                dataSource.DataReceived += (sender, args) =>
                {
                    LogManager.GetLogger(sender.GetType()).DebugFormat("Successfull -> {0}", sourceName);
                    Publish(new DashboardMessage(sourceName, args.Name, args.Data));
                };

                dataSource.Error += (sender, args) =>
                {
                    LogManager.GetLogger(sender.GetType()).Warn("Error retrieving data" + sourceName, args.Exception);
                    Publish(new DashboardErrorMessage(sourceName, args.Name, args.Exception));
                };
            }
        }

        public void Publish(DashboardMessage message)
        {
            if (message == null)
                return;
            
            if (!string.IsNullOrWhiteSpace(message.Source))
            {
                DataSourcesHubClients.Clients.Message(message);
                Log.DebugFormat("Publishing {0}", message.EventName);
            }

            if (!(message is DashboardErrorMessage))
            {
                DataSourcesHub.MessageCache.Add(message);
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