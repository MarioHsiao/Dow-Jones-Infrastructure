using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Mvc;
using DowJones.Factiva.Currents.Website.Controllers;
using DowJones.Infrastructure;

namespace DowJones.Factiva.Currents.Website.App_Start
{
	public class RouteConfigTask : IBootstrapperTask
	{
		private readonly RouteCollection _routes;

		public RouteConfigTask(RouteCollection routes)
		{
			_routes = routes;
		}
		public void Execute()
		{
			_routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			_routes.Add(
				new Route("pages/{name}",
					new RouteValueDictionary( new { controller = "Pages", action = "Index", name = "" }),
					new DashRouteHandler()));

			_routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

		}
	}
}