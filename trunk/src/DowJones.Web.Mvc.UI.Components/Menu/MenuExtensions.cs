using System.Reflection;
using System.Web.UI;
using DowJones.Web;

[assembly: WebResource(DowJones.Web.Mvc.UI.Components.Menu.MenuExtensions.MenuScriptResourceName, KnownMimeTypes.JavaScript)]
[assembly: WebResource(DowJones.Web.Mvc.UI.Components.Menu.MenuExtensions.MenuTemplateResourceName, KnownMimeTypes.Html)]

namespace DowJones.Web.Mvc.UI.Components.Menu
{
    [ScriptResourceAttribute("Menu", ResourceName = MenuScriptResourceName, ResourceKind = ClientResourceKind.Script)]
    [ClientTemplateResourceAttribute(ResourceName = MenuTemplateResourceName, ResourceKind = ClientResourceKind.ClientTemplate, TemplateId = "simpleMenu")]
    public static class MenuExtensions
    {
        internal const string BaseResourceName = "DowJones.Web.Mvc.UI.Components.Menu.";
        internal const string MenuScriptResourceName = BaseResourceName + "Menu.js";
        internal const string MenuTemplateResourceName = BaseResourceName + "ClientTemplates.SimpleMenu.htm";

        private static readonly Assembly TargetAssembly = typeof(MenuExtensions).Assembly;

        public static ViewComponentFactory Menu(this ViewComponentFactory factory)
        {
            factory.ScriptRegistry().IncludeResource(
                TargetAssembly,
                MenuScriptResourceName,
                ClientResourceDependencyLevel.MidLevel);

            return factory;
        }
    }
}
