using System.Runtime.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Common.Headlines.Packages;

namespace DowJones.Factiva.Currents.ServiceModels.PageService
{
    [DataContract(Name = "portalHeadlinesServiceResult", Namespace = "")]
    public class PortalHeadlinesServiceResult : AbstractServiceResult
    {
        [DataMember(Name = "package")]
        public PortalHeadlinesPackage Package { get; set; }
    }
}
