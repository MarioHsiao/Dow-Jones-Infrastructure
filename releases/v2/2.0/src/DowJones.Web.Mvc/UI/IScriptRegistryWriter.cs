using System.Web.UI;

namespace DowJones.Web.Mvc.UI
{
    public interface IScriptRegistryWriter
    {
        string Render(ScriptRegistry scriptRegistry);
        void Render(ScriptRegistry scriptRegistry, HtmlTextWriter writer);
        string RenderHead(ScriptRegistry scriptRegistry);
    }
}