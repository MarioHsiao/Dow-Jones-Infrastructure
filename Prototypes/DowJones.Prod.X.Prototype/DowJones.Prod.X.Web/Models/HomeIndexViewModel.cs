using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DowJones.Prod.X.Models.Site;
using DowJones.Prod.X.Models.Site.Home;
using DowJones.Session;

namespace DowJones.Prod.X.Web.Models
{
    public class HomeIndexViewModel : AbstractBasicSiteViewModel<IndexModel>
    {
        public HomeIndexViewModel(Interfaces.IBasicSiteRequestDto basicSiteRequest, IControlData controlData, MainNavigationCategory category)
            : base(basicSiteRequest, controlData, category)
        {
           
        }
    }
}