using System.Collections.Generic;
using DowJones.Pages.Modules.Templates;
using DowJones.Web.ClientResources;

namespace DowJones.Web.Mvc.UI.Canvas.Modules.ScriptModule.ClientResources
{
    public class ScriptModuleResourceRepository : IClientResourceRepository
    {
        private readonly IScriptModuleTemplateManager _templateManager;

        public ScriptModuleResourceRepository(IScriptModuleTemplateManager templateManager)
        {
            _templateManager = templateManager;
        }

        public IEnumerable<ClientResource> GetClientResources()
        {
            var templates = _templateManager.GetTemplates();

            foreach (var template in templates)
            {
                var name = ScriptModule.ClientResourceName(template.Id);

                yield return new ClientResource(name) {
                                     ResourceKind = ClientResourceKind.Script,
                                     Url = ClientResourceHandler.GenerateUrl(name),
                                };

                yield return new ClientResource(name+".css") {
                                     ResourceKind = ClientResourceKind.Stylesheet,
                                     Url = ClientResourceHandler.GenerateUrl(name),
                                 };
            }
        }
    }
}
