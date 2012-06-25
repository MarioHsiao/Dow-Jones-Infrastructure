using System.Web.Mvc;
using System.Web.Routing;
using DowJones.Infrastructure;

namespace DowJones.MvcShowcase.BootstrapperTasks
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
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
            );
        }
    }
}