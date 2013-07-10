using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DowJones.Web.Mvc.UI.Components.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Components.StockKiosk;

namespace DowJones.Prod.X.Models.Site.Home
{
    public class SearchModel
    {
        public PortalHeadlineListModel Headlines { get; set; }
        public PortalHeadlineListModel Pictures { get; set; }
        public PortalHeadlineListModel Videos { get; set; }
        public StockKioskModel StockKiosk { get; set; }
    }
}
