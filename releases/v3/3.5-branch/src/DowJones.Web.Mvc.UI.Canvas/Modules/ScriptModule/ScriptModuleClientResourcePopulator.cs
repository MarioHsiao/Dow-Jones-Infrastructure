using System;
using System.IO;
using System.Web;
using DowJones.DependencyInjection;
using DowJones.Pages.Modules.Templates;

namespace DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule
{
    public class ScriptModuleClientResourcePopulator : IClientResourceProcessor
    {
        private readonly IScriptModuleTemplateManager _templateManager;

        public HttpContextBase HttpContext { get; set; }

        public ClientResourceProcessorOrder? Order
        {
            get { return ClientResourceProcessorOrder.First; }
        }

        public ClientResourceProcessorKind ProcessorKind
        {
            get { return ClientResourceProcessorKind.Populator; }
        }

        public ScriptModuleClientResourcePopulator(IScriptModuleTemplateManager templateManager)
        {
            _templateManager = templateManager;
        }

        public void Process(ProcessedClientResource resource)
        {
            var name = resource.Name ?? string.Empty;
            if (!name.StartsWith(ScriptModule.ClientResourcePrefix, StringComparison.OrdinalIgnoreCase))
                return;

            var filename = resource.Name.Substring(ScriptModule.ClientResourcePrefix.Length);
            var templateId = Path.GetFileNameWithoutExtension(filename);
            var extension = Path.GetExtension(filename);

            var template = _templateManager.GetTemplate(templateId);

            if (template == null)
                return;

            var isStylesheet = ".css".Equals(extension, StringComparison.OrdinalIgnoreCase);

            resource.Content = isStylesheet ? template.Styles : template.Script;
            resource.ContentLoaded = true;
        }
    }
}
