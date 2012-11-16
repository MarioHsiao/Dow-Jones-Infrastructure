using System.Web.Mvc;
using System.Web.Routing;
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
				new Route("article/{year}/{month}/{day}/{name}/{an}",
					new RouteValueDictionary(new { controller = "Article", action = "Index", an = "" }),
					new DashRouteHandler()));

			_routes.Add(
				new Route("pages/{name}",
					new RouteValueDictionary( new { controller = "Pages", action = "Index" }),
					new DashRouteHandler()));

			_routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

		}
	}
}