using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DowJones.Web.Showcase.Areas.Factiva.Modules;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using System.Configuration;
using DowJones.Web.Showcase.Models;

namespace DowJones.Web.Showcase.Areas.Factiva.Controllers
{

    /// <summary>
    /// usage : http://localhost:44355/Factiva/NewsPages?userid=yourUserID&password=yourPassword&productID=16&pageID=thePageID
    /// </summary>

    public class NewspagesController : CanvasControllerBase
    {
        // GET: /Factiva/Newspages/
        public ActionResult Index(int pageID)
        {

            Page page = PageAssetsManager.GetPage(pageID.ToString());

           
            FactivaCanvasModel canvas = new FactivaCanvasModel()
            {
                ControlData = ControlData,
                CanvasID = page.Id.ToString(),
                Preferences = new BasePreferences("en") { ContentLanguages = new ContentLanguageCollection() { "en" } }

            };

            return Canvas(canvas, page);
        }

    }
}
