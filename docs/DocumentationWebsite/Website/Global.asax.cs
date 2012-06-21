using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DowJones.Documentation.DataAccess;
using DowJones.Documentation.Website.App_Start;
using DowJones.Documentation.Website.Models;

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
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngineConfig.RegisterViewEngines(BasePagesDirectory);
		}

	    private void InitializeContentRepository(string docsDirectory)
        {
            var docsPath = HttpContext.Current.Server.MapPath(docsDirectory);
            ContentRepository = new FileBasedContentRepository(docsPath)
                {
                    SectionOrder = new[]
                        {
                            "LiveDemo", "Overview", "Configuration", 
                            "Constructors", "Events", "Properties", 
                            "Methods"
                        }
                };
        }
    }
}