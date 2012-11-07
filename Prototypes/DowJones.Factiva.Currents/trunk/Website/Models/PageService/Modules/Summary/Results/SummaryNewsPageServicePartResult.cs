using System.Runtime.Serialization;
using DowJones.Factiva.Currents.Website.Models.PageService.Modules.Summary.Packages;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Summary.Results
{
    [DataContract(Name = "summaryNewsPageModuleServicePartResult", Namespace = "")]
    public class SummaryNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage> 
        where TPackage : AbstractSummaryPackage
    {
    }
}