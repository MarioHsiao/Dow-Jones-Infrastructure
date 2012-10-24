using System.Collections.Generic;
using System.Timers;
using DowJones.Dash.Caching;
using DowJones.Dash.Website.Hubs;
using DowJones.Exceptions;
using DowJones.Infrastructure;
using Microsoft.Isam.Esent.Interop;
using SignalR.Client.Hubs;
using log4net;

namespace DowJones.Dash.Website.App_Start
{
    public class InitializationTask : IBootstrapperTask
    {
        private readonly HubClientConnection _clientConnection;

        public InitializationTask(HubClientConnection clientConnection)
        {
            _clientConnection = clientConnection;
        }

        #region IBootstrapperTask Members

        public void Execute()
        {
            _clientConnection.Start();
        }

        #endregion
    }

    public class HubClientConnection
    {
        private const string Url = "http://localhost:9091";
        private static readonly ILog Log = LogManager.GetLogger(typeof (HubClientConnection));
        private static Timer _timer;
        private HubConnection _connection;
        private IHubProxy _hubProxy;

        public void Ping(object sender, ElapsedEventArgs args)
        {
            if (_hubProxy != null)
            {
                _hubProxy.Invoke<ICollection<DashboardMessage>>("Pickup").ContinueWith(interiorTask =>
                    {
                        if (interiorTask.IsFaulted)
                        {
                            if (interiorTask.Exception != null) Log.ErrorFormat("An error occurred during the method call {0}", interiorTask.Exception.GetBaseException());
                        }
                        else
                        {
                            Log.InfoFormat("Successfully called Pickup");
                            foreach (DashboardMessage dashboardMessage in interiorTask.Result)
                            {
                                Log.InfoFormat("Publishing Message {0}", dashboardMessage.Source);
                                Dashboard.Publish(dashboardMessage);
                            }
                        }
                    });
            }
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
                                    foreach (DashboardMessage dashboardMessage in interiorTask.Result)
                                    {
                                        Log.InfoFormat("Add to Cache Message {0}", dashboardMessage.Source);
                                        Dashboard.Cache.Add(dashboardMessage);
                                    }
                                }
                            }).Wait();

                        // Do the first pickup of data
                        _hubProxy.Invoke<ICollection<DashboardMessage>>("Pickup").ContinueWith(interiorTask =>
                            {
                                if (interiorTask.IsFaulted)
                                {
                                    if (interiorTask.Exception != null) Log.ErrorFormat("An error occurred during the method call {0}", interiorTask.Exception.GetBaseException());
                                }
                                else
                                {
                                    Log.InfoFormat("Successfully called Pickup");
                                    foreach (DashboardMessage dashboardMessage in interiorTask.Result)
                                    {
                                        Log.InfoFormat("Publishing Message {0}", dashboardMessage.Source);
                                        Dashboard.Publish(dashboardMessage);
                                    }
                                }
                            }).Wait();

                        // Set up the timer to pick it up
                        _timer = new Timer(300);
                        _timer.Elapsed += Ping;
                        _timer.AutoReset = true;
                        _timer.Start();
                    }
                }).Wait();
           
        }

        public void Stop()
        {
            if (_connection != null)
            {
                _connection.Stop();
                _connection = null;
            }

            _hubProxy = null;

            if (_timer == null) return;
            _timer.Stop();
            _timer.Dispose();
            _timer = null;
        }
    }
}