using System.Runtime.Serialization;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using RegionalMapNewsVolumePackage = DowJones.Factiva.Currents.Website.Models.PageService.Modules.RegionalMap.Packages.RegionalMapNewsVolumePackage;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.RegionalMap.Results
{
	[DataContract(Name = "regionalMapNewsPageModuleServiceResult", Namespace = "")]
	public class RegionalMapNewsPageModuleServiceResult :
		AbstractModuleServiceResult<RegionalMapNewsPageServicePartResult<RegionalMapNewsVolumePackage>, RegionalMapNewsVolumePackage, RegionalMapNewspageModule>
	{
	}
}