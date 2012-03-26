using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Web.Mvc.UI.Components.SocialMedia;

namespace DowJones.Web.Mvc.UI.Components.SocialMedia
{
    public class ExpertsModel :ViewComponentModel
    {
        [ClientData("topExperts")]
        public List<User> Experts { get; set; }

        [ClientProperty("viewAll")]
        public bool IsViewAll { get; set; }
    }


  
   
}
