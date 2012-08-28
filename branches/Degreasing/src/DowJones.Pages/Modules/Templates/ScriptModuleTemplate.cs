using System.Collections.Generic;
using System.Linq;

namespace DowJones.Pages.Modules.Templates
{
    public class ScriptModuleTemplate
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> ExternalIncludes { get; set; }
        public string HtmlLayout { get; set; }
        public IEnumerable<ScriptModuleTemplateOption> Options { get; set; }
        public string Script { get; set; }
        public string Styles { get; set; }
        public string Title { get; set; }


        public ScriptModuleTemplate()
        {
            Options = Enumerable.Empty<ScriptModuleTemplateOption>();
            ExternalIncludes = Enumerable.Empty<string>();
            Script = string.Empty;
        }
    }
}