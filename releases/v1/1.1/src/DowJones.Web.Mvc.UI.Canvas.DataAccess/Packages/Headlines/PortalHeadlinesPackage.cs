using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.Headlines
{
    [DataContract(Name = "portalHeadlinesPackage", Namespace = "")]
    public class PortalHeadlinesPackage : AbstractHeadlinePackage, IViewAllSearchContextRef
    {
        [DataMember(Name = "viewAllSearchContext")]
        public string ViewAllSearchContextRef { get; set; }
    }
}
