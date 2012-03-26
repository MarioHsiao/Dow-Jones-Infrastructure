using System;
using System.Runtime.Serialization;

namespace DowJones.Pages.Company
{
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
        public DataPointFrequency? Frequency { get; set; }

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