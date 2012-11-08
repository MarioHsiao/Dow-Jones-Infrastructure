using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Interfaces;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Newsstand.Packages
{
    [DataContract(Name = "newsstandPackage", Namespace = "")]
    [KnownType(typeof(NewsstandHeadlinesPackage))]
    [KnownType(typeof(NewsstandDiscoveredEntitiesPackage))] 
    [KnownType(typeof(NewsstandHeadlineHitCountsPackage))]
    public abstract class AbstractNewsstandPackage : IPackage
    {
    }
}
