using System.Collections.Generic;
using System.Linq;
using DowJones.Pages.Modules.Templates;

namespace DowJones.DegreasedDashboards.Website.Models
{
    public class ScriptModuleTemplateViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public IEnumerable<string> Scripts { get; set; }
        public IEnumerable<ScriptModuleTemplateOption> Options { get; set; }


        public int ScriptCount
        {
            get { return Scripts.Count(); }
        }

        public int OptionCount
        {
            get { return Options.Count(); }
        }


        public ScriptModuleTemplateViewModel()
        {
            Scripts = Enumerable.Empty<string>();
            Options = Enumerable.Empty<ScriptModuleTemplateOption>();
        }

        public ScriptModuleTemplateViewModel(ScriptModuleTemplate template)
        {
            if (template == null)
                return;

            Id = template.Id;
            Title = template.Title;
            Description = template.Description;
            Scripts = template.Scripts;
            Options = template.Options;
        }
    }
}