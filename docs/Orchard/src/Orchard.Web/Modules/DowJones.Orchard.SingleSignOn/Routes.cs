using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace DowJones.Orchard.SingleSignOn
{
    public class Routes : IRouteProvider
    {
        private const string AreaName = "DowJones.Orchard.SingleSignOn";

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            var routes = new List<RouteDescriptor> {
                new RouteDescriptor {
                    Priority = 0,
                    Route = new Route(
                        "SSO/{action}",
                        new RouteValueDictionary {
                                {"area", AreaName},
                                {"controller", "SingleSignOn"},
                                {"action", "Authenticate"}
                            },
                        new RouteValueDictionary(),
                        new RouteValueDictionary { {"area", AreaName} },
                        new MvcRouteHandler())
                }
            };

            // Override the default Account controller routes
            // NOTE: Comment/remove this call to fall back to default Account controller
            routes.Add(                
                new RouteDescriptor {
                    Priority = int.MaxValue,
                    Route = new Route(
                        "AdminAccount/{action}",
                        new RouteValueDictionary {
                            {"area", AreaName},
                            {"controller", "SingleSignOn"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary { {"area", AreaName} },
                        new MvcRouteHandler())
                });

            return routes;
        }
    }
}