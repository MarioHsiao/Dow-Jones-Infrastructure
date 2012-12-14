using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Interfaces;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.RegionalMap.Packages
{
    [DataContract(Name = "AbstractRegionalMapPackage", Namespace = "")]
    [KnownType(typeof(RegionalMapNewsVolumePackage)), KnownType(typeof(RegionalMapHeadlinesPackage))]
    public abstract class AbstractRegionalMapPackage : IPackage
    {
    }
}