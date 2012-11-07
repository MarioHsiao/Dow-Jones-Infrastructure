using System.Runtime.Serialization;
using DowJones.Models.Charting.MarketData;
//using DowJones.Pages.Company;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Summary.Packages
{
    [DataContract(Name = "summaryChartPackage", Namespace = "")]
    public class SummaryChartPackage : AbstractSummaryPackage
    {
        //[DataMember(Name = "marketIndexIntradayResult")]
        //public MarketIndexIntradayResult MarketIndexIntradayResult { get; protected internal set; }

        [DataMember(Name = "marketDataInstrumentIntradayResult")]
        public MarketDataInstrumentIntradayResult MarketDataInstrumentIntradayResult { get; protected internal set; }
    }
}