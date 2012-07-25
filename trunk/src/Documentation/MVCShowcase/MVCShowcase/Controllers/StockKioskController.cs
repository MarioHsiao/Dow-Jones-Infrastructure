using System.Linq;
using System.Web.Mvc;
using DowJones.Assemblers.Charting.MarketData;
using DowJones.Managers.Charting.MarketData;
using DowJones.Models.Charting.MarketData;
using DowJones.Web.Mvc.UI.Components.StockKiosk;

namespace DowJones.MvcShowcase.Controllers
{
	public class StockKioskController : BaseController
	{
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
			// arbitrary list of FCodes
			var fCodes = new[] { "ibm", "mcrost", "goog", "reggr", "carsvc", "cmdbnn", "rgrc", "stgtec", "precos", "comasc" };
			var response = MarketDataChartingManager.GetMarketChartData(fCodes);

			// if we do not have a reponse, do not process further.
			if (response.PartResults == null || !response.PartResults.Any())
				return null;

			// map response to DTO
			var assembler = new MarketDataInstrumentIntradayResultSetAssembler(new Preferences.Preferences("en"));
			var data = assembler.Convert(response.PartResults);
			return data;
		}
	}
}
