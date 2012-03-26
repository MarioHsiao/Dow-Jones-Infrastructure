using System.Reflection;
using DowJones.Web;
using DowJones.Web.Mvc.UI.Components.Pager;

[assembly: System.Web.UI.WebResourceAttribute(PagerExtensions.PagerScriptResourceName, KnownMimeTypes.JavaScript)]
[assembly: System.Web.UI.WebResourceAttribute(PagerExtensions.PagerStylesheetResourceName, KnownMimeTypes.Stylesheet, PerformSubstitution = true)]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.Pager.images.nav_dots.png", KnownMimeTypes.PngImage)]

namespace DowJones.Web.Mvc.UI.Components.Pager
{

    [ScriptResource(ResourceName = PagerScriptResourceName, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]

    public static class PagerExtensions
    {
        private static readonly Assembly TargetAssembly = typeof(PagerExtensions).Assembly;
        internal const string PagerScriptResourceName = "DowJones.Web.Mvc.UI.Components.Pager.Pager.js";
        internal const string PagerStylesheetResourceName = "DowJones.Web.Mvc.UI.Components.Pager.Pager.css";

        public static ViewComponentFactory Pager(this ViewComponentFactory factory)
        {
            factory.ScriptRegistry().IncludeResource(
                    TargetAssembly,
                    PagerScriptResourceName,
                    ClientResourceDependencyLevel.MidLevel,
                    performSubstitution: false
                );

            factory.StylesheetRegistry().IncludeResource(
                    TargetAssembly,
                    PagerStylesheetResourceName,
                    ClientResourceDependencyLevel.MidLevel,
                    performSubstitution: false
                );

            return factory;
        }

    }

}