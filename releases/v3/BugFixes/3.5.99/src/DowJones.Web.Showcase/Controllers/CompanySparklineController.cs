using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.Ajax.CompanySparkline;
using DowJones.Managers.Sparkline;
using DowJones.Web.Mvc.Search.Controllers;
using DowJones.Web.Mvc.UI.Components.CompanySparkline;

namespace DowJones.Web.Showcase.Controllers
{
    public class CompanySparklineController : SearchControllerBase
    {

        public ActionResult Index()
        {
            CompaniesSparklinesModel basicModel = GetBasicModel();
            basicModel.DataServiceUrl = Url.Action("Sparklines");
            return View("Index", basicModel);
        }
                                                       
        [HttpGet]
        public ActionResult Sparkline([Bind(Prefix = "c")] IEnumerable<string> companies)
        {
            return Json(GetBasicModel().Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Sparklines( [Bind(Prefix = "c")] IEnumerable<string> companyCodes, [Bind( Prefix = "np" )] uint dataPoints = 5u )
        {
            var request = new SparklineServiceRequest { CompanyCodes = companyCodes, DataPoints = dataPoints };
            return base.Sparklines(request);
        }

        private CompaniesSparklinesModel GetBasicModel()
        {
            IEnumerable<SparklineDataSet> response = SparklineService.GetData(new SparklineServiceRequest {CompanyCodes = new[] {"GOOG", "MCROST", "IBM", "APPL"}});
            var mapped = response.Select(Mapper.Map<CompanySparklineDataResult>).ToList();

            var temp = new CompaniesSparklinesModel
                           {
                               SeriesColorForIncrease = "#009933",
                               SeriesColorForDecrease = "#FF0000",
                               BaselineSeriesColor = "#666666",
                               IsClientMode = true,
                               Data = mapped
                           };       
            return temp;
        }
    }
}