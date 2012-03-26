using System.Collections.Generic;
using System.Linq;
using DowJones.Managers.Charting.MarketData;
using DowJones.Models.Charting.MarketData;
using DowJones.Preferences;
using DowJones.Thunderball.Library.Charting;

namespace DowJones.Assemblers.Charting.MarketData
{
    public class MarketDataInstrumentIntradayResultSetAssembler
        : AbstractMarketDataIntrumentResultAssembler,
          IAssembler<MarketDataInstrumentIntradayResultSet, ChartDataResponse>,
          IAssembler<MarketDataInstrumentIntradayResultSet, IEnumerable<MarketChartDataServicePartResult<MarketChartDataPackage>>>
    {
        public MarketDataInstrumentIntradayResultSetAssembler(IPreferences preferences)
            : base(preferences)
        {
        }

        #region IAssembler<MarketDataInstrumentIntradayResultSet,ChartDataResponse> Members

        public MarketDataInstrumentIntradayResultSet Convert(ChartDataResponse source)
        {
            return Convert(source, 15);
        }

        #endregion

        public MarketDataInstrumentIntradayResultSet Convert(ChartDataResponse source, int incrementInMinutes = 15)
        {
            var marketResultSet = new MarketDataInstrumentIntradayResultSet();
            foreach (var kvp in source.Data)
            {
                if (kvp.Key == null)
                    continue;
                var tempKvp = kvp;
                marketResultSet.AddRange(kvp.Value.Sessions.Select(session => GetMarketDataInstrumentIntradayResult(tempKvp.Key, tempKvp.Value.Name, System.Convert.ToBoolean(tempKvp.Value.IsIndex), session, incrementInMinutes)));
            }
            return marketResultSet;
        }

        public MarketDataInstrumentIntradayResultSet Convert(IEnumerable<MarketChartDataServicePartResult<MarketChartDataPackage>> source)
        {
            var marketResultSet = new MarketDataInstrumentIntradayResultSet();
            marketResultSet.AddRange(from part in source where part.ReturnCode == 0 select GetMarketDataInstrumentIntradayResult(part.Package.Symbol, part.Package.Name, part.Package.IsIndex, part.Package.Session, part.Package.BarSize, part.ReturnCode, part.StatusMessage));
            return marketResultSet;
        }
    }
}