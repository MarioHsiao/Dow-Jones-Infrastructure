using DowJones.Dash.Components.Model.StatsMap;
using DowJones.Mapping;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Canvas;


namespace DowJones.Dash.Modules
{
	public class PageLoadByRegionModuleModel : Module
	{
		public StatsMapModel StatsMap { get; set; }

		[ClientProperty("mapType")]
		public string MapType { get; set; }

		[ClientProperty("map")]
		public string Map { get; set; }

		public PageLoadByRegionModuleModel()
		{
			ModuleState = ModuleState.Maximized;
			Map = "us";
			MapType = "country";
		}
	}

	public class PageLoadByRegionNewspageModule : DowJones.Pages.Modules.Module
	{
		public PageLoadByRegionMapType MapType { get; set; }
	}

	public enum PageLoadByRegionMapType
	{
		Country,
		World
	}

	public class PageLoadByRegionNewspageModuleMapper :
		TypeMapper<PageLoadByRegionNewspageModule, IModule>
	{
		public override IModule Map(PageLoadByRegionNewspageModule source)
		{
			return new PageLoadByRegionModuleModel
				{
					StatsMap = new StatsMapModel(),
					ModuleId = source.Id,
					Title = source.Title,
					Description = source.Description,
					CanEdit = true,
					MapType = source.MapType.ToString().ToLowerInvariant(),
				};
		}
	}
}