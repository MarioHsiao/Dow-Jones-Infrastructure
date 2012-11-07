using System.Runtime.Serialization;
using DowJones.Factiva.Currents.Website.Models.PageService.Modules.Summary.Packages;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Summary.Results
{
    [DataContract(Name = "summaryNewsPageModuleServiceResult", Namespace = "")]
    public class SummaryNewsPageModuleServiceResult :
        AbstractModuleServiceResult<SummaryNewsPageServicePartResult<AbstractSummaryPackage>, AbstractSummaryPackage, SummaryNewspageModule>
    {
        [DataMember(Name = "hasMarketDataIndex")]
        public bool HasMarketDataIndex { get; set; }
    }
}
