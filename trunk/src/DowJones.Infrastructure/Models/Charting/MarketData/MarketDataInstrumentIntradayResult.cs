using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Formatters;
using DowJones.Models.Company;

namespace DowJones.Models.Charting.MarketData
{
    [CollectionDataContract(Name = "marketDataInstrumentIntradayResultSet", ItemName = "marketDataInstrumentIntradayResult", Namespace = "")]
    public class MarketDataInstrumentIntradayResultSet : List<MarketDataInstrumentIntradayResult>
    {       
    }

    [DataContract(Name = "marketDataInstrumentIntradayResult", Namespace = "" )]
    public class MarketDataInstrumentIntradayResult
    {
        /// <summary>
        /// Gets or sets the return code.
        /// </summary>
        /// <value>
        /// The return code.
        /// </value>
        [DataMember(Name = "rc")]
        public long ReturnCode { get; set; }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        /// <value>
        /// The status message.
        /// </value>
        [DataMember(Name = "statusMessage")]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the requested id.
        /// </summary>
        /// <value>
        /// The requested id.
        /// </value>
        [DataMember(Name = "requestedId")]
        public string RequestedId { get; set; }

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        [DataMember(Name = "symbol")]
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the F code.
        /// </summary>
        /// <value>
        /// The F code.
        /// </value>
        [DataMember(Name = "fCode")]
        public string FCode { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the isin.
        /// </summary>
        /// <value>
        /// The isin.
        /// </value>
        [DataMember(Name = "isin")]
        public string Isin { get; set; }

        /// <summary>
        /// Gets or sets the sedol.
        /// </summary>
        /// <value>
        /// The sedol.
        /// </value>
        [DataMember(Name = "sedol")]
        public string Sedol { get; set; }

        /// <summary>
        /// Gets or sets the cusip.
        /// </summary>
        /// <value>
        /// The cusip.
        /// </value>
        [DataMember(Name = "cusip")]
        public string Cusip { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the low.
        /// </summary>
        /// <value>
        /// The low.
        /// </value>
        [DataMember(Name = "low")]
        public DoubleNumberStock Low { get; set; }

        /// <summary>
        /// Gets or sets the high.
        /// </summary>
        /// <value>
        /// The high.
        /// </value>
        [DataMember(Name = "high")]
        public DoubleNumberStock High { get; set; }

        /// <summary>
        /// Gets or sets the previous close.
        /// </summary>
        /// <value>
        /// The previous close.
        /// </value>
        [DataMember(Name = "previousClose")]
        public DoubleNumberStock PreviousClose { get; set; }

        /// <summary>
        /// Gets or sets the last.
        /// </summary>
        /// <value>
        /// The last.
        /// </value>
        [DataMember(Name = "last")]
        public DoubleNumberStock Last { get; set; }

        /// <summary>
        /// Gets or sets the open.
        /// </summary>
        /// <value>
        /// The open.
        /// </value>
        [DataMember(Name = "open")]
        public DoubleNumberStock Open { get; set; }

        /// <summary>
        /// Gets or sets the data points.
        /// </summary>
        /// <value>
        /// The data points.
        /// </value>
        [DataMember(Name = "dataPoints")]
        public BasicDataPointCollection DataPoints { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        [DataMember(Name = "end")]
        public DateTime? End { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        [DataMember(Name = "start")]
        public DateTime? Start { get; set; }

        /// <summary>
        /// Gets or sets the start descripter.
        /// </summary>
        /// <value>
        /// The start descripter.
        /// </value>
        [DataMember(Name = "startDescripter")]
        public string StartDescripter { get; set; }

        /// <summary>
        /// Gets or sets the stop.
        /// </summary>
        /// <value>
        /// The stop.
        /// </value>
        [DataMember(Name = "stop")]
        public DateTime Stop { get; set; }

        /// <summary>
        /// Gets or sets the stop descripter.
        /// </summary>
        /// <value>
        /// The stop descripter.
        /// </value>
        [DataMember(Name = "stopDescripter")]
        public string StopDescripter { get; set; }

        /// <summary>
        /// Gets or sets the adjusted start.
        /// </summary>
        /// <value>
        /// The adjusted start.
        /// </value>
        [DataMember(Name = "adjustedStart")]
        public DateTime AdjustedStart { get; set; }

        /// <summary>
        /// Gets or sets the adjusted start descripter.
        /// </summary>
        /// <value>
        /// The adjusted start descripter.
        /// </value>
        [DataMember(Name = "adjustedStartDescripter")]
        public string AdjustedStartDescripter { get; set; }

        /// <summary>
        /// Gets or sets the name of the adjusted start updated time zone.
        /// </summary>
        /// <value>
        /// The name of the adjusted start updated time zone.
        /// </value>
        [DataMember(Name = "adjustedStartUpdatedTimeZoneName")]
        public string AdjustedStartUpdatedTimeZoneName { get; set; }

        /// <summary>
        /// Gets or sets the adjusted start updated time zone abbr.
        /// </summary>
        /// <value>
        /// The adjusted start updated time zone abbr.
        /// </value>
        [DataMember(Name = "adjustedStartUpdatedTimeZoneAbbr")]
        public string AdjustedStartUpdatedTimeZoneAbbr { get; set; }

        /// <summary>
        /// Gets or sets the adjusted stop.
        /// </summary>
        /// <value>
        /// The adjusted stop.
        /// </value>
        [DataMember(Name = "adjustedStop")]
        public DateTime AdjustedStop { get; set; }

        /// <summary>
        /// Gets or sets the adjusted stop descripter.
        /// </summary>
        /// <value>
        /// The adjusted stop descripter.
        /// </value>
        [DataMember(Name = "adjustedStopDescripter")]
        public string AdjustedStopDescripter { get; set; }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>
        /// The last updated.
        /// </value>
        [DataMember(Name = "lastUpdated")]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the last updated descripter.
        /// </summary>
        /// <value>
        /// The last updated descripter.
        /// </value>
        [DataMember(Name = "lastUpdatedDescripter")]
        public string LastUpdatedDescripter { get; set; }

        /// <summary>
        /// Gets or sets the adjusted last updated.
        /// </summary>
        /// <value>
        /// The adjusted last updated.
        /// </value>
        [DataMember(Name = "adjustedLastUpdated")]
        public DateTime? AdjustedLastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the name of the adjusted last updated time zone.
        /// </summary>
        /// <value>
        /// The name of the adjusted last updated time zone.
        /// </value>
        [DataMember(Name = "adjustedLastUpdatedTimeZoneName")]
        public string AdjustedLastUpdatedTimeZoneName { get; set; }

        /// <summary>
        /// Gets or sets the adjusted last updated time zone abbr.
        /// </summary>
        /// <value>
        /// The adjusted last updated time zone abbr.
        /// </value>
        [DataMember(Name = "AdjustedLastUpdatedTimeZoneAbbr")]
        public string AdjustedLastUpdatedTimeZoneAbbr { get; set; }

        /// <summary>
        /// Gets or sets the adjusted last updated descripter.
        /// </summary>
        /// <value>
        /// The adjusted last updated descripter.
        /// </value>
        [DataMember(Name = "adjustedLastUpdatedDescripter")]
        public string AdjustedLastUpdatedDescripter { get; set; }

        /// <summary>
        /// Gets or sets the percent change.
        /// </summary>
        /// <value>
        /// The percent change.
        /// </value>
        [DataMember(Name = "percentChange")]
        public PercentStock PercentChange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is index.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is index; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "isIndex")]
        public bool IsIndex { get; set; }

        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        /// <value>
        /// The provider.
        /// </value>
        [DataMember(Name = "provider")]
        public Provider Provider { get; set; }

        /// <summary>
        /// Gets or sets the exchange.
        /// </summary>
        /// <value>
        /// The exchange.
        /// </value>
        [DataMember(Name = "exchange")]
        public Exchange Exchange { get; set; }
    }
}

