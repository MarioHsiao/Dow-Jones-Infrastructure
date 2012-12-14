using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.RegionalMap.Packages;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.RegionalMap.Results
{
	[DataContract(Name = "regionalMapNewsPageModuleServiceResult", Namespace = "")]
	public class RegionalMapNewsPageModuleServiceResult :
		AbstractModuleServiceResult<RegionalMapNewsPageServicePartResult<RegionalMapNewsVolumePackage>, RegionalMapNewsVolumePackage, RegionalMapNewspageModule>
	{
	}
}