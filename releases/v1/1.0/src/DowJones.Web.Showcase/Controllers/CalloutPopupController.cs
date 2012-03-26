using System.Web.Mvc;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.CalloutPopup;

namespace DowJones.Web.Showcase.Controllers
{
    public class CalloutPopupController : DowJonesControllerBase
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return View("Index", GetCalloutPupupModel("OnLinkClick"));
        }

        private CalloutPopupModel GetCalloutPupupModel(string clickHandler)
        {
            var calloutp = new CalloutPopupModel() { 
                //OnLinkClick=clickHandler 
            };

            return calloutp;
        }
    }
}
