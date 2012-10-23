using System;
using DowJones.Dash.Common.DependencyResolver;
using DowJones.Dash.DataSourcesServer.Hub;
using DowJones.Dash.DataSourcesServer.Module;
using Ninject;
using SignalR;
using SignalR.Hosting.Self;
using log4net;
using log4net.Config;

namespace DowJones.Dash.DataSourcesServer
{
    class Program
    {
        const string Url = "http://localhost:9091/";
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
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

            while(true)
            {
                var ki = Console.ReadKey(true);

                if (ki.Key == ConsoleKey.X)
                {
                    break;
                }
            }
        }
    }
}
