using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Mapping;
using DowJones.Pages.Modules.Templates;
using DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule.Editor;

namespace DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule
{
    public class ScriptModule : Module
    {
        public const string ClientResourcePrefix = "ScriptModule-";

        internal static Func<string, string> TemplateUrlThunk = ClientResourceHandler.GenerateUrl;

        [ClientProperty("templateId")]
        public string TemplateId { get; set; }

        public string Html { get; set; }

        [ClientProperty("externalIncludes")]
        public IEnumerable<string> ExternalIncludes { get; set; }

        [ClientProperty("scriptOptions", Nested = false)]
        public IEnumerable<KeyValuePair<string, object>> ScriptOptions { get; set; }

        [ClientProperty("columnSpan")]
        public int ColumnSpan { get; set; }

        public bool HasStylesheet { get; set; }

        public string StylesheetUrl
        {
            get { return TemplateUrlThunk(ClientResourceName(TemplateId) + ".css"); }
        }

        [ClientProperty("templateUrl")]
        public string TemplateUrl
        {
            get { return TemplateUrlThunk(ClientResourceName(TemplateId)); }
        }

        public ScriptModule()
        {
            DataServiceUrl = CanvasSettings.Default.GetDataServiceUrl(GetType(), CanvasSettings.Default);
            ScriptOptions = Enumerable.Empty<KeyValuePair<string, object>>();
            ModuleState = ModuleState.Maximized;
        }


        public static string ClientResourceName(string id)
        {
            return ClientResourcePrefix + id;
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
                    template = template ?? new ScriptModuleTemplate();

                return new ScriptModule {
                    CanEdit = canEdit,
                    ColumnSpan = source.ColumnSpan,
                    Editor = CreateEditor(source, template) ?? new ScriptModuleEditor(),
                    ExternalIncludes = template.ExternalIncludes ?? Enumerable.Empty<string>(),
                    ModuleId = source.Id,
                    Title = source.Title,
                    Description = source.Description,
                    Position = source.Position,
                    TemplateId = source.TemplateId,
                    HasStylesheet = !string.IsNullOrWhiteSpace(template.Styles),
                    Html = (template.HtmlLayout ?? string.Empty),
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
