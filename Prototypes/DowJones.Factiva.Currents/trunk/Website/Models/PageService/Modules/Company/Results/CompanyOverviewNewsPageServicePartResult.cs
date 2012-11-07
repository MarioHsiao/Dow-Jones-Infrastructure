using System.Runtime.Serialization;
using DowJones.Factiva.Currents.Website.Models.PageService.Modules.Company.Packages;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Company.Results
{
    [DataContract(Name = "companyOverviewNewsPageModuleServicePart", Namespace = "")]
    public class CompanyOverviewNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage>
        where TPackage : AbstractCompanyPackage
    {
    }
}
