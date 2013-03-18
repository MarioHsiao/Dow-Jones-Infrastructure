using System.Web.Mvc;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.NewsChart;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class CompanyProfileGraphController : ControllerBase
    {
        public ActionResult Index()
        {
            return Components("Index", 
                new ContentContainerModel(new IViewComponentModel[] { new NewsChartModel {
                    ID="companyOverviewChart",
                    IsFullChart = false,
                    onPointClick="ClickFired", 
                    OnZoomSelect= "GetZoomAreaDates",
                    OnZoomReset= "GraphReset"
                }
            }));
        }
    }
}
