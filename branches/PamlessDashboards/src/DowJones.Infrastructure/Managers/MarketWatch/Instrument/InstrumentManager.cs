// -----------------------------------------------------------------------
// <copyright file="InstrumentManager.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Xml;
using DowJones.Managers.MarketWatch.Core;
using DowJones.Mapping;
using DowJones.MarketWatch.Dylan.Core.Financialdata;
using DowJones.Properties;
using log4net.Core;
using InstrumentType =  DowJones.MarketWatch.Dylan.Core.Symbology.Instrument;

namespace DowJones.Managers.MarketWatch.Instrument
{
    public enum SymbolDialectType
    {
        Factiva,
        Ticker,
        Cusip,
        Sedol,
        Isin,
    } 

    public class SymbolDialectTypeToStringMapper: TypeMapper<SymbolDialectType,string>
    {
        private const string FactivaDialect = "http://system.dowjones.com/dowjones/fcode";
        private const string SymbolDialect = "http://system.marketwatch.com/mktw/symbol";
        private const string CusipDialect = "http://system.dowjones.com/dowjones/cusip";
        private const string SedolDialect = "http://system.dowjones.com/dowjones/sedol";
        private const string IsinDialect = "http://system.dowjones.com/dowjones/isin";

        public override string Map(SymbolDialectType source)
        {
            switch (source)
            {
                case SymbolDialectType.Cusip:
                    return CusipDialect;
                case SymbolDialectType.Isin:
                    return IsinDialect;
                case SymbolDialectType.Sedol:
                    return SedolDialect;
                case SymbolDialectType.Ticker:
                    return SymbolDialect;
                case SymbolDialectType.Factiva:
                default:
                    return FactivaDialect;
            }
        }
    }

    public class InstrumentManager : AbstractMarketWatchService<IInstrument>
    {
        private static readonly BasicHttpBinding BasicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
        private static readonly EndpointAddress EndpointAddress = new EndpointAddress(Settings.Default.DylanInstrumentEndpointAddress);

        /// <summary>
        /// Gets the instrument.
        /// </summary>
        /// <param name="symbols">The factiva codes.</param>
        /// <param name="symbolDialectType">Type of the symbol dialect.</param>
        /// <param name="maxInstrumentMatches">The max instrument matches.</param>
        /// <returns></returns>
        public List<InstrumentByDialectResponse> GetInstruments(IEnumerable<string> symbols, SymbolDialectType symbolDialectType = SymbolDialectType.Factiva, int maxInstrumentMatches = 1)
        {
            var client = new InstrumentClient(BasicHttpBinding, EndpointAddress);

            var request = new GetInstrumentByDialectRequest
                              {
                                  Dialect = Mapper.Map<string>(symbolDialectType),
                                  InstrumentRequests = Normalize(symbols, symbolDialectType).ToArray(),
                                  MaxInstrumentMatches = maxInstrumentMatches,
                                  Needed = GetNeededQNames().ToArray(),
                              };

            GetInstrumentByDialectResponse response = null;

            AddEntitlementToken(client,
                () => {
                        response = ((IInstrument)client).GetInstrumentByDialect(request);
                });

            if (response != null
                && (response.InstrumentResponses != null)
                && (response.InstrumentResponses.Length > 0))
            {
                return new List<InstrumentByDialectResponse>(response.InstrumentResponses);
            }
            return new List<InstrumentByDialectResponse>();
        }


        private static List<InstrumentByDialectRequest> Normalize(IEnumerable<string> symbols, SymbolDialectType symbolDialectType)
        {
            return symbols.Select(symbol => new InstrumentByDialectRequest
                                                  {
                                                      Symbol = symbol, 
                                                      RequestId = string.Concat(symbolDialectType.ToString(),":",symbol),
                                                  }).ToList();
        }

        /// <summary>
        /// This sets What you need returned from dylan you can pass in any permutation of qnames based on your need.
        /// Only pass in the ones you care about to keep message size to a minimum
        /// </summary>
        /// <returns></returns>
        private static List<XmlQualifiedName> GetNeededQNames()
        {
            return new List<XmlQualifiedName>
                       {
                           new XmlQualifiedName("Trading", "http://service.marketwatch.com/ws/2007/10/financialdata"), // Open, High, Low, Close & Volume
                           // new XmlQualifiedName("AfterHoursTrading", "http://service.marketwatch.com/ws/2007/10/financialdata"), // OHLC for after hours
                           // new XmlQualifiedName("BeforeHoursTrading", "http://service.marketwatch.com/ws/2007/10/financialdata"), // OHLC for before hours
                           // new XmlQualifiedName("Financials", "http://service.marketwatch.com/ws/2007/10/financialdata"), // various fields like dividend, shares outstanding, yield...
                           // new XmlQualifiedName("FinancialStatusIndicator", "http://service.marketwatch.com/ws/2007/10/financialdata"), // NASDAQ FSI
                           // new XmlQualifiedName("Offers", "http://service.marketwatch.com/ws/2007/10/financialdata"), // bid/ask info
                           new XmlQualifiedName("TimeZoneInfo", "http://service.marketwatch.com/ws/2007/10/financialdata"), // 52 week high/low, volume averages...
                           //new XmlQualifiedName("TradingStatistics", "http://service.marketwatch.com/ws/2007/10/financialdata"), // 52 week high/low, volume averages...
                           // new XmlQualifiedName("RealTimeTrading", "http://service.marketwatch.com/ws/2007/10/financialdata"), // OHLC from NLS, possibly BATS in the future
                           new XmlQualifiedName("DebugInfo", "http://service.marketwatch.com/ws/2007/10/financialdata"), // OHLC from NLS, possibly BATS in the future
                           new XmlQualifiedName("Meta", "http://service.marketwatch.com/ws/2007/10/financialdata"), // OHLC from NLS, possibly BATS in the future
                           // new XmlQualifiedName("RealTimeBeforeHoursTrading", "http://service.marketwatch.com/ws/2007/10/financialdata"), // OHLC for before hours from NLS
                           // new XmlQualifiedName("RealTimeAfterHoursTrading", "http://service.marketwatch.com/ws/2007/10/financialdata"), // OHLC for after hours from NLS
                           // new XmlQualifiedName("CompositeTrading", "http://service.marketwatch.com/ws/2007/10/financialdata"), // OHLC using realtime and delayed elements
                           // new XmlQualifiedName("CompositeBeforeHoursTrading", "http://service.marketwatch.com/ws/2007/10/financialdata"), // OHLC for before hours using rt and delayed elements
                           // new XmlQualifiedName("CompositeAfterHoursTrading", "http://service.marketwatch.com/ws/2007/10/financialdata") // OHLC for after hours using rt and delayed elemen
                       };
        }
    }
}
