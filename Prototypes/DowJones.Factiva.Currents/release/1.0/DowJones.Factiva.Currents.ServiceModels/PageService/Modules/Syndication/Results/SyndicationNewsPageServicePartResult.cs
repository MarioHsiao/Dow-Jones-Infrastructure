using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Common.Headlines.Packages;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Syndication.Results
{
    [DataContract(Name = "syndicationNewsPageModuleServicePartResult", Namespace = "")]
    public class SyndicationNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage> 
        where TPackage : AbstractHeadlinePackage
    {
    }
}