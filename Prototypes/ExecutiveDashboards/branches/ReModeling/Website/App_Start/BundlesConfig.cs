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
            bundles.Add(new ScriptBundle("~/bundles/core").Include(
                "~/Scripts/bootstrap*",
                "~/Scripts/jquery.signalR-{version}.js"
            ));

			bundles.Add(new StyleBundle("~/bundles/bootstrap").Include(
				"~/Content/bootstrap*"
				));
        }
    }
}