using System.Diagnostics;
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

		const string Url = "http://localhost:9091/";
		private static readonly ILog Log = LogManager.GetLogger(typeof(DataSourcesHub));

		public DataSourcesHub()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			//if (!Debugger.IsAttached)
			//	Debugger.Launch();

			// Set up logging 
			XmlConfigurator.Configure();

			// Set up Dependency resolver
			IKernel kernel = new StandardKernel(new DependenciesModule(), new DataSourcesModule());

			GlobalHost.DependencyResolver = new NinjectDependencyResolver(kernel);

			var server = new Server(Url);


			// Map connections
			server.MapHubs();

			// Start the server
			server.Start();

			if (Log.IsInfoEnabled)
			{
				Log.InfoFormat("Server running on {0}", Url);
			};

			// Start the DataSources
			var initializationTask = kernel.Get<DataSourcesInitializationTask>();
			initializationTask.Execute();
		}

		protected override void OnStop()
		{
		}

	}
}
