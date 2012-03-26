using DowJones.Web.Mvc.UI.Components.Models;

namespace DowJones.Web.Showcase.Models
{
    public class MultiviiewPortalHeadlinesListModel
    {
        public PortalHeadlineListModel AuthorView
        {
            get;
            set;
        }

        public PortalHeadlineListModel HeadlineView
        {
            get;
            set;
        }

        public PortalHeadlineListModel TimelineView
        {
            get;
            set;
        }
    }
}