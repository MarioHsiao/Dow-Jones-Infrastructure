using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Newsstand.Packages
{
    [DataContract(Name = "newsstandPackage", Namespace = "")]
    [KnownType(typeof(NewsstandHeadlinesPackage))]
    [KnownType(typeof(NewsstandDiscoveredEntitiesPackage))] 
    [KnownType(typeof(NewsstandHeadlineHitCountsPackage))]
    public abstract class AbstractNewsstandPackage : IPackage
    {
    }
}
