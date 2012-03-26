using System.Runtime.Serialization;
using DowJones.Formatters;

namespace DowJones.Pages.Company
{
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
}