using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Orchard.Themes;

namespace DowJones.Orchard.DashBoard.Controllers
{
    class DashBoardController: Controller
    {
        [Themed]
        public ActionResult Index()
        {
            return View();
        }
    }
}
