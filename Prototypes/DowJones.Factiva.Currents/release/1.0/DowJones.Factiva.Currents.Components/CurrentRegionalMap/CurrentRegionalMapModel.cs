using DowJones.Models.Common;
using DowJones.Web.Mvc.UI;

namespace DowJones.Factiva.Currents.Components.CurrentRegionalMap
{
	public class CurrentRegionalMapModel : ViewComponentModel
	{
		[ClientData]
		public RegionNewsVolumeResult Data { get; set; }
	}
}