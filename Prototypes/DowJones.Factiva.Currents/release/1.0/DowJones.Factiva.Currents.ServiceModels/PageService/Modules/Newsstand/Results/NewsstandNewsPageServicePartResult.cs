using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Newsstand.Packages;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Newsstand.Results
{
    [DataContract(Name = "newsstandNewsPageModuleServicePartResult", Namespace = "")]
    public class NewsstandNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage>
        where TPackage : AbstractNewsstandPackage
    {
    }
}