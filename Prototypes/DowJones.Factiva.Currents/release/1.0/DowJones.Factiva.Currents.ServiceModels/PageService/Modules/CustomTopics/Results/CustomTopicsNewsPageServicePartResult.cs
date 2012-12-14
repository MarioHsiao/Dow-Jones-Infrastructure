using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Common.Headlines.Packages;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.CustomTopics.Results
{
    [DataContract(Name = "customTopicsNewsPageModuleServicePartResult", Namespace = "")]
    public class CustomTopicsNewsPageServicePartResult<TPackage> :
        AbstractServicePartResult<TPackage> where TPackage : AbstractHeadlinePackage
    {
    }
}