using System.Runtime.Serialization;
using DowJones.Factiva.Currents.Website.Models.PageService.Modules.Newsstand.Packages;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Newsstand.Results
{
    [DataContract(Name = "newsstandNewsPageModuleServicePartResult", Namespace = "")]
    public class NewsstandNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage>
        where TPackage : AbstractNewsstandPackage
    {
    }
}