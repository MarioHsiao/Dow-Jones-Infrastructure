using System.Collections.Generic;
using System.Linq;
using DowJones.Pages.Modules.Templates;

namespace DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule.Editor
{
    public class ScriptModuleEditor : CompositeComponentModel
    {
        [ClientProperty("moduleId")]
        public int? ModuleId { get; set; }

        [ClientProperty("options")]
        public IEnumerable<ScriptModuleTemplateOption> Options { get; set; }

        [ClientProperty("templateId")]
        public string TemplateId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [ClientProperty("pageId")]
        public string PageId { get; set; }

        [ClientProperty("columnSpan")]
        public int ColumnSpan { get; set; }

        public int MaxColumnSpan { get; set; }
        public int MinColumnSpan { get; set; }


        public ScriptModuleEditor()
        {
            DataServiceUrl = CanvasSettings.Default.GetDataServiceUrl(typeof(ScriptModule), CanvasSettings.Default);
            Options = Enumerable.Empty<ScriptModuleTemplateOption>();
            ColumnSpan = 2;
            MinColumnSpan = 1;
            MaxColumnSpan = 4;
        }
    }
}