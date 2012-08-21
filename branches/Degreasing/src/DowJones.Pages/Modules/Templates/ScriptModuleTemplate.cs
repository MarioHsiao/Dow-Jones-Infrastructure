using System.Collections.Generic;
using System.Linq;

namespace DowJones.Pages.Modules.Templates
{
    public class ScriptModuleTemplate
    {
        public string Id { get; set; }

        public string Description { get; set; }
        
        public IEnumerable<string> Scripts { get; set; }

        public IEnumerable<ScriptModuleTemplateOption> Options { get; set; }

        public string Title { get; set; }

        public string Html { get; set; }


        public ScriptModuleTemplate()
        {
            Scripts = Enumerable.Empty<string>();
            Options = Enumerable.Empty<ScriptModuleTemplateOption>();
        }
    }
}