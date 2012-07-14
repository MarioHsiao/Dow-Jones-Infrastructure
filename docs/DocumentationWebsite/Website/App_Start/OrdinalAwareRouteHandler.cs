using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DowJones.Documentation.DataAccess;

namespace DowJones.Documentation.Website.App_Start
{
	public class OrdinalAwareRouteHandler : MvcRouteHandler
	{
		readonly IContentRepository _repository;

		public OrdinalAwareRouteHandler(IContentRepository repository)
		{
			_repository = repository;
		}

		/// <summary>
		/// Provides the object that processes the request.
		/// </summary>
		/// <returns>
		/// An object that processes the request.
		/// </returns>
		/// <param name="requestContext">An object that encapsulates information about the request.</param>
		protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			requestContext.HttpContext.SetSessionStateBehavior(GetSessionStateBehavior(requestContext));
			return new OrdinalUrlHandler(_repository, requestContext);
		}
	}

	// Deriving from MvcHandler doesn't seem to call MapOrdinalBasedRoute 
	// in the overriden ProcessRequest for some weird reason, hence implmenting IHttpHandler
	public class OrdinalUrlHandler : IHttpHandler
	{
		protected RequestContext RequestContext { get; private set; }
		private readonly IContentRepository _repository;

		public OrdinalUrlHandler(IContentRepository repository, RequestContext requestContext)
		{
			_repository = repository;
			RequestContext = requestContext;
		}


		void IHttpHandler.ProcessRequest(HttpContext context)
		{
			MapOrdinalBasedRoute(RequestContext.RouteData);
			var controllerId = (string)RequestContext.RouteData.Values["controller"];
			IController controller = null;
			IControllerFactory factory = null;
			try
			{
				factory = ControllerBuilder.Current.GetControllerFactory();
				controller = factory.CreateController(RequestContext, controllerId);
				if (controller != null)
				{
					controller.Execute(RequestContext);
				}
			}
			finally
			{
				if (factory != null) factory.ReleaseController(controller);
			}
		}

		/// <summary>
		/// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
		/// </summary>
		/// <returns>
		/// true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
		/// </returns>
		bool IHttpHandler.IsReusable
		{
			get { return false; }
		}

		/// <summary>
		/// Maps a friendly route to Ordinal based route.
		/// E.g. ~/gettingstarted -> ~/1gettingstarted
		/// </summary>
		/// <param name="routeData">Incoming RouteData</param>
		public void MapOrdinalBasedRoute(RouteData routeData)
		{
			var category = routeData.Values["category"] as string;
			if (category == null) return;	// return if category is null

			var mappedCategory = _repository.GetCategory(category, true);
			if (mappedCategory == null) return;		// return if category is not found

			// map route value to its key
			routeData.Values["category"] = mappedCategory.Name.Key;

			var page = routeData.Values["page"] as string;
			if (page == null) return;		// return if page is null

			var mappedPage = mappedCategory.Find(page);

			if (mappedPage != null)		// if page found, map route value to its key
				routeData.Values["page"] = mappedPage.Name.Key;
		}
	}

}