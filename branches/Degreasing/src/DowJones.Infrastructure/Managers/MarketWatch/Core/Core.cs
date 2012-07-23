// -----------------------------------------------------------------------
// <copyright file="Core.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml;
using DowJones.MarketWatch.Dylan.Core.Financialdata;
using DowJones.MarketWatch.Dylan.Core.Financialdata.Types;
using DowJones.MarketWatch.Dylan.Core.Schemas;
using DowJones.MarketWatch.Dylan.Core.Symbology;
using DowJones.MarketWatch.Dylan.Core.Utility;
using TimeZoneInfo = DowJones.MarketWatch.Dylan.Core.Financialdata.TimeZoneInfo;

namespace DowJones.MarketWatch.Dylan.Core.Schemas
{
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ExtensibleDataObjectBase", Namespace = "http://schemas.datacontract.org/2004/07/DowJones.Framework.Schemas")]
    [KnownType(typeof(Instrument))]
    [KnownType(typeof(Currency))]
    [KnownType(typeof(Market))]
    [KnownType(typeof(Industry))]
    [KnownType(typeof(InstrumentRequest))]
    [KnownType(typeof(GetInstrumentResponse))]
    [KnownType(typeof(InstrumentResponse))]
    [KnownType(typeof(Match))]
    [KnownType(typeof(Trading))]
    [KnownType(typeof(Trade))]
    [KnownType(typeof(AfterHoursTrading))]
    [KnownType(typeof(Offers))]
    [KnownType(typeof(TradingStatistics))]
    [KnownType(typeof(CurrencyStatistic))]
    [KnownType(typeof(CalendarRange))]
    [KnownType(typeof(Duration))]
    [KnownType(typeof(DecimalStatistic))]
    [KnownType(typeof(Financials))]
    [KnownType(typeof(BeforeHoursTrading))]
    [KnownType(typeof(FinancialStatusIndicator))]
    [KnownType(typeof(IndustryClassification))]
    [KnownType(typeof(FutureSpecific))]
    [KnownType(typeof(BondSpecific))]
    [KnownType(typeof(BlueGrassChannel))]
    [KnownType(typeof(Financialdata.TimeZoneInfo))]
    [KnownType(typeof(DebugInfo))]
    [KnownType(typeof(CurrencySpecific))]
    [KnownType(typeof(DatafeedSpecificCurrency))]
    [KnownType(typeof(Meta))]
    [KnownType(typeof(RoncoSpecific))]
    [KnownType(typeof(GetEntityRequest))]
    [KnownType(typeof(EntityRequest))]
    [KnownType(typeof(GetEntityResponse))]
    [KnownType(typeof(EntityResponse))]
    [KnownType(typeof(EntityMatch))]
    [KnownType(typeof(Entity))]
    [KnownType(typeof(EntityInstrument))]
    [KnownType(typeof(EntityInstrumentLink))]
    [KnownType(typeof(GetExchangeSummaryRequest))]
    [KnownType(typeof(ExchangeSummaryRequest))]
    [KnownType(typeof(GetExchangeSummaryResponse))]
    [KnownType(typeof(ExchangeSummaryResponse))]
    [KnownType(typeof(ExchangeSummary))]
    [KnownType(typeof(InstrumentByDialectRequest))]
    [KnownType(typeof(InstrumentByDialectResponse))]
    [KnownType(typeof(GetInstrumentRequest))]
    public class ExtensibleDataObjectBase : object, IExtensibleDataObject
    {
        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }
}

namespace DowJones.MarketWatch.Dylan.Core.Symbology
{
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "Instrument", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class Instrument : ExtensibleDataObjectBase
    {
        [DataMember]
        public XmlQualifiedName[] Types { get; set; }

        [DataMember(Order = 1)]
        public string[] Debug { get; set; }

        [DataMember(Order = 2)]
        public string CommonName { get; set; }

        [DataMember(Order = 3)]
        public string Ticker { get; set; }

        [DataMember(Order = 4)]
        public string Cusip { get; set; }

        [DataMember(Order = 5)]
        public string Sedol { get; set; }

        [DataMember(Order = 6)]
        public string Isin { get; set; }

        [DataMember(Order = 7)]
        public DateTime? Expires { get; set; }

        [DataMember(Order = 8)]
        public Currency Strike { get; set; }

        [DataMember(Order = 9)]
        public Market Exchange { get; set; }

        [DataMember(Order = 10)]
        public Instrument Issuer { get; set; }

        [DataMember(Order = 11)]
        public Instrument Underlying { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "Market", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class Market : ExtensibleDataObjectBase
    {
        [DataMember]
        public XmlQualifiedName[] Types { get; set; }

        [DataMember(Order = 1)]
        public string[] Debug { get; set; }

        [DataMember(Order = 2)]
        public string CommonName { get; set; }

        [DataMember(Order = 3)]
        public string Ticker { get; set; }

        [DataMember(Order = 4)]
        public string CountryCode { get; set; }

        [DataMember(Order = 5)]
        public string MarketCode { get; set; }

        [DataMember(Order = 6)]
        public int? ExchangeDelay { get; set; }

        [DataMember(Order = 7)]
        public string IsoCode { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "Industry", Namespace = "http://service.marketwatch.com/ws/2007/05/symbology")]
    public class Industry : ExtensibleDataObjectBase
    {
        [DataMember]
        public Instrument Instrument { get; set; }

        [DataMember(Order = 1)]
        public string DisplayName { get; set; }
    }
}

namespace DowJones.MarketWatch.Dylan.Core.Utility
{
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "Currency", Namespace = "http://service.marketwatch.com/ws/2007/05/utility")]
    public class Currency : ExtensibleDataObjectBase
    {
        [DataMember]
        public string Iso { get; set; }

        [DataMember]
        public decimal Value { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "CalendarRange", Namespace = "http://service.marketwatch.com/ws/2007/05/utility")]
    public class CalendarRange : ExtensibleDataObjectBase
    {
        [DataMember]
        public XmlQualifiedName Type { get; set; }

        [DataMember(Order = 1)]
        public DateTime? Begin { get; set; }

        [DataMember(Order = 2)]
        public DateTime? End { get; set; }

        [DataMember(Order = 3)]
        public Duration Duration { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "Duration", Namespace = "http://service.marketwatch.com/ws/2007/05/utility")]
    public class Duration : ExtensibleDataObjectBase
    {
        [DataMember]
        public string ValueString { get; set; }
    }
}

namespace DowJones.MarketWatch.Dylan.Core.Financialdata
{
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "InstrumentRequest", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class InstrumentRequest : ExtensibleDataObjectBase
    {
        [DataMember]
        public string RequestId { get; set; }

        [DataMember(Order = 1)]
        public Symbology.Instrument Instrument { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetInstrumentResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetInstrumentResponse : ExtensibleDataObjectBase
    {
        [DataMember]
        public InstrumentResponse[] InstrumentResponses { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "InstrumentResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class InstrumentResponse : ExtensibleDataObjectBase
    {
        [DataMember]
        public string RequestId { get; set; }

        [DataMember(Order = 1)]
        public Match[] Matches { get; set; }

        [DataMember(Order = 2)]
        public int FilteredCount { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "Match", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class Match : ExtensibleDataObjectBase
    {
        [DataMember]
        public Instrument Instrument { get; set; }

        [DataMember]
        public Instrument UpdatedInstrument { get; set; }

        [DataMember(Order = 2)]
        public Trading Trading { get; set; }

        [DataMember(Order = 3)]
        public AfterHoursTrading AfterHoursTrading { get; set; }

        [DataMember(Order = 4)]
        public Offers Offers { get; set; }

        [DataMember(Order = 5)]
        public TradingStatistics TradingStatistics { get; set; }

        [DataMember(Order = 6)]
        public Financials Financials { get; set; }

        [DataMember(Order = 7)]
        public BeforeHoursTrading BeforeHoursTrading { get; set; }

        [DataMember(Order = 8)]
        public FinancialStatusIndicator FinancialStatusIndicator { get; set; }

        [DataMember(Order = 9)]
        public Trading RealtimeTrading { get; set; }

        [DataMember(Order = 10)]
        public BeforeHoursTrading RealTimeBeforeHoursTrading { get; set; }

        [DataMember(Order = 11)]
        public AfterHoursTrading RealTimeAfterHoursTrading { get; set; }

        [DataMember(Order = 12)]
        public Trading CompositeTrading { get; set; }

        [DataMember(Order = 13)]
        public BeforeHoursTrading CompositeBeforeHoursTrading { get; set; }

        [DataMember(Order = 14)]
        public AfterHoursTrading CompositeAfterHoursTrading { get; set; }

        [DataMember(Order = 15)]
        public IndustryClassification IndustryClassification { get; set; }

        [DataMember(Order = 16)]
        public FutureSpecific FutureSpecific { get; set; }

        [DataMember(Order = 17)]
        public BondSpecific BondSpecific { get; set; }

        [DataMember(Order = 18)]
        public BlueGrassChannel[] BlueGrassChannels { get; set; }

        [DataMember(Order = 19)]
        public TimeZoneInfo TimeZoneInfo { get; set; }

        [DataMember(Order = 20)]
        public DebugInfo DebugInfo { get; set; }

        [DataMember(Order = 21)]
        public CurrencySpecific CurrencySpecific { get; set; }

        [DataMember(Order = 22)]
        public DatafeedSpecificCurrency DatafeedSpecificCurrency { get; set; }

        [DataMember(Order = 23)]
        public Meta Meta { get; set; }

        [DataMember(Order = 24)]
        public RoncoSpecific RoncoSpecific { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "Trading", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class Trading : ExtensibleDataObjectBase
    {
        [DataMember]
        public Trade Last { get; set; }

        [DataMember]
        public Currency Open { get; set; }

        [DataMember(Order = 2)]
        public Currency High { get; set; }

        [DataMember(Order = 3)]
        public Currency Low { get; set; }

        [DataMember(Order = 4)]
        public Currency NetChange { get; set; }

        [DataMember(Order = 5)]
        public decimal? Volume { get; set; }

        [DataMember(Order = 6)]
        public XmlQualifiedName TradeCondition { get; set; }

        [DataMember(Order = 7)]
        public decimal? ChangePercent { get; set; }

        [DataMember(Order = 8)]
        public bool? IsRealtime { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "Trade", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class Trade : ExtensibleDataObjectBase
    {
        [DataMember]
        public Currency Price { get; set; }

        [DataMember]
        public decimal? Size { get; set; }

        [DataMember]
        public DateTime? Time { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "AfterHoursTrading", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class AfterHoursTrading : ExtensibleDataObjectBase
    {
        [DataMember]
        public Currency Price { get; set; }

        [DataMember]
        public decimal? Volume { get; set; }

        [DataMember(Order = 2)]
        public DateTime? Time { get; set; }

        [DataMember(Order = 3)]
        public Currency NetChange { get; set; }

        [DataMember(Order = 4)]
        public decimal? ChangePercent { get; set; }

        [DataMember(Order = 5)]
        public Currency Open { get; set; }

        [DataMember(Order = 6)]
        public Currency High { get; set; }

        [DataMember(Order = 7)]
        public Currency Low { get; set; }

        [DataMember(Order = 8)]
        public double TradeCount { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "Offers", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class Offers : ExtensibleDataObjectBase
    {
        [DataMember]
        public Trade Bid { get; set; }

        [DataMember(Order = 1)]
        public Trade Ask { get; set; }

        [DataMember(Order = 2)]
        public bool? BidUp { get; set; }

        [DataMember(Order = 3)]
        public Trade LastTrade { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "TradingStatistics", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class TradingStatistics : ExtensibleDataObjectBase
    {
        [DataMember]
        public CurrencyStatistic[] PriceStatistics { get; set; }

        [DataMember]
        public DecimalStatistic[] VolumeStatistics { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "CurrencyStatistic", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class CurrencyStatistic : ExtensibleDataObjectBase
    {
        [DataMember]
        public CalendarRange Range { get; set; }

        [DataMember(Order = 1)]
        public Currency Average { get; set; }

        [DataMember(Order = 2)]
        public Currency Minimum { get; set; }

        [DataMember(Order = 3)]
        public Currency Maximum { get; set; }

        [DataMember(Order = 4)]
        public DateTime? MinimumDateTime { get; set; }

        [DataMember(Order = 5)]
        public DateTime? MaximumDateTime { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "DecimalStatistic", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class DecimalStatistic : ExtensibleDataObjectBase
    {
        [DataMember]
        public CalendarRange Range { get; set; }

        [DataMember(Order = 1)]
        public decimal? Average { get; set; }

        [DataMember(Order = 2)]
        public decimal? Minimum { get; set; }

        [DataMember(Order = 3)]
        public decimal? Maximum { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "Financials", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class Financials : ExtensibleDataObjectBase
    {
        [DataMember]
        public decimal? OpenInterest { get; set; }

        [DataMember(Order = 1)]
        public Currency LastDividendPerShare { get; set; }

        [DataMember(Order = 2)]
        public decimal? SharesOutstanding { get; set; }

        [DataMember(Order = 3)]
        public decimal? Yield { get; set; }

        [DataMember(Order = 4)]
        public Currency LastEarningsPerShare { get; set; }

        [DataMember(Order = 5)]
        public decimal? PriceToEarningsRatio { get; set; }

        [DataMember(Order = 6)]
        public Currency MarketCapitalization { get; set; }

        [DataMember(Order = 7)]
        public Trade Previous { get; set; }

        [DataMember(Order = 8)]
        public DateTime? LastDividendExDateTime { get; set; }

        [DataMember(Order = 9)]
        public decimal? ShortInterest { get; set; }

        [DataMember(Order = 10)]
        public decimal? DollarsTraded { get; set; }

        [DataMember(Order = 11)]
        public decimal? ShortInterestNetChange { get; set; }

        [DataMember(Order = 12)]
        public decimal? ShortInterestChangePercent { get; set; }

        [DataMember(Order = 13)]
        public decimal? DaysToCover { get; set; }

        [DataMember(Order = 14)]
        public decimal? PercentOfFloatShorted { get; set; }

        [DataMember(Order = 15)]
        public decimal? PercentOfSharesOutstandingShorted { get; set; }

        [DataMember(Order = 16)]
        public decimal? PublicFloat { get; set; }

        [DataMember(Order = 17)]
        public decimal? MoneyFlow { get; set; }

        [DataMember(Order = 18)]
        public DateTime? LastSplitDate { get; set; }

        [DataMember(Order = 19)]
        public decimal? LastSplitFactor { get; set; }

        [DataMember(Order = 20)]
        public bool? AnnualReportAvailable { get; set; }

        [DataMember(Order = 21)]
        public DateTime? LastDividendPayDate { get; set; }

        [DataMember(Order = 22)]
        public DateTime? LastShortInterestDate { get; set; }

        [DataMember(Order = 23)]
        public decimal? MoneyFlowRatio { get; set; }

        [DataMember(Order = 24)]
        public decimal? PreviousVolume { get; set; }

        [DataMember(Order = 25)]
        public bool? SpecialDividend { get; set; }

        [DataMember(Order = 26)]
        public decimal? AnnualizedDividend { get; set; }

        [DataMember(Order = 27)]
        public decimal? EstimatedPriceToEarningsRatioCurrentYear { get; set; }

        [DataMember(Order = 28)]
        public decimal? EstimatedPriceToEarningsRatioNextYear { get; set; }

        [DataMember(Order = 29)]
        public decimal? SalesPerEmployee { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "BeforeHoursTrading", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class BeforeHoursTrading : ExtensibleDataObjectBase
    {
        [DataMember]
        public Currency Price { get; set; }

        [DataMember]
        public decimal? Volume { get; set; }

        [DataMember(Order = 2)]
        public DateTime? Time { get; set; }

        [DataMember(Order = 3)]
        public Currency NetChange { get; set; }

        [DataMember(Order = 4)]
        public decimal? ChangePercent { get; set; }

        [DataMember(Order = 5)]
        public Currency Open { get; set; }

        [DataMember(Order = 6)]
        public Currency High { get; set; }

        [DataMember(Order = 7)]
        public Currency Low { get; set; }

        [DataMember(Order = 8)]
        public double TradeCount { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "FinancialStatusIndicator", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class FinancialStatusIndicator : ExtensibleDataObjectBase
    {
        [DataMember]
        public XmlQualifiedName[] FinancialStatuses { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "IndustryClassification", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class IndustryClassification : ExtensibleDataObjectBase
    {
        [DataMember]
        public Industry Industry { get; set; }

        [DataMember]
        public Industry Sector { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "FutureSpecific", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class FutureSpecific : ExtensibleDataObjectBase
    {
        [DataMember]
        public decimal? SettlementPrice { get; set; }

        [DataMember(Order = 1)]
        public DateTime? SettlementDate { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "BondSpecific", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class BondSpecific : ExtensibleDataObjectBase
    {
        [DataMember]
        public Currency TradePrice { get; set; }

        [DataMember(Order = 1)]
        public Currency TradeNetChange { get; set; }

        [DataMember(Order = 2)]
        public decimal? TradeChangePercent { get; set; }

        [DataMember(Order = 3)]
        public decimal? Yield { get; set; }

        [DataMember(Order = 4)]
        public decimal? YieldNetChange { get; set; }

        [DataMember(Order = 5)]
        public decimal? YieldChangePercent { get; set; }

        [DataMember(Order = 6)]
        public decimal? WeekToDateNetChange { get; set; }

        [DataMember(Order = 7)]
        public decimal? WeekToDateNetChangePercent { get; set; }

        [DataMember(Order = 8)]
        public decimal? FiftyTwoWeekNetChange { get; set; }

        [DataMember(Order = 9)]
        public decimal? FiftyTwoWeekNetChangePercent { get; set; }

        [DataMember(Order = 10)]
        public decimal? YearToDateNetChange { get; set; }

        [DataMember(Order = 11)]
        public decimal? YearToDateChangePercent { get; set; }

        [DataMember(Order = 12)]
        public Trade PreviousWeekClose { get; set; }

        [DataMember(Order = 13)]
        public Currency TradeOpenPrice { get; set; }

        [DataMember(Order = 14)]
        public Currency TradeHighPrice { get; set; }

        [DataMember(Order = 15)]
        public Currency TradeLowPrice { get; set; }

        [DataMember(Order = 16)]
        public Currency ClosingYieldHigh { get; set; }

        [DataMember(Order = 17)]
        public Currency ClosingYieldLow { get; set; }

        [DataMember(Order = 18)]
        public Currency ClosingYieldOpen { get; set; }

        [DataMember(Order = 19)]
        public Currency ClosingYield { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "TimeZoneInfo", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class TimeZoneInfo : ExtensibleDataObjectBase
    {
        [DataMember]
        public string TimeZone { get; set; }

        [DataMember]
        public int UtcOffsetHours { get; set; }

        [DataMember(Order = 2)]
        public string DisplayName { get; set; }

        [DataMember(Order = 3)]
        public int UtcOffsetMinutes { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "DebugInfo", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class DebugInfo : ExtensibleDataObjectBase
    {
        [DataMember]
        public string[] DatasourceInfos { get; set; }

        [DataMember]
        public bool ResolvedFromCache { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "CurrencySpecific", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class CurrencySpecific : ExtensibleDataObjectBase
    {
        [DataMember]
        public decimal? WeekToDateNetChange { get; set; }

        [DataMember]
        public decimal? WeekToDateNetChangePercent { get; set; }

        [DataMember(Order = 2)]
        public decimal? FiftyTwoWeekNetChange { get; set; }

        [DataMember(Order = 3)]
        public decimal? FiftyTwoWeekNetChangePercent { get; set; }

        [DataMember(Order = 4)]
        public decimal? YearToDateNetChange { get; set; }

        [DataMember(Order = 5)]
        public decimal? YearToDateChangePercent { get; set; }

        [DataMember(Order = 6)]
        public Trade PreviousWeekClose { get; set; }

        [DataMember(Order = 7)]
        public Trade PreviousCloseBid { get; set; }

        [DataMember(Order = 8)]
        public Trade PreviousCloseAsk { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "DatafeedSpecificCurrency", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class DatafeedSpecificCurrency : ExtensibleDataObjectBase
    {
        [DataMember]
        public Currency ClosingHigh { get; set; }

        [DataMember]
        public Currency ClosingLow { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "Meta", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class Meta : ExtensibleDataObjectBase
    {
        [DataMember]
        public string MarketState { get; set; }

        [DataMember]
        public string RealtimeProvider { get; set; }

        [DataMember(Order = 2)]
        public string DataProvider { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "RoncoSpecific", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class RoncoSpecific : ExtensibleDataObjectBase
    {
        [DataMember]
        public long? AdvancingBlockCount { get; set; }

        [DataMember]
        public long? BlockCount { get; set; }

        [DataMember]
        public long? BlockMoneyflowDown { get; set; }

        [DataMember]
        public long? BlockMoneyflowUnchanged { get; set; }

        [DataMember]
        public long? BlockMoneyflowUp { get; set; }

        [DataMember]
        public long? DecliningBlockCount { get; set; }

        [DataMember]
        public long? IrregularBlockCount { get; set; }

        [DataMember]
        public long? NormalMoneyflowDown { get; set; }

        [DataMember]
        public long? NormalMoneyflowUnchanged { get; set; }

        [DataMember]
        public long? NormalMoneyflowUp { get; set; }

        [DataMember]
        public double? TickChange { get; set; }

        [DataMember]
        public long? UnchangedBlockCount { get; set; }

        [DataMember(Order = 12)]
        public long? DollarVolume { get; set; }

        [DataMember(Order = 13)]
        public long? TradeVolume { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetEntityRequest", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetEntityRequest : ExtensibleDataObjectBase
    {
        [DataMember]
        public EntityRequest[] EntityRequests { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "EntityRequest", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class EntityRequest : ExtensibleDataObjectBase
    {
        [DataMember]
        public string RequestId { get; set; }

        [DataMember(Order = 1)]
        public Instrument Instrument { get; set; }

        [DataMember(Order = 2)]
        public DateTime? EffectiveDate { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetEntityResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetEntityResponse : ExtensibleDataObjectBase
    {
        [DataMember]
        public EntityResponse[] EntityResponses { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "EntityResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class EntityResponse : ExtensibleDataObjectBase
    {
        [DataMember]
        public string RequestId { get; set; }

        [DataMember(Order = 1)]
        public EntityMatch[] Matches { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "EntityMatch", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class EntityMatch : ExtensibleDataObjectBase
    {
        [DataMember]
        public int? MatchedInstrumentIndex { get; set; }

        [DataMember(Order = 1)]
        public Entity Entity { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "Entity", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class Entity : ExtensibleDataObjectBase
    {
        [DataMember]
        public XmlQualifiedName EntityStatus { get; set; }

        [DataMember(Order = 1)]
        public int? CurrentInstrumentIndex { get; set; }

        [DataMember(Order = 2)]
        public EntityInstrument[] EntityInstruments { get; set; }

        [DataMember(Order = 3)]
        public EntityInstrumentLink[] EntityInstrumentLinks { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "EntityInstrument", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class EntityInstrument : ExtensibleDataObjectBase
    {
        [DataMember]
        public Instrument Instrument { get; set; }

        [DataMember]
        public DateTime? StartDate { get; set; }

        [DataMember(Order = 2)]
        public DateTime? EndDate { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "EntityInstrumentLink", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class EntityInstrumentLink : ExtensibleDataObjectBase
    {
        [DataMember]
        public int? PreviousInstrumentIndex { get; set; }

        [DataMember(Order = 1)]
        public int? NextEntityInstrumentIndex { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetExchangeSummaryRequest", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetExchangeSummaryRequest : ExtensibleDataObjectBase
    {
        [DataMember]
        public ExchangeSummaryRequest[] ExchangeSummaryRequests { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ExchangeSummaryRequest", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class ExchangeSummaryRequest : ExtensibleDataObjectBase
    {
        [DataMember]
        public string RequestId { get; set; }

        [DataMember(Order = 1)]
        public Market Market { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetExchangeSummaryResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetExchangeSummaryResponse : ExtensibleDataObjectBase
    {
        [DataMember]
        public ExchangeSummaryResponse[] ExchangeSummaryResponses { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ExchangeSummaryResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class ExchangeSummaryResponse : ExtensibleDataObjectBase
    {
        [DataMember]
        public string RequestId { get; set; }

        [DataMember(Order = 1)]
        public ExchangeSummary ExchangeSummary { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ExchangeSummary", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class ExchangeSummary : ExtensibleDataObjectBase
    {
        [DataMember]
        public int? AdvancingCount { get; set; }

        [DataMember]
        public int? DecliningCount { get; set; }

        [DataMember]
        public int? UnchangedCount { get; set; }

        [DataMember(Order = 3)]
        public decimal? AdvancingVolume { get; set; }

        [DataMember(Order = 4)]
        public decimal? DecliningVolume { get; set; }

        [DataMember(Order = 5)]
        public decimal? UnchangedVolume { get; set; }

        [DataMember(Order = 6)]
        public decimal? New52WeekHighCount { get; set; }

        [DataMember(Order = 7)]
        public decimal? New52WeekLowCount { get; set; }

        [DataMember(Order = 8)]
        public decimal? IssueCount { get; set; }

        [DataMember(Order = 9)]
        public decimal? TotalVolume { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "InstrumentByDialectRequest", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class InstrumentByDialectRequest : ExtensibleDataObjectBase
    {
        [DataMember]
        public string RequestId { get; set; }

        [DataMember]
        public string Symbol { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "InstrumentByDialectResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class InstrumentByDialectResponse : ExtensibleDataObjectBase
    {
        [DataMember]
        public string RequestId { get; set; }

        [DataMember(Order = 1)]
        public Match[] Matches { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetInstrumentRequest", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetInstrumentRequest : ExtensibleDataObjectBase
    {
        [DataMember]
        public InstrumentRequest[] InstrumentRequests { get; set; }

        [DataMember]
        public XmlQualifiedName[] Needed { get; set; }

        [DataMember(Order = 2)]
        public int? MaxInstrumentMatches { get; set; }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "PhoneticSearchRequest", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class PhoneticSearchRequest : object, IExtensibleDataObject
    {
        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public XmlQualifiedName InstrumentType { get; set; }

        [DataMember(Order = 2)]
        public string CountryCode { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "PhoneticSearchResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class PhoneticSearchResponse : object, IExtensibleDataObject
    {
        [DataMember]
        public PhoneticMatch[] PhoneticMatches { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "PhoneticMatch", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class PhoneticMatch : object, IExtensibleDataObject
    {
        [DataMember]
        public Instrument Instrument { get; set; }

        [DataMember]
        public Instrument UpdatedInstrument { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetOptionChainRequest", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetOptionChainRequest : object, IExtensibleDataObject
    {
        [DataMember]
        public Instrument Instrument { get; set; }

        [DataMember(Order = 1)]
        public string ExpirationMonth { get; set; }

        [DataMember(Order = 2)]
        public string ExpirationYear { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetOptionChainResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetOptionChainResponse : object, IExtensibleDataObject
    {
        [DataMember]
        public Instrument Instrument { get; set; }

        [DataMember]
        public OptionChainItem[] OptionChainItems { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "OptionChainItem", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class OptionChainItem : object, IExtensibleDataObject
    {
        [DataMember]
        public Instrument Instrument { get; set; }

        [DataMember]
        public Trade Last { get; set; }

        [DataMember]
        public Currency NetChange { get; set; }

        [DataMember(Order = 3)]
        public decimal? ChangePercent { get; set; }

        [DataMember(Order = 4)]
        public decimal? Volume { get; set; }

        [DataMember(Order = 5)]
        public Trade Bid { get; set; }

        [DataMember(Order = 6)]
        public Trade Ask { get; set; }

        [DataMember(Order = 7)]
        public decimal? OpenInterest { get; set; }

        [DataMember(Order = 8)]
        public decimal? StrikePrice { get; set; }

        [DataMember(Order = 9)]
        public string ExpirationMonth { get; set; }

        [DataMember(Order = 10)]
        public string ExpirationYear { get; set; }

        [DataMember(Order = 11)]
        public XmlQualifiedName OptionType { get; set; }

        [DataMember(Order = 12)]
        public string ExpirationDay { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetIndexComponentsRequest", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetIndexComponentsRequest : object, IExtensibleDataObject
    {
        [DataMember]
        public Instrument Instrument { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetIndexComponentsResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetIndexComponentsResponse : object, IExtensibleDataObject
    {
        [DataMember]
        public Instrument IndexInstrument { get; set; }

        [DataMember(Order = 1)]
        public IndexComponent[] IndexComponents { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "IndexComponent", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class IndexComponent : object, IExtensibleDataObject
    {
        [DataMember]
        public Instrument Instrument { get; set; }

        [DataMember]
        public Trading Trading { get; set; }

        [DataMember(Order = 2)]
        public Currency MarketCapitalization { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetCurrencyInstrumentsResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetCurrencyInstrumentsResponse : object, IExtensibleDataObject
    {
        [DataMember]
        public CurrencyInstrument[] CurrencyInstruments { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "CurrencyInstrument", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class CurrencyInstrument : object, IExtensibleDataObject
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember(Order = 1)]
        public string CurrencyIsoCode { get; set; }

        [DataMember(Order = 2)]
        public Instrument Instrument { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ConvertCurrencyRequest", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class ConvertCurrencyRequest : object, IExtensibleDataObject
    {
        [DataMember]
        public Instrument SourceCurrencyInstrument { get; set; }

        [DataMember]
        public Instrument TargetCurrencyInstrument { get; set; }

        [DataMember]
        public decimal Value { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ConvertCurrencyResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class ConvertCurrencyResponse : object, IExtensibleDataObject
    {
        [DataMember]
        public decimal Value { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetIndexesRequest", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetIndexesRequest : object, IExtensibleDataObject
    {
        [DataMember]
        public Instrument Instrument { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetIndexesResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetIndexesResponse : object, IExtensibleDataObject
    {
        [DataMember]
        public IndexItem[] IndexItems { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "IndexItem", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class IndexItem : object, IExtensibleDataObject
    {
        [DataMember]
        public Instrument Instrument { get; set; }

        [DataMember]
        public Trading Trading { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetOptionMonthsRequest", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetOptionMonthsRequest : object, IExtensibleDataObject
    {
        [DataMember]
        public Instrument Instrument { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetOptionMonthsResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetOptionMonthsResponse : object, IExtensibleDataObject
    {
        [DataMember]
        public OptionMonthYear[] OptionMonthYears { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "OptionMonthYear", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class OptionMonthYear : object, IExtensibleDataObject
    {
        [DataMember]
        public string ExpirationMonth { get; set; }

        [DataMember]
        public string ExpirationYear { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetFutureInstrumentsResponse", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetFutureInstrumentsResponse : object, IExtensibleDataObject
    {
        [DataMember]
        public FutureInstrument[] FutureInstruments { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "FutureInstrument", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class FutureInstrument : object, IExtensibleDataObject
    {
        [DataMember]
        public string FriendlyName { get; set; }

        [DataMember]
        public Instrument Instrument { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetInstrumentFault", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetInstrumentFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "PayloadToLargeFault", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class PayloadToLargeFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetEntityFault", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetEntityFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetExchangeSummaryFault", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetExchangeSummaryFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "PhoneticSearchFault", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class PhoneticSearchFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetOptionChainFault", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetOptionChainFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetIndexComponentsFault", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetIndexComponentsFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetCurrencyInstrumentsFault", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetCurrencyInstrumentsFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ConvertCurrencyFault", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class ConvertCurrencyFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetInstrumentByDialectFault", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetInstrumentByDialectFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetIndexesFault", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetIndexesFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetOptionMonthsFault", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetOptionMonthsFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "GetFutureInstrumentsFault", Namespace = "http://service.marketwatch.com/ws/2007/10/financialdata")]
    public class GetFutureInstrumentsFault : object, IExtensibleDataObject
    {
        [DataMember]
        public string Message { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }
}

namespace DowJones.MarketWatch.Dylan.Core.Financialdata.Types
{
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "BlueGrassChannel", Namespace = "http://schemas.datacontract.org/2004/07/Dylan2011.Protocol.FinancialData.Types")]
    public class BlueGrassChannel : ExtensibleDataObjectBase
    {
        [DataMember]
        public XmlQualifiedName BlueGrassChannelType { get; set; }

        [DataMember]
        public string Channel { get; set; }
    }
}
