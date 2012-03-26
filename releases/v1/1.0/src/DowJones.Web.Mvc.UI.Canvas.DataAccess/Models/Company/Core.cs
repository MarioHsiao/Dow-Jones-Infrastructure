// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Core.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Utilities.Formatters;

namespace DowJones.Web.Mvc.UI.Models.Company
{
    public interface IDataPoint
    {
        DateTime? Date { get; set; }

        string DateDisplay { get; set; }
        
        Number DataPoint { get; set; }
    }

    [DataContract(Name = "marketIndexIntradayResult", Namespace = "")]
    public class MarketIndexIntradayResult
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "low")]
        public DoubleNumberStock Low { get; set; }

        [DataMember(Name = "high")]
        public DoubleNumberStock High { get; set; }

        [DataMember(Name = "previousClose")]
        public DoubleNumberStock PreviousClose { get; set; }

        [DataMember(Name = "dataPoints")]
        public BasicDataPointCollection DataPoints { get; set; }

        [DataMember(Name = "date")]
        public DateTime Start { get; set; }

        [DataMember(Name = "dateDescripter")]
        public string StartDescripter { get; set; }

        [DataMember(Name = "provider")]
        public Provider Provider { get; set; }
    }

    [DataContract(Name = "basicDataPoint", Namespace = "")]
    public class BasicDataPoint : IDataPoint
    {
        [DataMember(Name = "date")]
        public DateTime? Date { get; set; }

        [DataMember(Name = "dateDisplay")]
        public string DateDisplay { get; set; }

        [DataMember(Name = "dataPoint")]
        public Number DataPoint { get; set; }
    }

    [DataContract(Namespace = "")]
    public abstract class AbstractBaseCompany
    {
        [DataMember(Name = "companyName")]
        public string CompanyName { get; set; }

        [DataMember(Name = "fcode")]
        public string FCode { get; set; }

        [DataMember(Name = "ticker")]
        public string DjTicker { get; set; }

        [DataMember(Name = "ric")]
        public string Ric { get; set; }

        [DataMember(Name = "listedExchanges")]
        public List<Exchange> ListedExchanges { get; set; }

        [DataMember(Name = "marketIndices")]
        public List<MarketIndex> MarketIndicies { get; set; }
    }

    [DataContract(Name = "marketIndex", Namespace = "")]
    public class MarketIndex
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "descriptor")]
        public string Descriptor { get; set; }
    }

    [DataContract(Name = "exchange", Namespace = "")]
    public class Exchange
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "descriptor")]
        public string Descriptor { get; set; }

        [DataMember(Name = "isPrimary")]
        public bool IsPrimary { get; set; }
    }

    [DataContract(Name = "quote", Namespace = "")]
    public class Quote : AbstractBaseCompany
    {
        [DataMember(Name = "askPrice")]
        public DoubleNumberStock AskPrice { get; set; }

        [DataMember(Name = "bidPrice")]
        public DoubleNumberStock BidPrice { get; set; }

        [DataMember(Name = "change")]
        public DoubleNumber Change { get; set; }

        [DataMember(Name = "closePrice")]
        public DoubleNumber ClosePrice { get; set; }

        [DataMember(Name = "percentageChange")]
        public Percent PercentageChange { get; set; }

        [DataMember(Name = "open")]
        public DoubleNumberStock Open { get; set; }

        [DataMember(Name = "high")]
        public DoubleNumberStock High { get; set; }

        [DataMember(Name = "low")]
        public DoubleNumberStock Low { get; set; }

        [DataMember(Name = "last")]
        public DoubleNumberStock Last { get; set; }

        [DataMember(Name = "closeDate")]
        public DateTime? CloseDate { get; set; }

        [DataMember(Name = "closeDateDisplay")]
        public string CloseDateDisplay { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "fiftyTwoWeekHigh")]
        public DoubleNumberStock FiftyTwoWeekHigh { get; set; }

        [DataMember(Name = "fiftyTwoWeekHighDate")]
        public DateTime? FiftyTwoWeekHighDate { get; set; }

        [DataMember(Name = "fiftyTwoWeekHighDateDisplay")]
        public string FiftyTwoWeekHighDateDisplay { get; set; }

        [DataMember(Name = "fiftyTwoWeekLow")]
        public DoubleNumberStock FiftyTwoWeekLow { get; set; }

        [DataMember(Name = "fiftyTwoWeekLowDate")]
        public DateTime? FiftyTwoWeekLowDate { get; set; }

        [DataMember(Name = "fiftyTwoWeekLowDateDisplay")]
        public string FiftyTwoWeekLowDateDisplay { get; set; }

        [DataMember(Name = "volume")]
        public WholeNumber Volume { get; set; }

        [DataMember(Name = "marketCap")]
        public DoubleMoney MarketCap { get; set; }

        [DataMember(Name = "lastTradeDateTime")]
        public DateTime? LastTradeDateTime { get; set; }

        [DataMember(Name = "lastTradeDateTimeDisplay")]
        public string LastTradeDateTimeDisplay { get; set; }

        [DataMember(Name = "lastTradePrice")]
        public DoubleNumberStock LastTradePrice { get; set; }

        [DataMember(Name = "provider")]
        public Provider Provider { get; set; }
    }

    [DataContract(Name = "stockDataPoint", Namespace = "")]
    public class StockDataPoint : BasicDataPoint
    {
        [DataMember(Name = "closePrice")]
        public DoubleNumberStock ClosePrice { get; set; }
        
        [DataMember(Name = "dayHighPrice")]
        public DoubleNumberStock DayHighPrice { get; set; }

        [DataMember(Name = "dayLowPrice")]
        public DoubleNumberStock DayLowPrice { get; set; }

        [DataMember(Name = "openPrice")]
        public DoubleNumberStock OpenPrice { get; set; }

        [DataMember(Name = "volume")]
        public WholeNumber Volume { get; set; }
    }

    [CollectionDataContract(Name = "stockDataPoints", Namespace = "")]
    public class StockDataPointCollection : List<StockDataPoint>
    {
        public StockDataPointCollection()
        {
        }

        public StockDataPointCollection(IEnumerable<StockDataPoint>collection)
             : base(collection)
        {
        }
    }

    public class BasicDataPointCollection : List<BasicDataPoint>
    {
        public BasicDataPointCollection()
        {
        }

        public BasicDataPointCollection(IEnumerable<StockDataPoint> collection)
             : base(collection)
        {
        }
    }

    [DataContract(Name = "provider", Namespace = "")]
    public class Provider
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        
        [DataMember(Name = "externalUrl")]
        public string ExternalUrl { get; set; }
    }

    [DataContract(Name = "historicalStockDataResult", Namespace = "")]
    public class HistoricalStockDataResult
    {
        [DataMember(Name = "currencyCode")]
        public string CurrencyCode { get; set; }

        [DataMember(Name = "dataPoints")]
        public StockDataPointCollection DataPoints { get; set; }

        [DataMember(Name = "djTicker")]
        public string DjTicker { get; set; }

        [DataMember(Name = "exchange")]
        public Exchange Exchange { get; set; }

        [DataMember(Name = "frequency")]
        public Core.DataPointFrequency? Frequency { get; set; }

        [DataMember(Name = "fromDate")]
        public DateTime? FromDate { get; set; }

        [DataMember(Name = "instrumentName")]
        public string InstrumentName { get; set; }

        [DataMember(Name = "requestedSymbol")]
        public string RequestedSymbol { get; set; }

        [DataMember(Name = "ric")]
        public string Ric { get; set; }

        [DataMember(Name = "provider")]
        public Provider Provider { get; set; }

        [DataMember(Name = "toDate")]
        public DateTime? ToDate { get; set; }
    }
}

