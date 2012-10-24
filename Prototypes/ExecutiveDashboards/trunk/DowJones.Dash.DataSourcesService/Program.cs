using System.ServiceProcess;

namespace DowJones.Dash.DataSourcesService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[] 
                { 
                    new DataSourcesHub() 
                };
            ServiceBase.Run(servicesToRun);
        }
    }
}
