using System.Collections.Generic;
using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.SocialButtons;

namespace DowJones.Web.Showcase.Controllers
{
    public class SocialButtonsController : Controller
    {
        //
        // GET: /SocialButtons/

        public ActionResult Index()
        {
            var socialNetworks = new List<SocialNetworks> {SocialNetworks.Delicious, SocialNetworks.Facebook, SocialNetworks.Digg, SocialNetworks.Yahoo, SocialNetworks.Google};


            var model = new SocialButtonsModel
                            {
                                SocialNetworks = socialNetworks,
                                Title = "TestTitle",
                                Url = "http://www.dowjones.com",
                                Keywords = "Social",
                                Description = "Test Description"
                            };
            return View(model); 
        }

    }
}
