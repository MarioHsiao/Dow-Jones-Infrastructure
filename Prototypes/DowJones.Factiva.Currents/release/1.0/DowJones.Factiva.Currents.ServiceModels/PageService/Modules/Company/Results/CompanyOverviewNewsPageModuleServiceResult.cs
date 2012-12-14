using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Company.Packages;
using DowJones.Formatters;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Company.Results
{
    [DataContract(Name = "companyOverviewNewsPageModuleServiceResult", Namespace = "")]
    [KnownType(typeof(CompanyChartPackage))]
    [KnownType(typeof(CompanyTrendingPackage))]
    [KnownType(typeof(CompanySnapshotPackage))]
    [KnownType(typeof(CompanyRecentArticlesPackage))]
    public class CompanyOverviewNewsPageModuleServiceResult :
        AbstractModuleServiceResult<CompanyOverviewNewsPageServicePartResult<AbstractCompanyPackage>, AbstractCompanyPackage, CompanyOverviewNewspageModule>
    {
        
        [DataMember(Name = "companyName")]
        public string CompanyName { get; set; }

        [DataMember(Name = "fcode")]
        public string FCode { get; set; }

        [DataMember(Name = "hasPublicCompanyInformation")]
        public bool HasPublicCompanyInformation { get; set; }
        
        
        internal class Additionaldata
        {
            internal DoubleMoney MarketCap { get; set; }

            internal string Exchange { get; set; }

            internal string ExchangeDescriptor { get; set; }
        }
    }
}
