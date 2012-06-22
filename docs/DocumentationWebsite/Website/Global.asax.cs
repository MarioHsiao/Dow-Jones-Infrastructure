using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DowJones.Documentation.DataAccess;
using DowJones.Documentation.Website.App_Start;

namespace DowJones.Documentation.Website
{
	public class MvcApplication : HttpApplication
	{
        private static readonly string BasePagesDirectory = ConfigurationManager.AppSettings["DocumentationDirectory"];
        
        public static IContentRepository ContentRepository { get; private set; }

		protected void Application_Start()
		{
            InitializeContentRepository(BasePagesDirectory);

			AreaRegistration.RegisterAllAreas();
			new FilterConfig().RegisterGlobalFilters(GlobalFilters.Filters);
			new RouteConfig(ContentRepository).RegisterRoutes(RouteTable.Routes);
			new BundleConfig().RegisterBundles(BundleTable.Bundles);
            new ViewEngineConfig().RegisterViewEngines(BasePagesDirectory);
		}

	    private void InitializeContentRepository(string docsDirectory)
        {
            var docsPath = HttpContext.Current.Server.MapPath(docsDirectory);
	        var sectionOrder = new[]
	            {
	                "Overview", "LiveDemo", "Configuration",
	                "Constructors", "Events", "Properties",
	                "Methods"
	            };
            ContentRepository = new FileBasedContentRepository(docsPath, sectionOrder);
        }
    }
}