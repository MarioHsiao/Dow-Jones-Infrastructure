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
            var existingTemplates = _templateManager.GetTemplates();
            var generatedTemplates = _templatesGenerator.GenerateScripModuleTemplates();

            var templatesToAdd =
                (
                    from generated in generatedTemplates
                    join ex in existingTemplates on generated.Title equals ex.Title into joined
                    from existing in joined.DefaultIfEmpty()
                    where existing == null
                    select generated
                ).ToArray();
                
            foreach (var template in templatesToAdd)
            {
                _templateManager.CreateTemplate(template);
            }
        }
    }
}