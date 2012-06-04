using System.Web;
using DowJones.Documentation.Website.App_Start;
using DowJones.Documentation.Website.ViewEngines;

[assembly: PreApplicationStartMethod(typeof(ViewEngineConfig), "InitializeBuildProviders")]

namespace DowJones.Documentation.Website.App_Start
{
    public class ViewEngineConfig
    {
        public static void RegisterViewEngines(string docsDirectory)
        {
            System.Web.Mvc.ViewEngines.Engines.Insert(0, new VSDocViewEngine(docsDirectory));
            System.Web.Mvc.ViewEngines.Engines.Insert(0, new MarkdownViewEngine(docsDirectory));
        }

        public static void InitializeBuildProviders()
        {
            MarkdownRazor.MarkdownRazorBuildProvider.RegisterAsBuildProvider();
            XmlViewEngine.XmlBuildProvider.RegisterAsBuildProvider();
        }
    }
}