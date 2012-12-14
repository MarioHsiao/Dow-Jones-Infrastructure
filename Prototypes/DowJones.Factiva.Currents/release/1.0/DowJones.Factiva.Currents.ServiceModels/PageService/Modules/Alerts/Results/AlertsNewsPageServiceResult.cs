using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Alerts.Packages;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Alerts.Results
{
    [DataContract(Name = "alertsNewsPageModuleServiceResult", Namespace = "")]
    public class AlertsNewsPageServiceResult :
        AbstractModuleServiceResult<AlertsNewsPageServicePartResult<AlertsPackage>, AlertsPackage, AlertsNewspageModule>
    {}
}
