using System.Collections.Generic;
using DowJones.Ajax.SocialMedia;

namespace DowJones.Web.Mvc.UI.Components.TwitterExperts
{
    public class TwitterExpertsModel : ViewComponentModel
    {
        [ClientData("experts")]
        public List<User> Experts { get; set; }
    }
}
