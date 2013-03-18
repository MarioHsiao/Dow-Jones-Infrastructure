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

        public string Script { get; set; }
        public IEnumerable<string> ExternalIncludes { get; set; }
        public IEnumerable<ScriptModuleTemplateOption> Options { get; set; }


        public int ExternalIncludesCount
        {
            get { return ExternalIncludes.Count(); }
        }

        public int OptionCount
        {
            get { return Options.Count(); }
        }


        public ScriptModuleTemplateViewModel()
        {
            Script = string.Empty;
            ExternalIncludes = Enumerable.Empty<string>();
            Options = Enumerable.Empty<ScriptModuleTemplateOption>();
        }

        public ScriptModuleTemplateViewModel(ScriptModuleTemplate template)
        {
            if (template == null)
                return;

            Id = template.Id;
            Title = template.Title;
            Description = template.Description;
            Script = template.Script;
            Options = template.Options;
            ExternalIncludes = template.ExternalIncludes;
        }
    }
}