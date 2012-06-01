using System.Web;
using System.Web.Mvc;
using DowJones.Documentation.Website.App_Start;
using DowJones.Documentation.Website.Views;

[assembly: PreApplicationStartMethod(typeof(ViewEngineConfig), "InitializeBuildProviders")]

namespace DowJones.Documentation.Website.App_Start
{
    public class ViewEngineConfig
    {
        public static void RegisterViewEngines(string docsDirectory)
        {
            ViewEngines.Engines.Insert(0, new DocumentationViewEngine(docsDirectory));
        }

        public static void InitializeBuildProviders()
        {
            MarkdownRazor.MarkdownRazorBuildProvider.Register();
        }
    }
}