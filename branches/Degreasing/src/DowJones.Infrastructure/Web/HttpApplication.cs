using System;
using System.IO;
using System.Web;
using DowJones.Extensions;
using log4net;
using log4net.Config;
using DowJones.DependencyInjection;
using DowJones.Infrastructure;
using DowJones.Properties;
using Ninject.Activation.Caching;

namespace DowJones.Web
{
    public abstract class HttpApplication : System.Web.HttpApplication
    {
        protected static ILog Log = LogManager.GetLogger(typeof(HttpApplication));

        public static DateTime AssemblyTimestamp { get; private set; }

        protected HttpApplication()
        {
            EndRequest += HttpApplication_EndRequest;
        }

        public void Application_Start()
        {
            lock (this)
            {
                XmlConfigurator.Configure(new FileInfo(HttpContext.Current.Server.MapPath(Settings.Default.Log4NetConfigurationFile)));

                AssemblyTimestamp = GetType().Assembly.GetAssemblyTimestamp();
                ClientResourceHandler.LastModifiedCalculator = context => AssemblyTimestamp;

                ServiceLocator.Initialize();
                ServiceLocator.Current.Inject(this);

                OnApplicationStarted();
            }
        }

        public void Application_End()
        {
            lock (this)
            {
                if (ServiceLocator.Current != null)
                    ServiceLocator.Current.Dispose();

                OnApplicationStopped();
            }
        }

        protected virtual void Application_Error()
        {
            Exception error = Server.GetLastError();
            Log.Error("Unhandled error", error);
        }

        protected virtual void OnApplicationStarted()
        {
            ServiceLocator.Resolve<Bootstrapper>().Execute();
        }

        private static void HttpApplication_EndRequest(object sender, EventArgs e)
        {
            if (!(ServiceLocator.Current is NinjectServiceLocator))
                throw new ApplicationException("Service locator instance is not a Ninject Service Locator");

            var kernel = ((NinjectServiceLocator)ServiceLocator.Current).Kernel;
            kernel.Components.Get<ICache>().Clear(HttpContext.Current);
        }

        protected virtual void OnApplicationStopped()
        {
            Log.Info("Application stopped.");
        }
    }
}
