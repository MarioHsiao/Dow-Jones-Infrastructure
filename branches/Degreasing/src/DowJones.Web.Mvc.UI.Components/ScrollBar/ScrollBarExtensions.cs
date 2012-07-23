using System.Reflection;
using System.Web.UI;
using DowJones.Web;

[assembly: WebResource(DowJones.Web.Mvc.UI.Components.ScrollBar.ScrollBarExtensions.ScrollBarScriptResourceName, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.UI.Components.ScrollBar
{
    [ScriptResourceAttribute(null, ResourceName = ScrollBarScriptResourceName, ResourceKind = ClientResourceKind.Script)]
    public static class ScrollBarExtensions
    {
        internal const string ScrollBarScriptResourceName = "DowJones.Web.Mvc.UI.Components.ScrollBar.ScrollBar.js";
        private static readonly Assembly TargetAssembly = typeof (ScrollBarExtensions).Assembly;


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
