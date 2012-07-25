using System.Linq;
using System.Web;
using System.Web.Mvc;
using DowJones.Documentation.Website.App_Start;
using DowJones.Documentation.Website.ViewEngines;

[assembly: PreApplicationStartMethod(typeof(ViewEngineConfig), "InitializeBuildProviders")]

namespace DowJones.Documentation.Website.App_Start
{
    public class ViewEngineConfig
    {
        public void RegisterViewEngines(string docsDirectory)
        {
            var viewEngines = System.Web.Mvc.ViewEngines.Engines;

            var webFormsEngine = viewEngines.OfType<WebFormViewEngine>().FirstOrDefault();

            if (webFormsEngine != null)
                viewEngines.Remove(webFormsEngine);

            viewEngines.Insert(0, new VSDocViewEngine(docsDirectory));
            viewEngines.Insert(0, new MarkdownViewEngine(docsDirectory));
        }

        public static void InitializeBuildProviders()
        {
            MarkdownRazor.MarkdownRazorBuildProvider.RegisterAsBuildProvider();
            XmlViewEngine.XmlBuildProvider.RegisterAsBuildProvider();
        }
    }
}