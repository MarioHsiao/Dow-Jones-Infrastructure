using System;
using System.Runtime.Serialization;
using DowJones.Formatters;

namespace DowJones.Models.Company
{
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
}