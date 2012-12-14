using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Interfaces;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Company.Packages
{
    [DataContract(Namespace = "")]
    [KnownType(typeof(CompanyChartPackage))]
    [KnownType(typeof(CompanyRecentArticlesPackage))]
    [KnownType(typeof(CompanySnapshotPackage))]
    [KnownType(typeof(CompanyTrendingPackage))]
    public class AbstractCompanyPackage : IPackage
    {
    }
}
