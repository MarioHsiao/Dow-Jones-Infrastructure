using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DowJones.DependencyInjection;
using DowJones.Web.Mvc.Mobile;
using DowJones.Web.Mvc.Routing;
using DowJonesHttpApplication = DowJones.Web.HttpApplication;

namespace DowJones.Web.Mvc
{
    public class HttpApplication : DowJonesHttpApplication
    {
        private static bool customRoutesInitialized;

        [Inject("No access to constructor injection")]
        public IDependencyResolver CustomDependencyResolver { get; set; }

        protected void Application_BeginRequest()
        {
            // This needs to happen on the first request as opposed
            // to Application Start because we need an actual Request
            // object to build our Routes from
            RegisterCustomRouteAttributes();
        }

        protected override void OnApplicationStarted()
        {
            RegisterDefaultRoutingRules(RouteTable.Routes);

            var dataAnnotationsValidator = ModelValidatorProviders.Providers.OfType<DataAnnotationsModelValidatorProvider>().SingleOrDefault();
            var filterProvider = FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().SingleOrDefault();

            if (dataAnnotationsValidator != null)
            {
                ModelValidatorProviders.Providers.Remove(dataAnnotationsValidator);
            }

            if (filterProvider != null)
            {
                FilterProviders.Providers.Remove(filterProvider);
            }

            DependencyResolver.SetResolver(CustomDependencyResolver);

            AreaRegistration.RegisterAllAreas();

            RegisterMobileInformationFilter();

            base.OnApplicationStarted();
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

        private static void RegisterMobileInformationFilter()
        {
            GlobalFilters.Filters.Add(new MobileDeviceInformationFilter());
        }

        private void RegisterCustomRouteAttributes()
        {
            if (customRoutesInitialized) return;
            lock (RouteTable.Routes)
            {
                if (customRoutesInitialized) return;
                var routeGenerator = ServiceLocator.Resolve<IRouteGenerator>();
                var customRoutes = routeGenerator.Generate();

                foreach (var route in customRoutes)
                {
                    RouteTable.Routes.Insert(0, route);
                }

                customRoutesInitialized = true;
            }
        }
    }
}
