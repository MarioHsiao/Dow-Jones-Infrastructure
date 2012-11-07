using System.Runtime.Serialization;
using DowJones.Factiva.Currents.Website.Models.PageService.Modules.TopNews.Packages;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.TopNews.Results
{
    [DataContract(Name = "topNewsNewsPageModuleServicePartResult", Namespace = "")]
    public class TopNewsNewsPageServicePartResult<TPortalHeadlines> : AbstractServicePartResult<TPortalHeadlines>
        where TPortalHeadlines : AbstractTopNewsPackage
    {
    }
}