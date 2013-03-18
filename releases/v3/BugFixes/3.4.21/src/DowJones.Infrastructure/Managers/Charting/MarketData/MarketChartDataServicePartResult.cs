using System.Runtime.Serialization;
using DowJones.Managers.Core;

namespace DowJones.Managers.Charting.MarketData
{
    [DataContract(Name = "marktetDataServicePartResult", Namespace = "")]
    public class MarketChartDataServicePartResult<TPackage> : AbstractServicePartResult<TPackage> 
        where TPackage : MarketChartDataPackage
    {
    }
}