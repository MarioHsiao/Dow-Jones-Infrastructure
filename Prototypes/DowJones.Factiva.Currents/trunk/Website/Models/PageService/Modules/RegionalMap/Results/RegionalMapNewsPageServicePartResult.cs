using System.Runtime.Serialization;
using DowJones.Factiva.Currents.Website.Models.PageService.Modules.RegionalMap.Packages;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.RegionalMap.Results
{
    [DataContract(Name = "regionalMapNewsPageModuleServicePartResult", Namespace = "")]
    public class RegionalMapNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage> where TPackage : RegionalMapNewsVolumePackage
    {
    }
}