using System.Collections.Generic;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;

namespace DowJones.Web.Showcase.Components.CompanyHeadlines
{
    public class CompanyHeadlinesModel : CompositeComponentModel
    {
        [ClientData]
        public IEnumerable<string> Companies { get; set; }

        [ClientData]
        public PortalHeadlineListModel Headlines { get; set; }
    }
}