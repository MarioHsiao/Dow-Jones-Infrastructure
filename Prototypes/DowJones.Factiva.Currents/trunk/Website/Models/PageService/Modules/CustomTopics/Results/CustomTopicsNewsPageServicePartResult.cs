using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.CustomTopics.Results
{
    [DataContract(Name = "customTopicsNewsPageModuleServicePartResult", Namespace = "")]
    public class CustomTopicsNewsPageServicePartResult<TPackage> :
        AbstractServicePartResult<TPackage> where TPackage : AbstractHeadlinePackage
    {
    }
}