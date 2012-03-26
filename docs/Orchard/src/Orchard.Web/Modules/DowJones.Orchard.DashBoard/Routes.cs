using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace DowJones.Orchard.SingleSignOn
{
    public class Routes : IRouteProvider
    {
        private const string AreaName = "DowJones.Orchard.DashBoard";

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }


        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        AreaName,
                        new RouteValueDictionary {
                            {"area", AreaName},
                            {"controller", "DashBoard"},
                            {"action", "Index"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", AreaName}
                        },
                        new MvcRouteHandler())
                }
            };
        }


    }
}