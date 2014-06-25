using System.Web.Mvc;
using System.Web.Routing;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.Routing;

namespace DowJones.Web.Showcase.BootstrapperTasks
{
    public class RouteRegistrationTask : IBootstrapperTask
    {
        private readonly RouteCollection _routes;

        public RouteRegistrationTask(RouteCollection routes)
        {
            _routes = routes;
        }

        public void Execute()
        {
            /*** IMPORTANT:  Avoid adding routes here - prefer RouteAttribute instead!  ****/
            _routes.IgnoreRoute("common.js");
            //_routes.IgnoreRoute("content/{*pathInfo}");
			_routes.MapRoute("article",
				"article/{accessionNumber}",
				new { controller = "Article", action = "Article" },
				new { accessionNumber = new NotEqual("ComponentExplorerDemo") });

            _routes.MapRoute(
                "StaDemo", // Route name
                "StaDemo/{action}/{id}", // URL with parameters
                new { controller = "StaDemo", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );
            _routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }
    }
}