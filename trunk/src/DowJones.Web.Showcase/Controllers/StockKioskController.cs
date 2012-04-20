﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using DowJones.Assemblers.Charting.MarketData;
using DowJones.Managers.Charting.MarketData;
using DowJones.Managers.MarketWatch.Instrument;
using DowJones.Managers.MarketWatch.Symbology;
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

        public ActionResult Index([ModelBinder(typeof(StringSplitModelBinder))]string[] syms, SymbolType symbolType = SymbolType.FCode, Frequency frequency = Frequency.FifteenMinutes, int pageSize = 10)
        {
            var list = new List<string>(syms ?? new[] { "ibm" });
            var response = MarketDataChartingManager.GetMarketChartData(list.ToArray(), symbolType, TimePeriod.OneDay, frequency);

            using (var ms = new MemoryStream())
            {
                var serializer = new DataContractSerializer(response.GetType());
                serializer.WriteObject(ms,response);
            }

            var kioskModel = new StockKioskModel();

            if (response.PartResults != null && response.PartResults.Count() > 0)
            {
                var data = _assembler.Convert(response.PartResults);
                kioskModel.Data = data;
                kioskModel.PageSize = pageSize;
                if (kioskModel.PageSize > 8) kioskModel.PageSize = 8;     //min as per the design to fit in
            }
            return View( "Index", kioskModel);
        } 

        public ActionResult Dylan([ModelBinder(typeof(StringSplitModelBinder))]string[]fCodes)
        {
            var manager = new InstrumentManager();

            manager.GetInstruments(new []{"reinmo"});
            return View("Index", null);
            
        }
    }
}
