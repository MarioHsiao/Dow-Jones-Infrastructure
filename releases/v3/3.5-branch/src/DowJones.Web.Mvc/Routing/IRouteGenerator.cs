using System.Collections.Generic;
using System.Web.Routing;

namespace DowJones.Web.Mvc.Routing
{
    public interface IRouteGenerator
    {
        IEnumerable<RouteBase> Generate();
    }
}