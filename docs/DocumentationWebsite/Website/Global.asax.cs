using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DowJones.Documentation.Website.App_Start;
using DowJones.Documentation.Website.Models;

namespace DowJones.Documentation.Website
{
	public class MvcApplication : HttpApplication
	{
        private static readonly string BasePagesDirectory = ConfigurationManager.AppSettings["DocumentationDirectory"];
        
        public static DocumentationPages DocumentationPages { get; private set; }

		protected void Application_Start()
		{
            LoadDocumentationPages(BasePagesDirectory);

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngineConfig.RegisterViewEngines(BasePagesDirectory);
		}

	    private void LoadDocumentationPages(string docsDirectory)
        {
            var docsPath = HttpContext.Current.Server.MapPath(docsDirectory);
            var locator = new DocumentationPageLocator(docsPath) { SectionOrder = new [] { "LiveDemo", "Overview" }};
            DocumentationPages = locator.LocateDocumentationPages();
        }
    }
}