using System.IO;

namespace DowJones.Web.Mvc.UI
{
    public interface IScriptRegistryWriter
    {
        void Render(IScriptRegistry scriptRegistry, TextWriter writer);
        void RenderClientScripts(IScriptRegistry scriptRegistry, TextWriter writer, bool renderScriptBlock);
        void RenderHead(IScriptRegistry scriptRegistry, TextWriter writer);
    }

    public static class ScriptRegistryWriterExtensions
    {
        public static string Render(this IScriptRegistryWriter writer, IScriptRegistry scriptRegistry)
        {
            return new HtmlTextWriterRenderer().Render(output => writer.Render(scriptRegistry, output));
        }

        public static string RenderClientScripts(this IScriptRegistryWriter writer, IScriptRegistry scriptRegistry, bool renderScriptBlock = true)
        {
            return new HtmlTextWriterRenderer().Render(output => writer.RenderClientScripts(scriptRegistry, output, renderScriptBlock));
        }

        public static string RenderHead(this IScriptRegistryWriter writer, IScriptRegistry scriptRegistry)
        {
            return new HtmlTextWriterRenderer().Render(output => writer.RenderHead(scriptRegistry, output));
        }
    }
}