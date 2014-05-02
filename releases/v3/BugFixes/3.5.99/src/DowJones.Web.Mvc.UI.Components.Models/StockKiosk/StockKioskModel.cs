using DowJones.Managers.Charting.MarketData;
using DowJones.Models.Charting.MarketData;

namespace DowJones.Web.Mvc.UI.Components.StockKiosk
{
    public class StockKioskModel : DowJones.Web.Mvc.UI.ViewComponentModel
    {
        public StockKioskModel()
        {
            TimePeriod = TimePeriod.OneDay;
            Frequency = Frequency.FifteenMinutes;
        }

        [ClientProperty("pagesize")]
        public int PageSize { get; set; }

        [ClientProperty("timePeriod")]
        public TimePeriod TimePeriod { get; set; }

        [ClientProperty("frequency")]
        public Frequency Frequency { get; set; }

        [ClientData]
        public MarketDataInstrumentIntradayResultSet Data  { get; set; }     
    }
}