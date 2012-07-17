using System.Collections.Generic;
using System.Linq;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;

namespace DowJones.Web.Showcase.Models
{
    public class PortalHeadlineLists : Mvc.UI.Canvas.Module
    {
        public IEnumerable<PortalHeadlineListModel> PortalHeadlines { get; set; }

        public PortalHeadlineLists()
        {
            PortalHeadlines = Enumerable.Repeat(
                new PortalHeadlineListModel
                {
                    MaxNumHeadlinesToShow = 5,
                    ShowPublicationDateTime = false,
                    ShowSource = false,
                    DisplayNoResultsToken = true
                }, 3);
        }

    }
}