using System.Reflection;
using DowJones.Prod.X.Components.EmbededResources.JavaScript;
using DowJones.Web;
using DowJones.Web.Mvc.UI;

namespace DowJones.Prod.X.Components.Extentions
{
    public static class ScriptRegistryBuilderExtentions
    {
        public static ScriptRegistryBuilder WithBootstrap(this ScriptRegistryBuilder builder, bool enabled = true)
        {
            builder.IncludeResource(Assembly.GetAssembly(typeof(Resources.JavaScript)), Resources.JavaScript.BootstrapJs, ClientResourceDependencyLevel.MidLevel);
            return builder;
        }

        public static ScriptRegistryBuilder WithBootstrapModal(this ScriptRegistryBuilder builder, bool enabled = true)
        {
            builder.IncludeResource(Assembly.GetAssembly(typeof(Resources.JavaScript)), Resources.JavaScript.BootstrapModal, ClientResourceDependencyLevel.MidLevel);
            return builder;
        }

        public static ScriptRegistryBuilder WithJQueryUI(this ScriptRegistryBuilder builder, bool enabled = true)
        {
            builder.IncludeResource(Assembly.GetAssembly(typeof(Resources.JavaScript)), Resources.JavaScript.JQueryUI, ClientResourceDependencyLevel.MidLevel);
            return builder;
        }

        public static ScriptRegistryBuilder WithSortElements(this ScriptRegistryBuilder builder, bool enabled = true)
        {
            builder.IncludeResource(Assembly.GetAssembly(typeof(Resources.JavaScript)), Resources.JavaScript.SortElements, ClientResourceDependencyLevel.MidLevel);
            return builder;
        }
    }
}


