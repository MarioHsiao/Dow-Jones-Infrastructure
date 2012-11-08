using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Interfaces;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Summary.Packages
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
