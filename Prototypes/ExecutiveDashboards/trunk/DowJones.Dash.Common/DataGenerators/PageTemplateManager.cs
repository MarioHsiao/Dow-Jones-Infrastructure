using System.Collections.Generic;
using DowJones.Dash.Modules;
using DowJones.Pages.Modules;

namespace DowJones.Dash.DataGenerators
{
	public class PageTemplateManager : IPageTemplateManager
	{
		#region Implementation of IPageTemplateManager

		public IEnumerable<Module> GetDefaultModules()
		{
			return new List<Module>
				{
					new PageLoadByRegionNewspageModule  { Title = "Page Load By Region", Description = "Average page load times, broken down by state" },
				};
		}

		#endregion
	}
}