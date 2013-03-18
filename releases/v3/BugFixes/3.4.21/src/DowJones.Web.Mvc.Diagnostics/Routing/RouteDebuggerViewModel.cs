using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace DowJones.Web.Mvc.Diagnostics.Routing
{
    public class RouteDebuggerViewModel
    {
        private static readonly IEnumerable<string> DefaultHttpMethods = Enum.GetNames(typeof(HttpVerbs));

        public string Url { get; set; }

        public string HttpMethod { get; set; }

        public SelectList HttpMethods
        {
            get { return _httpMethods ?? new SelectList(DefaultHttpMethods, HttpMethod); }
            set { _httpMethods = value; }
        }
        private SelectList _httpMethods;

        public IEnumerable<RouteViewModel> Routes { get; set; }

        public RouteDebuggerViewModel()
        {
            Routes = Enumerable.Empty<RouteViewModel>();
        }
    }

    public class RouteViewModel
    {
        public Route Route { get; set; }
        public RouteData RouteData { get; set; }

        public bool IsMatch
        {
            get { return RouteData != null; }
        }

        public string Url
        {
            get { return (Route == null) ? string.Empty : Route.Url; }
        }

        public string Values
        {
            get { return (RouteData == null) ? string.Empty : FormatValues(RouteData.Values); }
        }

        public string Defaults
        {
            get { return (Route == null) ? string.Empty : FormatValues(Route.Defaults); }
        }

        public string Constraints
        {
            get { return (Route == null) ? string.Empty : FormatValues(Route.Constraints); }
        }

        public string DataTokens
        {
            get { return (Route == null) ? string.Empty : FormatValues(Route.DataTokens); }
        }


        public RouteViewModel(Route route, RouteData routeData)
        {
            Route = route;
            RouteData = routeData;
        }

        private static string FormatValues(RouteValueDictionary values)
        {
            return values == null ? "N/A" : String.Join(", ", (from key in values.Keys let value = values[key] ?? "[null]" select key + "=" + value).ToArray());
        }

    }
}