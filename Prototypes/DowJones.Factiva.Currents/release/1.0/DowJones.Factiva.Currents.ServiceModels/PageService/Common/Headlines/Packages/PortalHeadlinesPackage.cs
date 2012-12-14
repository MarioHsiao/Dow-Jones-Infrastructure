using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Interfaces;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Common.Headlines.Packages
{
    [DataContract(Name = "portalHeadlinesPackage", Namespace = "")]
    public class PortalHeadlinesPackage : AbstractHeadlinePackage, IViewAllSearchContextRef
    {
        [DataMember(Name = "viewAllSearchContext")]
        public string ViewAllSearchContextRef { get; set; }
    }
}
