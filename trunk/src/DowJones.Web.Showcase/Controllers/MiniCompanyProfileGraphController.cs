using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Controls.CompanyProfileGraph;
namespace DowJones.Web.Showcase.Controllers
{
    public class MiniCompanyProfileGraphController : DowJonesControllerBase
    {
        //
        // GET: /MiniCompanyProfileGraph/

        public ActionResult Index()
        {
            return Components("Index", GetModel());
        }
        private ContentContainerModel GetModel()
        {
            return new ContentContainerModel(new IViewComponentModel[] { new CompanyProfileGraphModel {
                IsFullChart = false
                
            }});
        }
    }
}
