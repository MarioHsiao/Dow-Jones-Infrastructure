using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Company.Packages;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Company.Results
{
    [DataContract(Name = "companyOverviewNewsPageModuleServicePart", Namespace = "")]
    public class CompanyOverviewNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage>
        where TPackage : AbstractCompanyPackage
    {
    }
}
