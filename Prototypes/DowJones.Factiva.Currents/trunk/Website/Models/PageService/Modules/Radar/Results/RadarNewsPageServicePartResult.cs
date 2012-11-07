using System.Runtime.Serialization;
using DowJones.Factiva.Currents.Website.Models.PageService.Modules.Radar.Packages;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Radar.Results
{
    [DataContract(Name = "radarNewspageModuleServicePartResult", Namespace = "")]
    public class RadarNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage> where TPackage : RadarPackage
    {
    }
}
