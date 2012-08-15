using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Ajax.SocialMedia;

namespace DowJones.Web.Mvc.UI.Components.SocialMedia
{
    public class ExpertsModel : ViewComponentModel
    {
        [ClientData("experts")]
        public List<User> Experts { get; set; }
    }

  
   
}
