using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.TopNews.Packages;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.TopNews.Results
{
    [DataContract(Name = "topNewsNewsPageModuleServiceResult", Namespace = "")]
    public class TopNewsNewsPageModuleServiceResult :
        AbstractModuleServiceResult<TopNewsNewsPageServicePartResult<AbstractTopNewsPackage>, AbstractTopNewsPackage, TopNewsNewspageModule>
    {
    }
}
