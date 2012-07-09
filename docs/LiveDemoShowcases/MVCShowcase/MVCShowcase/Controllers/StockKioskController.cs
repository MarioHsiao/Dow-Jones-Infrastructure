using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using DowJones.Assemblers.Charting.MarketData;
using DowJones.Managers.Charting.MarketData;
using DowJones.Managers.MarketWatch.Instrument;
using DowJones.Models.Charting.MarketData;
using DowJones.Web.Mvc.ModelBinders;
using DowJones.Web.Mvc.Routing;
using DowJones.Web.Mvc.UI.Components.StockKiosk;

namespace DowJones.MvcShowcase.Controllers
{
    public class StockKioskController : BaseController
    {
        private readonly MarketDataInstrumentIntradayResultSetAssembler _assembler;

		public StockKioskController(MarketDataInstrumentIntradayResultSetAssembler assembler)
        {
            _assembler = assembler;
        }

        public ActionResult Index()
        {
			var model = new StockKioskModel
				{
					PageSize = 8,		//min as per the design to fit in
					Data = GetData()
				};

	        return View("Index", model);
        }


		private MarketDataInstrumentIntradayResultSet GetData()
		{
			// arbitrary list of symbols
			var symbols = new[] { "ibm", "mcrost", "goog", "reggr", "carsvc", "cmdbnn", "rgrc", "stgtec", "precos", "comasc" };
			var response = MarketDataChartingManager.GetMarketChartData(symbols);
			
			// if we do not have a reponse, do not process further.
			if (response.PartResults == null || !response.PartResults.Any())
				return null;

			// map response to DTO
			var data = _assembler.Convert(response.PartResults);
			return data;
		}
    }
}
