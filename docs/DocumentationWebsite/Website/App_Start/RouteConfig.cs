using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DowJones.Documentation.DataAccess;

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
                new { controller = "Documentation", action = "Page", page = UrlParameter.Optional, section = UrlParameter.Optional },
                new DocumentationCategoryRouteConstraint(MvcApplication.ContentRepository)
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Documentation", action = "Homepage", id = UrlParameter.Optional }
            );
        }


        class DocumentationCategoryRouteConstraint : IRouteConstraint
        {
            private readonly IContentRepository _repository;

            public DocumentationCategoryRouteConstraint(IContentRepository repository)
            {
                _repository = repository;
            }

            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                return values.ContainsKey("category") 
                    && _repository.GetCategory(values["category"] as string) != null;
            }
        }
    }
}