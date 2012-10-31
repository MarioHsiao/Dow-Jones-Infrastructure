using System;
using System.ServiceProcess;
using DowJones.Dash.Common.DependencyResolver;
using DowJones.Dash.DataSourcesServer;
using DowJones.Dash.DataSourcesServer.Module;
using Ninject;
using SignalR;
using SignalR.Hosting.Self;
using log4net;
using log4net.Config;

namespace DowJones.Dash.DataSourcesService
{
	public partial class DataSourcesHub : ServiceBase
	{
	    private static readonly string Url = Properties.Settings.Default.DataSourcesHubUrl;
	    private static readonly ILog Log = LogManager.GetLogger(typeof(DataSourcesHub));
	    private static Server _server;
	    private readonly IKernel _kernel;
	    private readonly DataSourcesManger _dataSourcesManager;

		public DataSourcesHub()
		{
		    
			InitializeComponent();

            // Set up reading from the app.config for the logging 
            XmlConfigurator.Configure();

            // Set up Dependency resolver
            _kernel = new StandardKernel(new DependenciesModule(), new DataSourcesModule());
            GlobalHost.DependencyResolver = new NinjectDependencyResolver(_kernel);

            _server = new Server(Url);

            // Map connections
            _server.MapHubs();

            // Start the DataSources
            _dataSourcesManager = _kernel.Get<DataSourcesManger>();
		}

		protected override void OnStart(string[] args)
		{
			//if (!Debugger.IsAttached)
			//	Debugger.Launch();

            try
            {
                // Start the server
                _server.Start();

                if (Log.IsInfoEnabled)
                {
                    Log.InfoFormat("Server running on {0}", Url);
                }

                // Start the DataSources
                _dataSourcesManager.Start();
            }
			catch(Exception ex)
			{
                Log.Fatal("Error On Stop", ex);
			}
		}

		protected override void OnStop()
		{
            try
            {
                _server.Stop();
                _dataSourcesManager.Suspend();
            }
            catch(Exception ex)
            {
                Log.Fatal("Error On Stop", ex);
            }
		}

	}
}
