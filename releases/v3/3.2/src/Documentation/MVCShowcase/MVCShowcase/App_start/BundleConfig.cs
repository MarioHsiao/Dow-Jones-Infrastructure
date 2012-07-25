using System.Web.Optimization;
using DowJones.MvcShowcase.App_start;

[assembly: WebActivator.PostApplicationStartMethod(typeof(BundleConfig), "RegisterBundles") ]
namespace DowJones.MvcShowcase.App_start
{
	public class BundleConfig
	{
		 public static void RegisterBundles()
		 {
			 var bundles = BundleTable.Bundles;

			 bundles.Add(new StyleBundle("~/styles/core")
					.Include("~/styles/normalize.css",
							 "~/styles/normalizeExtensions.css",
							 "~/styles/site.css"));
		 }
	}
}