using System.Collections.Generic;

namespace DowJones.Pages.Modules
{
    public interface IModuleTemplateManager
    {
        ModuleTemplate GetTemplate(int templateId);
        IEnumerable<ModuleTemplate> GetTemplates();
        int CreateTemplate(ModuleTemplate template);
        void UpdateTemplate(ModuleTemplate template);
        void DeleteTemplate(int templateId);
    }
}
