using System;
using System.Threading.Tasks;
using log4net;

namespace DowJones.Prod.X.Web
{
    public partial class MvcApplication
    {
        private readonly static ILog Logger = LogManager.GetLogger(typeof(MvcApplication));

        public static Lazy<string> Version = new Lazy<string>(typeof(MvcApplication).Assembly.GetName().Version.ToString);

        public override void Init()
        {
            ViewEnginesRegistration.CleanupOtherViewEngines();
        }

        protected new void Application_Start()
        {
            TaskScheduler.UnobservedTaskException += (sender, excArgs) =>
            {
                if (Logger.IsErrorEnabled)
                {
                    Logger.Error(excArgs.Exception);
                }
            };
        }
    }
}