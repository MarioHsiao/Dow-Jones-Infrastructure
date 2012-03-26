using System.Web.UI;

namespace DowJones.Web.Mvc.UI
{
    public interface IScriptRegistryWriter
    {
        string Render(ScriptRegistry scriptRegistry);
        string RenderHead(ScriptRegistry scriptRegistry);
        void   RenderPartial(ScriptRegistry scriptRegistry, HtmlTextWriter writer);
    }
}