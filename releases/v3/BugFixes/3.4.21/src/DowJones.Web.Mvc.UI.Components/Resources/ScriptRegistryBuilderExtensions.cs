using System.Reflection;

namespace DowJones.Web.Mvc.UI.Components
{
    public static class ScriptRegistryBuilderExtensions
    {
        private static readonly Assembly CurrentAssembly = typeof (ScriptRegistryBuilderExtensions).Assembly;


        public static ScriptRegistryBuilder WithJQueryThreeDots(this ScriptRegistryBuilder builder)
        {
            return builder.IncludeResource(CurrentAssembly, Resources.EmbeddedResources.Js.JQueryThreeDots);
        }

		public static ScriptRegistryBuilder WithJQueryTooltip(this ScriptRegistryBuilder builder)
		{
			return builder.IncludeResource(CurrentAssembly, Resources.EmbeddedResources.Js.JQueryTooltip);
		}
    }
}
