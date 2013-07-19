using System.Reflection;
using DowJones.Web;

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.ScrollBar.ScrollBar.js", KnownMimeTypes.JavaScript)]
namespace DowJones.Web.Mvc.UI.Components.ScrollBar
{
    [ScriptResourceAttribute("ScrollBar", ResourceName = "DowJones.Web.Mvc.UI.Components.ScrollBar.ScrollBar.js", ResourceKind = ClientResourceKind.Script, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]
    public static class ScrollBarExtensions
    {
        internal const string ScrollBarScriptResourceName = "DowJones.Web.Mvc.UI.Components.ScrollBar.ScrollBar.js";
        private static readonly Assembly TargetAssembly = typeof(ScrollBarExtensions).Assembly;


        public static ViewComponentFactory ScrollBar(this ViewComponentFactory factory)
        {
            factory.ScriptRegistry().IncludeResource(
                TargetAssembly,
                ScrollBarScriptResourceName,
                ClientResourceDependencyLevel.MidLevel);

            return factory;
        }
    }
}
