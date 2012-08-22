using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DowJones.Pages.Modules.Templates;

namespace DowJones.DegreasedDashboards.Website.Models
{
    public class EditScriptModuleTemplateRequest
    {
        public bool IsNew
        {
            get { return string.IsNullOrWhiteSpace(Id); }
        }

        public string Id { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Script { get; set; }

        [DataType(DataType.MultilineText)]
        public string Html { get; set; }

        public List<ScriptModuleTemplateOption> Options { get; set; }

        public List<string> ExternalIncludes { get; set; }

        public EditScriptModuleTemplateRequest()
        {
            Options = new List<ScriptModuleTemplateOption>();
            ExternalIncludes = new List<string>();
        }

        public EditScriptModuleTemplateRequest(ScriptModuleTemplate template)
        {
            Options = new List<ScriptModuleTemplateOption>();
            ExternalIncludes = new List<string>();

            if (template == null)
                return;

            Id = template.Id;
            Description = template.Description;
            Html = template.HtmlLayout;
            Script = template.Script;
            Title = template.Title;

            if (template.ExternalIncludes != null)
                ExternalIncludes = new List<string>(template.ExternalIncludes);

            if (template.Options != null)
                Options = new List<ScriptModuleTemplateOption>(template.Options);
        }
    }
}