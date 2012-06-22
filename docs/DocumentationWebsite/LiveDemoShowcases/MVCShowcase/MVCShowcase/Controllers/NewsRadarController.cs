﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DowJones.Web.Mvc.UI.Components.NewsRadar;

namespace DowJones.MvcShowcase.Controllers
{
    public class NewsRadarController : Controller
    {
        //
        // GET: /NewsRadar/

        public ActionResult Index()
        {
            NewsRadarModel m;
            var serializer = new JavaScriptSerializer();

            using (var sr = new StreamReader(Server.MapPath(Url.Content("~/Views/NewsRadar/sample.json"))))
            {
                m = serializer.Deserialize<NewsRadarModel>(sr.ReadToEnd());
            }

            return View(m);
        }

    }
}
