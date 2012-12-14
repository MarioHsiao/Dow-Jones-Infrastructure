using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.TopNews.Packages;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.TopNews.Results
{
    [DataContract(Name = "topNewsNewsPageModuleServicePartResult", Namespace = "")]
    public class TopNewsNewsPageServicePartResult<TPortalHeadlines> : AbstractServicePartResult<TPortalHeadlines>
        where TPortalHeadlines : AbstractTopNewsPackage
    {
    }
}