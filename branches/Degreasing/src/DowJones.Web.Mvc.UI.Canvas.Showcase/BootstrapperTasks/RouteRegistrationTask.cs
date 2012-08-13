using System.Web.Mvc;
using System.Web.Routing;
using DowJones.Infrastructure;

namespace DowJones.DegreasedDashboards.Website.BootstrapperTasks
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
            _routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            _routes.IgnoreRoute("{resource}.asmx/{*pathInfo}");
            _routes.IgnoreRoute("{resource}.svc/{*pathInfo}");
            _routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");

            /*** IMPORTANT:  Avoid adding generic routes here - prefer RouteAttribute instead!  ****/

            _routes.MapRoute(
                "CanvasPage", // Route name
                "pages/{id}", // URL with parameters
                new {controller = "Pages", action = "Page" },
                new { id = "[0-9]*" }
            );

            _routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Pages", action = "Index", id = UrlParameter.Optional} // Parameter defaults
            );
        }
    }
}