using System.Reflection;
using DowJones.Prod.X.Components.EmbededResources.JavaScript;
using DowJones.Web;
using DowJones.Web.Mvc.UI;

namespace DowJones.Prod.X.Components.Extentions
{
    public static class ScriptRegistryBuilderExtentions
    {
        public static ScriptRegistryBuilder WithBootstrap(this ScriptRegistryBuilder builder, ClientResourceDependencyLevel level = ClientResourceDependencyLevel.MidLevel, bool enabled = true)
        {
            builder.IncludeResource(Assembly.GetAssembly(typeof(Resources.JavaScript)), Resources.JavaScript.BootstrapJs, level);
            return builder;
        }

        public static ScriptRegistryBuilder WithBootstrapModal(this ScriptRegistryBuilder builder, ClientResourceDependencyLevel level = ClientResourceDependencyLevel.MidLevel, bool enabled = true)
        {
            builder.IncludeResource(Assembly.GetAssembly(typeof(Resources.JavaScript)), Resources.JavaScript.BootstrapModal, level);
            return builder;
        }

        public static ScriptRegistryBuilder WithJQueryUI(this ScriptRegistryBuilder builder, ClientResourceDependencyLevel level = ClientResourceDependencyLevel.MidLevel, bool enabled = true)
        {
            builder.IncludeResource(Assembly.GetAssembly(typeof(Resources.JavaScript)), Resources.JavaScript.JQueryUI, level);
            return builder;
        }

        public static ScriptRegistryBuilder WithSortElements(this ScriptRegistryBuilder builder, ClientResourceDependencyLevel level = ClientResourceDependencyLevel.MidLevel, bool enabled = true)
        {
            builder.IncludeResource(Assembly.GetAssembly(typeof(Resources.JavaScript)), Resources.JavaScript.SortElements, level);
            return builder;
        }
    }
}


