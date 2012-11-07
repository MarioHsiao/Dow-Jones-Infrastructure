using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Sources.Results
{
    [DataContract(Name = "sourcesNewsPageModuleServicePartResult", Namespace = "")]
    public class SourcesNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage> 
        where TPackage : AbstractHeadlinePackage
    {
    }
}