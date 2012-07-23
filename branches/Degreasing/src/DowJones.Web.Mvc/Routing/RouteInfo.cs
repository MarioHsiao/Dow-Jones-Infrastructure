using System;
using System.Reflection;
using System.Web.Routing;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.Extensions;
using DowJones.Web.Mvc.Infrastructure;

namespace DowJones.Web.Mvc.Routing
{
    public class RouteInfo
    {
        public const string DefaultControllerExtension = ".aspx";

        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Url { get; set; }

        public RouteValueDictionary Defaults
        {
            get
            {
                var defaultValues = new { controller = Controller, action = Action };
                return new RouteValueDictionary(defaultValues);
            }
        }

        public RouteInfo()
        {
        }

        public RouteInfo(Type controller, MethodInfo action)
        {
            Controller = controller.Name.Substring(0, controller.Name.Length - "Controller".Length);
            Action = action.Name;
        }


        public string ResolveRoute(string explicitUrl, RouteCollection routes, RequestContext requestContext, IISVersion iisVersion)
        {
            Guard.IsNotNull(routes, "routes");
            Guard.IsNotNull(requestContext, "requestContext");
            Guard.IsNotNull(iisVersion, "iisVersion");

            // An explict URL trumps everything
            string routeUrl = explicitUrl;

            // If one doesn't exist, try to figure it out
            if (string.IsNullOrEmpty(routeUrl))
                routeUrl = routes.GetVirtualPath(requestContext, Defaults).VirtualPath;

            if ((routeUrl ?? string.Empty).StartsWith("/"))
                routeUrl = routeUrl.Substring(1);

            routeUrl = ApplyRoutingExtension(routeUrl, iisVersion);

            return routeUrl;
        }

        public static string ApplyRoutingExtension(string route, IISVersion iisVersion)
        {
            Guard.IsNotNull(iisVersion, "iisVersion");

            string routeUrl = route;

            if (string.IsNullOrWhiteSpace(routeUrl))
            {
                return routeUrl;
            }

            if (!iisVersion.SupportsRouting)
            {
                int firstSlash = routeUrl.IndexOf('/', 1);
                if (firstSlash <= 0)
                    routeUrl = routeUrl + DefaultControllerExtension;
                else
                    routeUrl = routeUrl.Insert(firstSlash, DefaultControllerExtension);
            }

            return routeUrl;
        }

    }
}