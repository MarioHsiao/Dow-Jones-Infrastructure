using System.Web.Mvc;
using System.Web.Routing;
using DowJones.Infrastructure;

namespace DowJones.Web.ComponentRenderingService
{
	public class Routes : IBootstrapperTask
	{
		public void Execute()
		{
			RouteTable.Routes.IgnoreRoute("common.js");
			RouteTable.Routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = string.Empty }
			);
		}
	}
}