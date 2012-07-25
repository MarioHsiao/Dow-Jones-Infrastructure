using System.Collections.Generic;
using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.SocialButtons;

namespace DowJones.MvcShowcase.Controllers
{
    public class SocialButtonsController : BaseController
    {
        public ActionResult Index()
        {
            var model = new SocialButtonsModel
            {
                SocialNetworks = new List<SocialNetworks>
                                     {
                                         SocialNetworks.Delicious,
                                         SocialNetworks.LinkedIn,
                                         SocialNetworks.Technorati,
                                         SocialNetworks.Yahoo,
                                         SocialNetworks.StumbleUpon
                                     },
                Title = "Share",
                Url = "http://www.dowjones.com",
                Keywords = "Social",
                Description = "Share Content"
            };

            return View(model);
        }
    }
}
