using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Company.Packages
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
