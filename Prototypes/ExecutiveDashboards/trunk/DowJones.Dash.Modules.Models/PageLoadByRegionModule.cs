using DowJones.Dash.Components.Model.StatsMap;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI.Canvas;


namespace DowJones.Dash.Modules
{
	public class PageLoadByRegionModuleModel : Module
	{
		public StatsMapModel StatsMap { get; set; }

		public PageLoadByRegionModuleModel()
		{
			this.ModuleState = ModuleState.Maximized;
		}
	}

	public class PageLoadByRegionNewspageModule : DowJones.Pages.Modules.Module
	{
		public PageLoadByRegionNewspageModule()
		{
			
		}
	}

	public class PageLoadByRegionNewspageModuleMapper :
		TypeMapper<PageLoadByRegionNewspageModule, IModule>
	{
		public override IModule Map(PageLoadByRegionNewspageModule source)
		{
			return new PageLoadByRegionModuleModel
				{
					StatsMap = new StatsMapModel()
				};
		}
	}
}