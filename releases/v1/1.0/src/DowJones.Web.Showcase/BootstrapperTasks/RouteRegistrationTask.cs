using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DowJones.Infrastructure;

namespace DowJones.Web.Showcase.BootstrapperTasks
{
    public class RouteRegistrationTask : IBootstrapperTask
    {
        public void Execute()
        {
            RegisterRoutes(RouteTable.Routes, new HttpContextWrapper(HttpContext.Current));
        }

        internal void RegisterRoutes(RouteCollection routes, HttpContextBase httpContext)
        {
            if (httpContext.GetIISVersion().SupportsRouting)
                MapExtensionlessRoutes(routes);
            else
                MapRoutesWithExtensions(routes);
        }

        
        private static void MapRoutesWithExtensions(RouteCollection routes)
        {
            routes.MapRoute(
                "Default with Extension", // Route name
                "{controller}.aspx/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        private static void MapExtensionlessRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

    }
}