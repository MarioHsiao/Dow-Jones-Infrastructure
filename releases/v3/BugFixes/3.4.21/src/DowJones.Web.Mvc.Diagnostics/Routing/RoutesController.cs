using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DowJones.Web.Mvc.Diagnostics.Fakes;

namespace DowJones.Web.Mvc.Diagnostics.Routing
{
    public class RoutesController : Controller
    {
        public ActionResult Index(string url, string httpMethod)
        {
            var relativeUrl = MakeAppRelative(url ?? string.Empty);
            var httpContext = new FakeHttpContext(relativeUrl, httpMethod ?? "GET");

            var routes = from route in RouteTable.Routes.OfType<Route>()
                                  let routeData = route.GetRouteData(httpContext)
                                  select new RouteViewModel(route, routeData);

            var viewModel = new RouteDebuggerViewModel {
                                Url = url,
                                HttpMethod = httpMethod,
                                Routes = routes
                            };

            return new DiagnosticsViewAction<RouteDebuggerView>(viewModel);
        }

        private static string MakeAppRelative(string url)
        {
            if (!url.StartsWith("~"))
            {
                if (!url.StartsWith("/"))
                    url = "~/" + url;
                else
                    url = "~" + url;
            }
            return url;
        }
    }
}


