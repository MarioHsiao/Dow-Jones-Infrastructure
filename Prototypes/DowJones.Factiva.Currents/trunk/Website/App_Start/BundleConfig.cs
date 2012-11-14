using System.Web.Optimization;
using DowJones.Infrastructure;

namespace DowJones.Factiva.Currents.Website.App_Start
{
	public class BundleConfigTask : IBootstrapperTask
	{
		public void Execute()
		{
			RegisterBundles(BundleTable.Bundles);
		}

		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
	
			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new StyleBundle("~/Content/css")
				.Include("~/Content/bootstrap.css")
				.Include("~/Content/bootstrap-responsive.css")
				.Include("~/Content/site.css")
				.Include("~/Content/canvas.css")
				.Include("~/Content/portalHeadlineList.css"));
		}
	}
}