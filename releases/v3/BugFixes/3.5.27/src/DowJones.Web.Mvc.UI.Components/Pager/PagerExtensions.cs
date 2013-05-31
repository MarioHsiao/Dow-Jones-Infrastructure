using DowJones.Web;
using DowJones.Web.Mvc.UI.Components.Pager;

[assembly: System.Web.UI.WebResourceAttribute(PagerExtensions.PagerScriptResourceName, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.UI.Components.Pager
{
    [ScriptResource(Name = "Pager",  ResourceName = PagerScriptResourceName, DependencyLevel = ClientResourceDependencyLevel.MidLevel)]

    public static class PagerExtensions
    {
        internal const string PagerScriptResourceName = "DowJones.Web.Mvc.UI.Components.Pager.Pager.js";

        public static ViewComponentFactory Pager(this ViewComponentFactory factory)
        {
            factory.ScriptRegistry().IncludeResource(
                    typeof(PagerExtensions).Assembly,
                    PagerScriptResourceName,
                    ClientResourceDependencyLevel.MidLevel
                );

            return factory;
        }
    }
}