using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.Assemblers;
using DowJones.Assemblers.Charting.MarketData;
using DowJones.Managers.Charting.MarketData;
using DowJones.Models.Charting.MarketData;
using DowJones.Thunderball.Library.Charting;
using DowJones.Web.Mvc.ModelBinders;
using DowJones.Web.Mvc.UI.Components.StockKiosk;

namespace DowJones.Web.Showcase.Controllers
{
    public class StockKioskController : Controller
    {
        private readonly MarketDataInstrumentIntradayResultSetAssembler _assembler;
        public StockKioskController(MarketDataInstrumentIntradayResultSetAssembler assembler)
        {
            _assembler = assembler;
        }

        public ActionResult Index([ModelBinder(typeof(StringSplitModelBinder))]string[] syms, Frequency frequency = Frequency.FifteenMinutes, int pageSize = 10)
        {
            //var list = new List<string>(new[] { "goog", "msft", "ibm", "cmw", "cac", "mi", "f", "cnw", "col", "cmm", "ci", "cgv", "m", "mcr", "mcd", "mdc", "mdu", "kcw", "kex", "kgc", "kmf", "kmi", "kmp", "kmm" });
            var list = new List<string>(syms ?? new[] { "msft" });
            var response = MarketDataManager.GetMarketChartData(list.ToArray(), TimePeriod.OneDay, frequency);
            var kioskModel = new StockKioskModel();


            if (response.PartResults.Count() > 0)
            {
                var data = _assembler.Convert(response.PartResults);
                kioskModel.Data = data;
                kioskModel.PageSize = pageSize;
                if (kioskModel.PageSize > 8) kioskModel.PageSize = 8;     //min as per the design to fit in
            }

            return View( "Index", kioskModel);
        } 
    }
}
