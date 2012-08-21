using System.Collections.Generic;
using System.Linq;
using DowJones.Mapping;
using DowJones.Pages.Modules.Templates;
using DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule.Editor;

namespace DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule
{
    public class ScriptModule : Module
    {
        [ClientProperty("templateId")]
        public string TemplateId { get; set; }

        public string Html { get; set; }

        [ClientProperty("scriptOptions", Nested = false)]
        public IEnumerable<KeyValuePair<string, object>> ScriptOptions { get; set; }

        [ClientProperty("columnSpan")]
        public int ColumnSpan { get; set; }

        public ScriptModule()
        {
            DataServiceUrl = CanvasSettings.Default.GetDataServiceUrl(GetType(), CanvasSettings.Default);
            ScriptOptions = Enumerable.Empty<KeyValuePair<string, object>>();
            ModuleState = ModuleState.Maximized;
        }


        public class ScriptModuleMapper : TypeMapper<DowJones.Pages.Modules.ScriptModule, IModule>
        {
            private readonly IScriptModuleTemplateManager _scriptModuleTemplateManager;

            public ScriptModuleMapper(IScriptModuleTemplateManager scriptModuleTemplateManager)
            {
                _scriptModuleTemplateManager = scriptModuleTemplateManager;
            }

            public override IModule Map(DowJones.Pages.Modules.ScriptModule source)
            {
                var canEdit = true;
                var template = _scriptModuleTemplateManager.GetTemplate(source.TemplateId);

                return new ScriptModule {
                    CanEdit = canEdit,
                    ColumnSpan = source.ColumnSpan,
                    Editor = CreateEditor(source, template) ?? new ScriptModuleEditor(),
                    ModuleId = source.Id,
                    Title = source.Title,
                    Description = source.Description,
                    Position = source.Position,
                    TemplateId = source.TemplateId,
                    Html = (template == null) ? string.Empty : (template.Html ?? string.Empty),
                    ScriptOptions = source.Options.Select(x => new KeyValuePair<string, object>(x.Name, x.Value)),
                    ModuleState = Mapper.Map<ModuleState>(source.ModuleState),
                };
            }

            private IViewComponentModel CreateEditor(Pages.Modules.ScriptModule source, ScriptModuleTemplate template)
            {
                if(template == null)
                    return null;

                var options =
                    from option in template.Options
                    join optionValue in source.Options on option.Name equals optionValue.Name
                    select new ScriptModuleTemplateOption(option) { DefaultValue = optionValue.Value };

                return new ScriptModuleEditor
                           {
                               ModuleId = source.Id,
                               TemplateId = template.Id,
                               Title = source.Title,
                               Options = options,
                               ColumnSpan = source.ColumnSpan,
                           };
            }
        }
    }
}
