using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DowJones.Web.Mvc.UI.Components.NewsRadar;

namespace DowJones.MvcShowcase.Controllers
{
    public class NewsRadar90DayAvgController : Controller
    {
        //
        // GET: /NewsRadar/

        public ActionResult Index()
        {
            NewsRadarModel m;
            var serializer = new JavaScriptSerializer();

            using (var sr = new StreamReader(Server.MapPath(Url.Content("~/App_Data/NewsRadar90DayAvg.sample.json"))))
            {
                m = serializer.Deserialize<NewsRadarModel>(sr.ReadToEnd());
            }

            return View(m);
        }

    }
}
