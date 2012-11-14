using System.Linq;
using DowJones.Factiva.Currents.Components.CurrentRegionalMap;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.RegionalMap.Results;
using DowJones.Mapping;
using DowJones.Models.Common;
using DowJones.Web.Mvc.UI;

namespace DowJones.Factiva.Currents.Models
{
	public class RegionalMapModel : CompositeComponentModel
	{
		public CurrentRegionalMapModel CurrentRegionalMap { get; set; }
	}

	public class RegionalMapModelMapper : TypeMapper<RegionalMapNewsPageModuleServiceResult, RegionalMapModel>
	{
		public override RegionalMapModel Map(RegionalMapNewsPageModuleServiceResult source)
		{
			if (!source.PartResults.Any())
				return null;

			return new RegionalMapModel
				{
					CurrentRegionalMap = new CurrentRegionalMapModel
						{
							Data = new RegionNewsVolumeResult
								{
									RegionNewsVolume = source.PartResults
															 .First()
															 .Package.RegionNewsVolume
								} 
						}
				};
		}
	}
}