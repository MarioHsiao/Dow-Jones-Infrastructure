using System.Runtime.Serialization;
using DowJones.Factiva.Currents.Website.Models.PageService.Modules.Radar.Packages;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Radar.Results
{
    [DataContract(Name = "radarNewsPageModuleServiceResult", Namespace = "")]
    public class RadarNewsPageModuleServiceResult :
        AbstractModuleServiceResult<RadarNewsPageServicePartResult<RadarPackage>, RadarPackage, RadarNewspageModule>
    {
    }
}
