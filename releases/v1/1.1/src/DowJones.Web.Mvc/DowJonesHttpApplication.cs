using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Routing;
using DowJones.DependencyInjection;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.Mobile;
using DowJones.Web.Mvc.Routing;
using log4net;
using log4net.Config;
using Ninject;
using Ninject.Web.Mvc;
using Inject = DowJones.DependencyInjection.InjectAttribute;

namespace DowJones.Web.Mvc
{
    public class DowJonesHttpApplication : NinjectHttpApplication
    {
        private static bool _customRoutesInitialized;
        private static readonly object StaRoutesLock = new object();

        // Not injected because it's static
        protected static ILog Log = LogManager.GetLogger(typeof (DowJonesHttpApplication));

        [Inject("Optional")]
        protected IEnumerable<IBootstrapperTask> BootstrapperTasks { get; set; }

        protected override IKernel CreateKernel()
        {
            var settings = new DowJonesNinjectSettings();

            IKernel kernel = new StandardKernel(settings);

            kernel.Bind<IAssemblyRegistry>()
                .ToConstant(CreateCustomAssemblyRegistry(settings.CustomAssemblyNames))
                .InSingletonScope();

            var filePatterns = settings.AutoLoadedAssemblyFilePatterns.ToList();

            kernel.Load(filePatterns);

            kernel.Bind<System.Web.Caching.Cache>()
                .ToMethod(x => Kernel.Get<HttpContextBase>().Cache);

            kernel.Bind<HttpRequestBase>()
                .ToMethod(x => Kernel.Get<HttpContextBase>().Request);

            // Set the current Dow Jones ServiceLocator
            ServiceLocator.Current = new NinjectServiceLocator(kernel);

            return kernel;
        }


        // TODO: Move this to its own class
        private static CustomAssemblyRegistry CreateCustomAssemblyRegistry(IEnumerable<string> assemblyNames)
        {
            IEnumerable<Assembly> customAssemblies =
                from assemblyName in assemblyNames
                from assembly in BuildManager.GetReferencedAssemblies().Cast<Assembly>()
                where  assembly.FullName == assemblyName
                    || assembly.FullName.StartsWith(assemblyName)
                select assembly;

            return new CustomAssemblyRegistry(customAssemblies);
        }


        protected override void OnApplicationStarted()
        {
            XmlConfigurator.Configure(new FileInfo(HttpContext.Current.Server.MapPath("web.config")));

            RegisterDefaultRoutingRules(RouteTable.Routes);

            AreaRegistration.RegisterAllAreas();

            ExecuteBootstrapperTasks();

            RegisterMobileInformationFilter();
        }

        private static void RegisterDefaultRoutingRules(RouteCollection routes)
        {
            // Ignore calls to services and handlers - let them fall through to ASP.NET
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.asmx/{*pathInfo}");
            routes.IgnoreRoute("{resource}.svc/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");

            // Ignore WebResource.axd requests no matter where they appear
            routes.IgnoreRoute("{*resource}", new { resource = @".*WebResource.axd.*" });

        }

        private void ExecuteBootstrapperTasks()
        {
            var bootstrapperTasks = BootstrapperTasks ?? Enumerable.Empty<IBootstrapperTask>();
            
            foreach (var bootstrapperTask in bootstrapperTasks)
            {
                bootstrapperTask.Execute();
            }
        }

        private static void RegisterMobileInformationFilter()
        {
            GlobalFilters.Filters.Add(new MobileDeviceInformationFilter());
        }

        protected void Application_BeginRequest()
        {
            // This needs to happen on the first request as opposed
            // to Application Start because we need an actual Request
            // object to build our Routes from
            RegisterCustomRouteAttributes();
        }

        protected void Application_EndRequest()
        {
        }

        protected virtual void Application_Error()
        {
            Exception error = Server.GetLastError();
            Log.Error("Unhandled error", error);
        }

        private void RegisterCustomRouteAttributes()
        {
            if (!_customRoutesInitialized)
            {
                lock(StaRoutesLock)
                {
                    if (!_customRoutesInitialized)
                    {
                        var routeGenerator = Kernel.Get<IRouteGenerator>();
                        var customRoutes = routeGenerator.Generate();

                        foreach (var route in customRoutes)
                        {
                            RouteTable.Routes.Insert(0, route);
                        }

                        _customRoutesInitialized = true;
                    }
                }
            }
        }
    }
}
