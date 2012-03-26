using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Web.Mvc.UI.Components.Twitter
{
    public class ProfileModel : ViewComponentModel
    {

        [ClientData("profile")]
        public UserDetails Profile { get; set; }

        [ClientProperty("splitView")]
        public bool IsSplitView { get; set; }

    }
}
