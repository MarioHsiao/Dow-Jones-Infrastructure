using System.Collections.Generic;
using System.Linq;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;

namespace DowJones.Dash.Website.DataSources
{
    public class TopPages : ChartBeatDataSource
    {
        public TopPages() : base("/toppages")
        {
            Parameters.Add("limit", 15);
        }

        protected override dynamic Map(dynamic response)
        {
            var pages =
                from page in (IEnumerable<dynamic>)response
                select new PortalHeadlineInfo
                    {
                        Title = page.i,
                        HeadlineUrl = "http://" + page.path,
                        ModificationTimeDescriptor = page.visitors,
                    };

            return new PortalHeadlineListModel
                {
					Layout = PortalHeadlineListLayout.TimelineLayout,
					Result = new PortalHeadlineListDataResult(new PortalHeadlineListResultSet(pages))
				};
        }
    }
}