using System.Collections.Generic;
using DowJones.Pages.Modules;

namespace DowJones.Dash
{
	public interface IPageTemplateManager
	{
		IEnumerable<Module> GetDefaultModules();
	}
}