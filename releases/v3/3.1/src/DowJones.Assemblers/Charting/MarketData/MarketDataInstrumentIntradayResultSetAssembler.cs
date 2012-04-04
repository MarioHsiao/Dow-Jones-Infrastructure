using System.Collections.Generic;
using System.Linq;
using DowJones.Managers.Charting.MarketData;
using DowJones.MarketWatch.Dylan.Core.Financialdata;
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
            return Convert(source, null);
        }

        #endregion

        public MarketDataInstrumentIntradayResultSet Convert(ChartDataResponse source, Match match = null, int incrementInMinutes = 15)
        {
            var marketResultSet = new MarketDataInstrumentIntradayResultSet();
            foreach (var kvp in source.Data)
            {
                if (kvp.Key == null)
                    continue;
                var tempKvp = kvp;
                marketResultSet.AddRange(kvp.Value.Sessions.Select(session => GetMarketDataInstrumentIntradayResult(tempKvp.Key, tempKvp.Value.Name, System.Convert.ToBoolean(tempKvp.Value.IsIndex), session, match, incrementInMinutes)));
            }
            return marketResultSet;
        }

        public MarketDataInstrumentIntradayResultSet Convert(IEnumerable<MarketChartDataServicePartResult<MarketChartDataPackage>> source)
        {
            var marketResultSet = new MarketDataInstrumentIntradayResultSet();
            marketResultSet.AddRange(from part in source where part.ReturnCode == 0 select GetMarketDataInstrumentIntradayResult(part.Package.Symbol, part.Package.Name, part.Package.IsIndex, part.Package.Session, part.Package.Match, part.Package.BarSize, part.ReturnCode, part.StatusMessage));
            return marketResultSet;
        }
    }
}