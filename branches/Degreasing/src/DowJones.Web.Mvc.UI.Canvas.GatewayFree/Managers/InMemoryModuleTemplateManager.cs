using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;

namespace DowJones.Pages.Modules
{
    public class InMemoryModuleTemplateManager : IModuleTemplateManager
    {
        private readonly List<ModuleTemplate> _moduleTemplates = new List<ModuleTemplate>();

        public ModuleTemplate GetTemplate(int templateId)
        {
            return _moduleTemplates.FirstOrDefault(x => x.Id == templateId);
        }

        public IEnumerable<ModuleTemplate> GetTemplates()
        {
            return _moduleTemplates;
        }

        public int CreateTemplate(ModuleTemplate template)
        {
            if (_moduleTemplates.Any())
                template.Id = _moduleTemplates.Max(x => x.Id) + 1;
            else
                template.Id = 0;
            
            _moduleTemplates.Add(template);
            
            return template.Id;
        }

        public void UpdateTemplate(ModuleTemplate template)
        {
            _moduleTemplates.Replace(x => x.Id == template.Id, template);
        }

        public void DeleteTemplate(int templateId)
        {
            _moduleTemplates.RemoveAll(x => x.Id == templateId);
        }
    }
}