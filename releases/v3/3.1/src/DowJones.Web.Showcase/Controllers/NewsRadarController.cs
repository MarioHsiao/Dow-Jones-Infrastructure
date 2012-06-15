using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DowJones.Web.Mvc.UI.Components.NewsRadar;

namespace DowJones.Web.Showcase.Controllers
{
    public class NewsRadarController : Controller
    {
        public ActionResult Index()
        {
            NewsRadarModel m;
            var serializer = new JavaScriptSerializer();

            using(var sr = new StreamReader(Server.MapPath(Url.Content("~/Views/NewsRadar/sample.json"))))
            {
                m = serializer.Deserialize<NewsRadarModel>(sr.ReadToEnd());
            }

            return View(m);
        }
    }
}