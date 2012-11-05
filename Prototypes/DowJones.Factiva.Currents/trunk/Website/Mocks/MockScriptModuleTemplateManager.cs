using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;
using DowJones.Pages.Modules.Templates;

namespace DowJones.Factiva.Currents.Website.Mocks
{
	public class MockScriptModuleTemplateManager : IScriptModuleTemplateManager
	{
		private readonly List<ScriptModuleTemplate> _moduleTemplates = new List<ScriptModuleTemplate>();

		public ScriptModuleTemplate GetTemplate(string templateId)
		{
			return _moduleTemplates.FirstOrDefault(x => x.Id == templateId);
		}

		public IEnumerable<ScriptModuleTemplate> GetTemplates()
		{
			return _moduleTemplates;
		}

		public string CreateTemplate(ScriptModuleTemplate template)
		{
			int id = 1;

			if (_moduleTemplates.Any())
				id = _moduleTemplates.Max(x => int.Parse(x.Id)) + 1;

			template.Id = id.ToString();

			_moduleTemplates.Add(template);

			return template.Id;
		}

		public void UpdateTemplate(ScriptModuleTemplate template)
		{
			_moduleTemplates.Replace(x => x.Id == template.Id, template);
		}

		public void DeleteTemplate(string templateId)
		{
			_moduleTemplates.RemoveAll(x => x.Id == templateId);
		}
	}
}