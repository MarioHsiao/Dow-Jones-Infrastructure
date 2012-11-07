using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.RegionalMap.Packages
{
    [DataContract(Name = "AbstractRegionalMapPackage", Namespace = "")]
    [KnownType(typeof(RegionalMapNewsVolumePackage)), KnownType(typeof(RegionalMapHeadlinesPackage))]
    public abstract class AbstractRegionalMapPackage : IPackage
    {
    }
}