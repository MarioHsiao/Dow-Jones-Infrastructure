using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Alerts.Results
{
    [DataContract(Name = "alertsNewsPageModuleServicePartResult", Namespace = "")]
    public class AlertsNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage>
        where TPackage : AbstractHeadlinePackage
    {
    }
}
