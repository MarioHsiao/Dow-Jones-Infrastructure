using System.Collections.Generic;
using System.Linq;
using DowJones.Managers.Charting.MarketData;
using DowJones.MarketWatch.Dylan.Core.Financialdata;
using DowJones.Models.Charting.MarketData;
using DowJones.Preferences;
using DowJones.Thunderball.Library.Charting;

namespace DowJones.Assemblers.Charting.MarketData
{
    /// <summary>
    /// 
    /// </summary>
    public class MarketDataInstrumentIntradayResultSetAssembler
        : AbstractMarketDataIntrumentResultAssembler,
          IAssembler<MarketDataInstrumentIntradayResultSet, ChartDataResponse>,
          IAssembler<MarketDataInstrumentIntradayResultSet, IEnumerable<MarketChartDataServicePartResult<MarketChartDataPackage>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketDataInstrumentIntradayResultSetAssembler" /> class.
        /// </summary>
        /// <param name="preferences">The preferences.</param>
        public MarketDataInstrumentIntradayResultSetAssembler(IPreferences preferences)
            : base(preferences)
        {
        }

        #region IAssembler<MarketDataInstrumentIntradayResultSet,ChartDataResponse> Members

        /// <summary>
        /// Converts the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public MarketDataInstrumentIntradayResultSet Convert(ChartDataResponse source)
        {
            return Convert(source, null);
        }

        #endregion

        /// <summary>
        /// Converts the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="match">The match.</param>
        /// <param name="incrementInMinutes">The increment in minutes.</param>
        /// <returns></returns>
        public MarketDataInstrumentIntradayResultSet Convert(ChartDataResponse source, Match match = null, int incrementInMinutes = 15)
        {
            var marketResultSet = new MarketDataInstrumentIntradayResultSet();
            foreach (var kvp in source.Data)
            {
                if (kvp.Key == null)
                    continue;
                var tempKvp = kvp;
                marketResultSet.AddRange(kvp.Value.Sessions.Select(session => GetMarketDataInstrumentIntradayResult(tempKvp.Key, tempKvp.Value.Name, System.Convert.ToBoolean(tempKvp.Value.IsIndex), session, match, null, incrementInMinutes)));
            }
            return marketResultSet;
        }

        /// <summary>
        /// Converts the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public MarketDataInstrumentIntradayResultSet Convert(IEnumerable<MarketChartDataServicePartResult<MarketChartDataPackage>> source)
        {
            var marketResultSet = new MarketDataInstrumentIntradayResultSet();
            marketResultSet.AddRange(from part in source where part.ReturnCode == 0 select GetMarketDataInstrumentIntradayResult(part.Package.Symbol, part.Package.Name, part.Package.IsIndex, part.Package.Session, part.Package.Match, part.Package.RequestId, part.Package.BarSize, part.ReturnCode, part.StatusMessage));
            return marketResultSet;
        }
    }
}