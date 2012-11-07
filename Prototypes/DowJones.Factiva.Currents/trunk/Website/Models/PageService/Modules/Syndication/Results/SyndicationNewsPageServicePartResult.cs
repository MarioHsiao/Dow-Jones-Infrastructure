using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Syndication.Results
{
    [DataContract(Name = "syndicationNewsPageModuleServicePartResult", Namespace = "")]
    public class SyndicationNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage> 
        where TPackage : AbstractHeadlinePackage
    {
    }
}