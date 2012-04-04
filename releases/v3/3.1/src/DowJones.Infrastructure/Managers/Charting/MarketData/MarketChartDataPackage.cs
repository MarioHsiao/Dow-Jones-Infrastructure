using DowJones.Managers.Core;
using DowJones.MarketWatch.Dylan.Core.Financialdata;
using ThunderSession = DowJones.Thunderball.Library.Charting.Session;

namespace DowJones.Managers.Charting.MarketData
{
    public class MarketChartDataPackage : IPackage
    {
        public string Symbol { get; set; }

        public string Name { get; set; }
        
        public bool IsIndex { get; set; }

        public int BarSize { get; set; }

        public ThunderSession Session { get; set; }

        public Match Match { get; set; }
    }
}