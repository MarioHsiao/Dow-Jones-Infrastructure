using System.Reflection;

namespace DowJones.Web.Mvc.UI.Components.Menu
{
    [ScriptResourceAttribute("Menu", ResourceName = "DowJones.Web.Mvc.UI.Components.Menu.Menu.js", ResourceKind = ClientResourceKind.Script)]
    [ClientTemplateResourceAttribute(ResourceName = "DowJones.Web.Mvc.UI.Components.Menu.ClientTemplates.SimpleMenu.htm", ResourceKind = ClientResourceKind.ClientTemplate, TemplateId = "simpleMenu")]
    public static class MenuExtensions
    {
        internal const string MenuScriptResourceName = "DowJones.Web.Mvc.UI.Components.Menu.Menu.js";
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
