using System.Runtime.Serialization;
using DowJones.Managers.Core;
using DowJones.MarketWatch.Dylan.Core.Financialdata;
using ThunderSession = DowJones.Thunderball.Library.Charting.Session;

namespace DowJones.Managers.Charting.MarketData
{
    [DataContract(Namespace = "")]
    public class MarketChartDataPackage : IPackage
    {
        [DataMember(Name = "symbol")]
        public string Symbol { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "isIndex")]
        public bool IsIndex { get; set; }

        [DataMember(Name = "barSize")]
        public int BarSize { get; set; }

        [DataMember(Name = "session")]
        public ThunderSession Session { get; set; }

        [DataMember(Name = "match")]
        public Match Match { get; set; }
    }
}