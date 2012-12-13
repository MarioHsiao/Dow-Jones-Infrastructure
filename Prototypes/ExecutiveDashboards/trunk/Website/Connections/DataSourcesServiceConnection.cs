using System;
using System.Collections.Generic;
using System.Timers;
using DowJones.Dash.Caching;
using DowJones.Dash.Website.Hubs;
using SignalR;
using SignalR.Client.Hubs;
using SignalR.Hubs;
using log4net;

namespace DowJones.Dash.Website.Connections
{
    public class DataSourcesServiceConnection
    {
        private readonly static Lazy<DataSourcesServiceConnection> PrivateInstance = new Lazy<DataSourcesServiceConnection>(() => new DataSourcesServiceConnection());
        private static readonly string Url = Properties.Settings.Default.DataSourcesHubUrl;
        private static readonly ILog Log = LogManager.GetLogger(typeof (DataSourcesServiceConnection));
        private HubConnection _connection;
        private IHubProxy _hubProxy;

        private readonly Lazy<IHubContext> _clientsInstance = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<DashboardHub>());

        private DataSourcesServiceConnection()
        {
        }

        public static DataSourcesServiceConnection Instance
        {
            get
            {
                return PrivateInstance.Value;
            }
        }

        private IHubContext DashboardHub
        {
            get { return _clientsInstance.Value; }
        }

        public void Start()
        {
            _connection = new HubConnection(Url);
            _hubProxy = _connection.CreateProxy("DataSourcesHub");
            _connection.Start().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        if (task.Exception != null) Log.ErrorFormat("Failed to start: {0}", task.Exception.GetBaseException());
                    }
                    else
                    {
                        Log.InfoFormat("Success! Connected with client connection id {0}", _connection.ConnectionId);

                        // Prime the cache for first load
                        _hubProxy.Invoke<ICollection<DashboardMessage>>("PrimeCache").ContinueWith(interiorTask =>
                            {
                                if (interiorTask.IsFaulted)
                                {
                                    if (interiorTask.Exception != null) Log.ErrorFormat("An error occurred during the method call {0}", interiorTask.Exception.GetBaseException());
                                }
                                else
                                {
                                    Log.InfoFormat("Successfully called PrimeCache");
                                    foreach (var dashboardMessage in interiorTask.Result)
                                    {
                                        Log.InfoFormat("Add to Cache Message {0}", dashboardMessage.Source);
                                        Hubs.DashboardHub.Cache.Add(dashboardMessage);
                                    }
                                }
                            }).Wait();

                        _hubProxy.On<DashboardMessage>("Message", Publish);

                    }
                }).Wait();
        }

        public void Publish(DashboardMessage message)
        {
            if (message == null)
                return;

            Log.DebugFormat("Publishing {0}", message.EventName);
            //DashboardHub.Clients.ping(message.Source);

            if (!string.IsNullOrWhiteSpace(message.Source))
            {
                var subscribers = DashboardHub.Clients[message.Source];
                subscribers.messageReceived(message);
            }

            if (!(message is DashboardErrorMessage))
            {
                Hubs.DashboardHub.Cache.Add(message);
            }
        }

        public void Stop()
        {
            if (_connection != null)
            {
                _connection.Stop();
                _connection = null;
            }

            _hubProxy = null;
        }
    }
}