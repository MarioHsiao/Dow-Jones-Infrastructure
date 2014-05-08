using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace DowJones.Web.Mvc.Routing
{
	public class NotEqual : IRouteConstraint
	{
		public NotEqual(string pattern)
		{
			Pattern = pattern;
		}

		public string Pattern { get; set; }

		public bool Match(HttpContextBase httpContext, Route route, string parameterName, 
			RouteValueDictionary values, RouteDirection routeDirection)
		{
			return String.Compare(values[parameterName].ToString(), Pattern, true) != 0;
		}
	}
}
