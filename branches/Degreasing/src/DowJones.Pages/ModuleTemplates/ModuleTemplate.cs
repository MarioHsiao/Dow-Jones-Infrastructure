using System;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Pages.Modules
{
    public class ModuleTemplate
    {
        public int Id { get; set; }

        public string Description { get; set; }
        
        public IEnumerable<ModuleTemplateMetaDatum> MetaData { get; set; }

        public Type ModuleType { get; set; }

        public IEnumerable<ModuleTemplateOption> Options { get; set; }

        public string Title { get; set; }


        public ModuleTemplate()
        {
            MetaData = Enumerable.Empty<ModuleTemplateMetaDatum>();
            Options = Enumerable.Empty<ModuleTemplateOption>();
        }
    }
}