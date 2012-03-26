using System.Web.Mvc;
using System.Web.Routing;
using DowJones.Infrastructure;

namespace DowJones.Web.Showcase
{
    public class RouteRegistryBootstrapper : IBootstrapperTask
    {
        public void Execute()
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*
             *  BEST PRACTICE:
             *  Ideally, globally-registered routing patterns 
             *  should be as generic as possible (e.g. the default 
             *  "{controller}/{action}/{id}" pattern).  When
             *  routing to individual actions, prefer the 
             *  RouteAttribute instead.
            */

            // TODO: Add more custom routes here

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }
    }
}