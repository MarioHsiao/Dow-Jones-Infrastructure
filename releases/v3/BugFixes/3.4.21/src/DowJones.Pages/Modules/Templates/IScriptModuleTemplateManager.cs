using System.Collections.Generic;

namespace DowJones.Pages.Modules.Templates
{
    public interface IScriptModuleTemplateManager
    {
        string CreateTemplate(ScriptModuleTemplate template);
        void DeleteTemplate(string templateId);
        ScriptModuleTemplate GetTemplate(string templateId);
        IEnumerable<ScriptModuleTemplate> GetTemplates();
        void UpdateTemplate(ScriptModuleTemplate template);
    }
}
