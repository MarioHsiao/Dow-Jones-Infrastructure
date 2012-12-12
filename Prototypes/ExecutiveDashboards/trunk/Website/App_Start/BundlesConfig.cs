using System.Web.Optimization;
using DowJones.Infrastructure;

namespace DowJones.Dash.Website.App_Start
{
    public class BundlesConfigurationTask : IBootstrapperTask
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725

        public void Execute()
        {
            RegisterBundles(BundleTable.Bundles);
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
	        bundles.Add(new StyleBundle("~/css/core")
		                    .Include(
								"~/Content/bootstrap.css",
								"~/Content/bootstrap-responsive.css",
								"~/Content/layout.css",
								"~/Content/layout-responsive.css"));

			bundles.Add(new StyleBundle("~/css/dashboard")
							.Include(
								"~/Content/dashboard.css"
								,"~/Content/statsMap.css"
								,"~/Content/platformStats.css"
								,"~/Content/gallery.css"
								,"~/Content/browserShare.css"
								,"~/Content/concurrentVisits.css"
								,"~/Content/perfGauge.css"
								,"~/Content/topPages.css"
								,"~/Content/pageTimings.css"
						));

            bundles.Add(new ScriptBundle("~/bundles/core").Include(
                "~/Scripts/bootstrap.*",
                "~/Scripts/jquery.signalR-{version}.js"
            ));

        }
    }
}