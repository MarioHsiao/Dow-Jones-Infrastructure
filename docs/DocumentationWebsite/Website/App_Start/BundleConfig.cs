using System.Web.Optimization;

namespace DowJones.Documentation.Website.App_Start
{
	public class BundleConfig
	{
		public void RegisterBundles(BundleCollection bundles)
		{

			bundles.Add(new ScriptBundle("~/bundles/head").Include(
						"~/Scripts/modernizr-2.5.3.js"
						));

			bundles.Add(new ScriptBundle("~/bundles/explorer").Include(
						"~/Scripts/jquery.asyncIFrame.js",
						"~/Scripts/explorer.js",
						"~/Scripts/collapsible-section.js",
						"~/Scripts/jquery.simplemodal.1.4.2.min.js",
						"~/Scripts/jquery.blockUI.js"));

			bundles.Add(new ScriptBundle("~/bundles/core").Include(
						"~/Scripts/jquery-1.7.2.js",
						"~/Scripts/bootstrap.js"));

			#region Syntax Highlighter

			bundles.Add(new ScriptBundle("~/bundles/syntaxhighlighter-js").Include(
							"~/Scripts/prettify/prettify.js"));

			bundles.Add(new StyleBundle("~/bundles/syntaxhighlighter-css").Include(
						"~/Styles/prettify/spacelab.css")); 

			#endregion

			#region Themes

			bundles.Add(new StyleBundle("~/Styles/theme-default").Include(
				"~/Styles/themes/default/bootstrap.css",
				"~/Styles/bootstrap-responsive.css",
				"~/Styles/Site.css"));

			bundles.Add(
				new StyleBundle("~/Styles").Include(
					"~/Styles/bootstrap-responsive.css",
					"~/Styles/Site.css")
			);

			bundles.Add(new StyleBundle("~/Styles/theme-spacelab").Include(
				"~/Styles/themes/spacelab/bootstrap.css",
				"~/Styles/themes/spacelab/site.css"));
			
			#endregion
		}
	}
}