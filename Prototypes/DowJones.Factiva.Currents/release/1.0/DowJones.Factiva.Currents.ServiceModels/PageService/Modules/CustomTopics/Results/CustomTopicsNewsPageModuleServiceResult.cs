using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.CustomTopics.Packages;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.CustomTopics.Results
{
    [DataContract(Name = "customTopicsNewsPageModuleServiceResult", Namespace = "")]
    public class CustomTopicsNewsPageModuleServiceResult :
        AbstractModuleServiceResult<CustomTopicsNewsPageServicePartResult<CustomTopicsPackage>, CustomTopicsPackage, CustomTopicsNewspageModule>
     
    {
    }
}