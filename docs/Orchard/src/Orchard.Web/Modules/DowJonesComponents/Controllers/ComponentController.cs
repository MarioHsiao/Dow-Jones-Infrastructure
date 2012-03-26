using System.Net;
using System.Web;
using System.Web.Mvc;
using Orchard.Security;
using Orchard.Themes;
using System.Configuration;
using DowJones.Orchard.SingleSignOn;   

namespace DowJones.Orchard.Components
{
    [Themed]
    public class ComponentController : Controller
    {
        public ActionResult Index()
        {
            //SingleSignOnController a = new SingleSignOnController();
            return View();
        }
    }
}