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
        [DataMember(Name = "rc")]
        public long ReturnCode { get; set; }

        [DataMember(Name = "statusMessage")]
        public string StatusMessage { get; set; }

        [DataMember(Name = "requestedId")]
        public string RequestedId { get; set; }

        [DataMember(Name = "symbol")]
        public string Symbol { get; set; }

        [DataMember(Name = "fCode")]
        public string FCode { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "isin")]
        public string Isin { get; set; }

        [DataMember(Name = "sedol")]
        public string Sedol { get; set; }

        [DataMember(Name = "cusip")]
        public string Cusip { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "low")]
        public DoubleNumberStock Low { get; set; }

        [DataMember(Name = "high")]
        public DoubleNumberStock High { get; set; }

        [DataMember(Name = "previousClose")]
        public DoubleNumberStock PreviousClose { get; set; }

        [DataMember(Name = "last")]
        public DoubleNumberStock Last { get; set; }

        [DataMember(Name = "open")]
        public DoubleNumberStock Open { get; set; }

        [DataMember(Name = "dataPoints")]
        public BasicDataPointCollection DataPoints { get; set; }

        [DataMember(Name = "end")]
        public DateTime End { get; set; }

        [DataMember(Name = "start")]
        public DateTime Start { get; set; }

        [DataMember(Name = "startDescripter")]
        public string StartDescripter { get; set; }

        [DataMember(Name = "stop")]
        public DateTime Stop { get; set; }

        [DataMember(Name = "stopDescripter")]
        public string StopDescripter { get; set; }

        [DataMember(Name = "adjustedStart")]
        public DateTime AdjustedStart { get; set; }

        [DataMember(Name = "adjustedStartDescripter")]
        public string AdjustedStartDescripter { get; set; }

        [DataMember(Name = "adjustedStop")]
        public DateTime AdjustedStop { get; set; }

        [DataMember(Name = "adjustedStopDescripter")]
        public string AdjustedStopDescripter { get; set; }

        [DataMember(Name = "lastUpdated")]
        public DateTime LastUpdated { get; set; }

        [DataMember(Name = "lastUpdatedDescripter")]
        public string LastUpdatedDescripter { get; set; }

        [DataMember(Name = "adjustedLastUpdated")]
        public DateTime AdjustedLastUpdated { get; set; }

        [DataMember(Name = "adjustedLastUpdatedDescripter")]
        public string AdjustedLastUpdatedDescripter { get; set; }

        [DataMember(Name = "percentChange")]
        public PercentStock PercentChange { get; set; }

        [DataMember(Name = "isIndex")]
        public bool IsIndex { get; set; }

        [DataMember(Name = "provider")]
        public Provider Provider { get; set; }
    }
}

