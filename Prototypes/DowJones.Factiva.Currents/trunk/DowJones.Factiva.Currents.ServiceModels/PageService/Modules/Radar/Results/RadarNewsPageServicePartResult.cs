using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Radar.Packages;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Radar.Results
{
    [DataContract(Name = "radarNewspageModuleServicePartResult", Namespace = "")]
    public class RadarNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage> where TPackage : RadarPackage
    {
    }
}
