using System.Collections.Generic;
using System.Linq;
using DowJones.Pages.Modules.Templates;
using Raven.Client;

namespace DowJones.Web.Mvc.UI.Canvas.RavenDb
{
    // TODO: Make this less chatty by changing all of the .SaveChanges() to unit of work pattern!
    /// <summary>
    /// RavenDB Script Module Template Repository implementation
    /// </summary>
    public class RavenDbScriptModuleTemplateRepository : IScriptModuleTemplateManager
    {
        private readonly IDocumentSession _session;

        public RavenDbScriptModuleTemplateRepository(IDocumentSession session)
        {
            _session = session;
        }

        public string CreateTemplate(ScriptModuleTemplate template)
        {
            _session.Store(template, template.Id);
            _session.SaveChanges();
            return template.Id;
        }

        public ScriptModuleTemplate GetTemplate(string templateId)
        {
            return _session.Query<ScriptModuleTemplate>()
                .FirstOrDefault(x => x.Id == templateId);
        }

        public IEnumerable<ScriptModuleTemplate> GetTemplates()
        {
            return _session.Query<ScriptModuleTemplate>();
        }

        public void UpdateTemplate(ScriptModuleTemplate template)
        {
            _session.Store(template);
            _session.SaveChanges();
        }

        public void DeleteTemplate(string templateId)
        {
            var template = GetTemplate(templateId);
            
            if (template == null)
                return;
            
            _session.Delete(template);
            _session.SaveChanges();
        }
    }
}