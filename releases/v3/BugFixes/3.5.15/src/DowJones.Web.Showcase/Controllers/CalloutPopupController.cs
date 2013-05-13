using System.Web.Mvc;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.CalloutPopup;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class CalloutPopupController : ControllerBase
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
