using System.Linq;
using DowJones.Dash.DataGenerators;
using DowJones.Infrastructure;
using DowJones.Pages.Modules.Templates;

namespace DowJones.Dash.Website.App_Start
{
    public class ScriptModuleTemplatesInitializer : IBootstrapperTask
    {
        private readonly IScriptModuleTemplateManager _templateManager;
        private readonly ScriptModuleTemplatesGenerator _templatesGenerator;

        public ScriptModuleTemplatesInitializer(
                IScriptModuleTemplateManager templateManager,
                ScriptModuleTemplatesGenerator templatesGenerator
            )
        {
            _templateManager = templateManager;
            _templatesGenerator = templatesGenerator;
        }

        public void Execute()
        {
            if (_templateManager.GetTemplates().Count() != 0)
                return;

            var templates = _templatesGenerator.GenerateScripModuleTemplates();

            foreach (var template in templates)
            {
                _templateManager.CreateTemplate(template);
            }
        }
    }
}