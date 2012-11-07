using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Summary.Packages
{
    [DataContract(Namespace = "")]
    [KnownType(typeof(SummaryChartPackage))]
    [KnownType(typeof(SummaryRegionalMapPackage))]
    [KnownType(typeof(SummaryVideosPackage))]
    [KnownType(typeof(SummaryRecentArticlesPackage))]
    [KnownType(typeof(SummaryTrendingPackage))]
    public abstract class AbstractSummaryPackage : IPackage
    {
    }
}
