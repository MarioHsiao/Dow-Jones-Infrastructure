using System.Collections.Generic;
using System.Timers;
using DowJones.Dash.Caching;
using DowJones.Dash.Website.Hubs;
using SignalR.Client.Hubs;
using DowJones.Infrastructure;
using log4net;

namespace DowJones.Dash.Website.App_Start
{
    public class InitializationTask: IBootstrapperTask
    {
        private readonly HubClientConnection _clientConnection;

        public InitializationTask(HubClientConnection clientConnection)
        {
            _clientConnection = clientConnection;
        }

        public void Execute()
        {
            _clientConnection.Start();
        }
    }

    public class HubClientConnection 
    {
        const string Url = "http://localhost:9091";
        private static readonly ILog Log = LogManager.GetLogger(typeof(HubClientConnection));
        private HubConnection _connection;
        private IHubProxy _hubProxy;
        private static Timer _timer;
        
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
                            foreach (var dashboardMessage in interiorTask.Result)
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
                                        Dashboard.Cache.Add(dashboardMessage);
                                    } 
                                }
                            }).Wait();

                    }
                }).Wait();
            
            _timer = new Timer(300);
            _timer.Elapsed += Ping;
            _timer.AutoReset = true;
            _timer.Start();

            _hubProxy.Invoke<ICollection<DashboardMessage>>("Pickup").ContinueWith(interiorTask =>
            {
                if (interiorTask.IsFaulted)
                {
                    if (interiorTask.Exception != null) Log.ErrorFormat("An error occurred during the method call {0}", interiorTask.Exception.GetBaseException());
                }
                else
                {
                    Log.InfoFormat("Successfully called Pickup");
                    foreach (var dashboardMessage in interiorTask.Result)
                    {
                        Log.InfoFormat("Publishing Message {0}", dashboardMessage.Source);
                        Dashboard.Publish(dashboardMessage);
                    }
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