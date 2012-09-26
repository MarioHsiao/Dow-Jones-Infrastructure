using System;
using System.Threading.Tasks;
using log4net;

namespace DowJones.Dash.Website
{
    public partial class MvcApplication
    {
        private readonly static ILog Logger = LogManager.GetLogger(typeof (MvcApplication));
/*
        public static string Version
        {
            // Clean up outstanding polling tasks
            PollingDataSource.GlobalCancellationTokenSource.Cancel(false);
        }
*/
        public static Lazy<string> Version = 
            new Lazy<string>(typeof(MvcApplication).Assembly.GetName().Version.ToString);


        protected void Application_Start()
        {
            TaskScheduler.UnobservedTaskException += (object sender, UnobservedTaskExceptionEventArgs excArgs) =>
            {
                if(Logger.IsErrorEnabled)
                {
                    Logger.Error(excArgs.Exception);
                }
            };
        }
    }
}