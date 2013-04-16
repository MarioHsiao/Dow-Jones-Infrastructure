using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.Gauge;
using DowJones.Web.Showcase.Models;

namespace DowJones.Web.Showcase.Controllers
{
    public class GaugeController : Controller
    {
        //
        // GET: /Gauge/

        public ActionResult Index(double min = 0, double max = 100, double value = 90, uint angle = 45)
        {
            return View("Index", GetModel(min, max, value, angle));
        }


        private GaugeViewModel GetModel(double min = 0, double max = 100, double value = 90, uint angle = 65)
        {
            return new GaugeViewModel
                {
                    Speedometer = new GaugeModel
                        {
                            Min = min,
                            Max = max,
                            Data = value,
                            Angle = angle,
                            GaugeType = GaugeType.Speedometer,
                            Title = "Concurrent Visits",
                            Footer = "Engaged for 0:48m on average"
                        },
                    Meter = new GaugeModel
                        {
                            Min = 0,
                            Max = 12,
                            Data = 6.7,
                            Angle = angle,
                            GaugeType = GaugeType.Meter,
                            Title = "Average Page Load",
                        }
                };
        }
    }
}
