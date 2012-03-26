using DowJones.Managers.Core;

namespace DowJones.Managers.Charting.MarketData
{
    public class MarketChartDataPackage : IPackage
    {
        public string Symbol { get; set; }

        public string Name { get; set; }
        
        public bool IsIndex { get; set; }

        public int BarSize { get; set; }
        
        public Thunderball.Library.Charting.Session Session { get; set; }
    }
}