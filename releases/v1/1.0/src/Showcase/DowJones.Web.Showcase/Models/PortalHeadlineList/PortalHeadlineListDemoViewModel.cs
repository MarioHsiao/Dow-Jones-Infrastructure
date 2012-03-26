using DowJones.Web.Mvc.UI.Components.Models;

namespace DowJones.Web.Showcase.Models.PortalHeadlineList
{
    public class PortalHeadlineListDemoPage
    {
        public PortalHeadlineListDemoSection PhilHaack { get; set; }
        public PortalHeadlineListDemoSection ScottGu { get; set; }
        public PortalHeadlineListDemoSection Hanselman { get; set; }
    }

    public class PortalHeadlineListDemoSection
    {
        public string Title { get; set; }
        public PortalHeadlineListModel HeadlineList { get; set; }
    }
}