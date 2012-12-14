using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Summary.Packages;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Summary.Results
{
    [DataContract(Name = "summaryNewsPageModuleServicePartResult", Namespace = "")]
    public class SummaryNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage> 
        where TPackage : AbstractSummaryPackage
    {
    }
}