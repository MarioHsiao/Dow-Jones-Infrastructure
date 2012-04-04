using System.Linq;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Models.Charting.MarketData;
using DowJones.Preferences;
using DowJones.Thunderball.Library.Charting;

namespace DowJones.Assemblers.Charting.MarketData
{
    public class MarketDataInstrumentIntradayResultAssembler : AbstractMarketDataIntrumentResultAssembler, IAssembler<MarketDataInstrumentIntradayResult, ChartDataResponse>
    {

        public MarketDataInstrumentIntradayResultAssembler(IPreferences preferences)
            : base(preferences)
        {
        }

        public MarketDataInstrumentIntradayResult Convert(ChartDataResponse source)
        {
            return (from kvp in source.Data let session = kvp.Value.Sessions[0] select GetMarketDataInstrumentIntradayResult(kvp.Key, kvp.Value.Name, System.Convert.ToBoolean(kvp.Value.IsIndex), session, null)).FirstOrDefault();
        }

        public MarketDataInstrumentIntradayResult Convert(ChartDataResponse source, int incrementInMinutes = 15)
        {
            return (from kvp in source.Data let session = kvp.Value.Sessions[0] select GetMarketDataInstrumentIntradayResult(kvp.Key, kvp.Value.Name, System.Convert.ToBoolean(kvp.Value.IsIndex), session, null, incrementInMinutes)).FirstOrDefault();
        }
    }
}
