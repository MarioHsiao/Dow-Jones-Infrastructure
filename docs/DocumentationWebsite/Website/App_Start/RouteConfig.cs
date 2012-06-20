using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DowJones.Documentation.Website.Models;

namespace DowJones.Documentation.Website.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "DocumentationBrowser",
                "{category}/{page}/{section}",
                new { controller = "Documentation", action = "Page", 
                      page = UrlParameter.Optional, section = UrlParameter.Optional
                },
                new DocumentationCategoryRouteConstraint(MvcApplication.DocumentationPages)
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Documentation", action = "Homepage", id = UrlParameter.Optional }
            );
        }


        class DocumentationCategoryRouteConstraint : IRouteConstraint
        {
            private readonly DocumentationPages _pages;

            public DocumentationCategoryRouteConstraint(DocumentationPages pages)
            {
                _pages = pages;
            }

            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                return values.ContainsKey("category") && _pages.Category(values["category"] as string) != null;
            }
        }
    }
}