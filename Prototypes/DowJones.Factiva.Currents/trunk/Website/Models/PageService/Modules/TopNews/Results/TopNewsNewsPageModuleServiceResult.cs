using System.Runtime.Serialization;
using DowJones.Factiva.Currents.Website.Models.PageService.Modules.TopNews.Packages;
using TopNewsNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.TopNewsNewspageModule;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.TopNews.Results
{
    [DataContract(Name = "topNewsNewsPageModuleServiceResult", Namespace = "")]
    public class TopNewsNewsPageModuleServiceResult :
        AbstractModuleServiceResult<TopNewsNewsPageServicePartResult<AbstractTopNewsPackage>, AbstractTopNewsPackage, TopNewsNewspageModule>
    {
    }
}
