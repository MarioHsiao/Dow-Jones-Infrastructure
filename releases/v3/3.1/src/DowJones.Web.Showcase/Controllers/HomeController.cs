using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.Threading;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.Models;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    [HandleError]
    public class HomeController : ControllerBase
    {

        public ActionResult ClientData( )
        {
            var clientState = new ClientState()
                                  {
                                      Data = new {test = "123"},
                                      EventHandlers = new Dictionary<string, string>()
                                      { { "onClick", "handleOnClick"}}
                                  };
            return Content(new JavaScriptSerializer().Serialize(clientState));
        }

        [OutputCache(Duration = 10, VaryByParam = "none")]
        public ActionResult Index()
        {
            return View("Index");
        }

        [OutputCache(Duration = 0, VaryByParam = "none")]
        public ActionResult ExecutionTime()
        {
            var executionTime = DateTime.Now - HttpContext.Timestamp;
            return Content(executionTime.ToString());
        }

        [AspCompat("home/apartmentstate/sta")]
        public ActionResult ApartmentState()
        {
            return View();
        }

        [Route("error")]
        public ActionResult HandleError()
        {
            return View("Error");
        }

        public ActionResult GenericError()
        {
            throw new Exception("This is an example error.");
        }

        public ActionResult FeedDataRetrievalError()
        {
            throw new WebException("This is an example web exception.");
        }


        [Route("test/{id}/{key}", Defaults = "{id:'[0-9]', key:'[a-z]'}")]
        public ActionResult Test(string id)
        {
            
            return View("Index");
        }

        
        public ActionResult AjaxDemo()
        {
            return View("AjaxDemo");
        }

        public ActionResult ViewComponent()
        {
            var model = new PortalHeadlineListModel() {Result = new PortalHeadlineListDataResult()};

            return ViewComponent(model);
        }

        public ActionResult Tokens( )
        {
            return View("Tokens");
        }
    }
}
