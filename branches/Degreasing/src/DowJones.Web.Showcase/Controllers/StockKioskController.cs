using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using DowJones.Assemblers.Charting.MarketData;
using DowJones.Managers.Charting.MarketData;
using DowJones.Managers.MarketWatch.Instrument;
using DowJones.Web.Mvc.ModelBinders;
using DowJones.Web.Mvc.UI.Components.StockKiosk;

namespace DowJones.Web.Showcase.Controllers
{
    public class ConformWriter : XmlTextWriter
    {
        internal void CheckUnicodeString(String value)
        {
            foreach (var t in value)
            {
                if (t > 0xFFFD)
                {
                    throw new Exception("Invalid Unicode");
                }
                if (t < 0x20 && t != '\t' & t != '\n' & t != '\r')
                {
                    throw new Exception("Invalid Xml Characters");
                }
            }
        }

        public ConformWriter(String fileName, Encoding encoding) : base(fileName, encoding) { }

        public ConformWriter(Stream stream, Encoding encoding) : base(stream, encoding)
        {
        }

        public override void WriteString(String value)
        {
            CheckUnicodeString(value);
            base.WriteString(value);
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            base.WriteStartElement(prefix, XmlConvert.EncodeLocalName(localName), ns);
        }
    }

    public class StockKioskControllerModel
    {
        public StockKioskModel Top;
        public StockKioskModel Bottom;
    }

    public class StockKioskController : Controller
    {
        private readonly MarketDataInstrumentIntradayResultSetAssembler _assembler;
        public StockKioskController(MarketDataInstrumentIntradayResultSetAssembler assembler)
        {
            _assembler = assembler;
        }

        public ActionResult Index([ModelBinder(typeof(StringSplitModelBinder))]string[] syms, SymbolType symbolType = SymbolType.FCode, Frequency frequency = Frequency.FifteenMinutes, int pageSize = 10)
        {
            var leftSyms = new List<string>(syms ?? new[] { "reggr", "carsvc", "cmdbnn", "rgrc", "stgtec", "precos", "comasc" });
            var rightSyms = new List<string>(new[] { "ibm", "mcrost", "goog", "reggr", "carsvc", "cmdbnn", "rgrc", "stgtec", "precos", "comasc" });

            var model = new StockKioskControllerModel
                            {
                                Bottom = GetStockKioskModel(rightSyms, symbolType, frequency, pageSize),
                                Top = GetStockKioskModel(leftSyms, symbolType, frequency, pageSize),
                            };

            return View( "Index", model);
        } 

        public ActionResult Dylan([ModelBinder(typeof(StringSplitModelBinder))]string[]fCodes)
        {
            var manager = new InstrumentManager();

            manager.GetInstruments(new []{"reinmo"});
            return View("Index", null);
            
        }

        private StockKioskModel GetStockKioskModel(IEnumerable<string> syms, SymbolType symbolType = SymbolType.FCode, Frequency frequency = Frequency.FifteenMinutes, int pageSize = 10)
        {
            var kioskModel = new StockKioskModel();
            var response = MarketDataChartingManager.GetMarketChartData(syms.ToArray(), symbolType, TimePeriod.OneDay, frequency);
            if (response.PartResults == null || response.PartResults.Count() <= 0)
            {
                return null;
            }
            var data = _assembler.Convert(response.PartResults);
            kioskModel.Data = data;
            kioskModel.PageSize = pageSize;
            if (kioskModel.PageSize > 8) kioskModel.PageSize = 8; //min as per the design to fit in

            return kioskModel;
        }

        public ActionResult ComponentExplorerDemo([ModelBinder(typeof(StringSplitModelBinder))]string[] syms, SymbolType symbolType = SymbolType.FCode, Frequency frequency = Frequency.FifteenMinutes, int pageSize = 10)
        {

            var leftSyms = new List<string>(syms ?? new[] { "reggr", "carsvc", "cmdbnn", "rgrc", "stgtec", "precos", "comasc" });
            var rightSyms = new List<string>(new[] { "ibm", "mcrost", "goog", "reggr", "carsvc", "cmdbnn", "rgrc", "stgtec", "precos", "comasc" });

            var model = new StockKioskControllerModel
            {
                Bottom = GetStockKioskModel(rightSyms, symbolType, frequency, pageSize),
                Top = GetStockKioskModel(leftSyms, symbolType, frequency, pageSize),
            };

            return View("Index", "_Layout_ComponentExplorer", model);
        }
    }
}
