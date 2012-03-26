using System.Runtime.Serialization;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.TopNewsCommunicator
{
    [DataContract(Name = "topNewsCommunicatorBreakingNewsPackage", Namespace = "")]
    public class TopNewsCommunicatorBreakingNewsPackage : AbstractTopNewsCommunicatorPackage, IPortalHeadlines
    {
        PortalHeadlineListDataResult headlineListDataResult;
        [DataMember(Name = "portalHeadlineListDataResult")]
        public PortalHeadlineListDataResult Result
        {
            get { return headlineListDataResult ?? (headlineListDataResult = new PortalHeadlineListDataResult()); }
            set { headlineListDataResult = value; }
        }
    }
}
