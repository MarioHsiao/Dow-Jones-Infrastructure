using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.Gauge;

namespace DowJones.Web.Showcase.Controllers
{
    public class GaugeController : Controller
    {
        //
        // GET: /Gauge/

        public ActionResult Index(decimal min = 0, decimal max=100, decimal value=90, uint angle = 45)
        {
            return View("Index", new GaugeModel
                {
                    Min = min,
                    Max = max,
                    Data = value,
                    Angle = angle,
                    Title = "Concurrent Visits",
                    Footer = "Engaged for 0:48m on average" 
                });
        }

    }
}
