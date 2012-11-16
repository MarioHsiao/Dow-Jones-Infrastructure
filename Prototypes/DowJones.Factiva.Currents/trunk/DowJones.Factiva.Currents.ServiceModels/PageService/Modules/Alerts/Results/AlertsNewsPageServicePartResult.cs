using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Common.Headlines.Packages;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Alerts.Results
{
    [DataContract(Name = "alertsNewsPageModuleServicePartResult", Namespace = "")]
    public class AlertsNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage>
        where TPackage : AbstractHeadlinePackage
    {
    }
}
