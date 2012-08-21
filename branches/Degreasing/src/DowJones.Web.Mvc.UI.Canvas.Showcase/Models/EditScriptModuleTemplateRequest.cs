using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        public EditScriptModuleTemplateRequest()
        {
            Options = new List<ScriptModuleTemplateOption>();
        }

        public EditScriptModuleTemplateRequest(ScriptModuleTemplate template)
        {
            Options = new List<ScriptModuleTemplateOption>();

            if (template == null)
                return;

            Id = template.Id;
            Description = template.Description;
            Title = template.Title;
            Script = template.Scripts.FirstOrDefault();
            Html = template.Html;

            if (template.Options != null)
                Options = new List<ScriptModuleTemplateOption>(template.Options);
        }
    }
}