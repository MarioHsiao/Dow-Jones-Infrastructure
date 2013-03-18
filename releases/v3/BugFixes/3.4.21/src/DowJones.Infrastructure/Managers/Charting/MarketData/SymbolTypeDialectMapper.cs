// -----------------------------------------------------------------------
// <copyright file="SymbolTypeDialectMapper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using DowJones.Managers.MarketWatch.Instrument;
using DowJones.Mapping;

namespace DowJones.Managers.Charting.MarketData
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SymbolTypeDialectMapper : TypeMapper<SymbolType, SymbolDialectType>
    {
        public override SymbolDialectType Map(SymbolType source)
        {
            switch (source)
            {
                case SymbolType.Sedol:
                    return SymbolDialectType.Sedol;
                case SymbolType.Ticker:
                    return SymbolDialectType.Ticker;
                case SymbolType.Cusip:
                    return SymbolDialectType.Cusip;
                case SymbolType.Isin:
                    return SymbolDialectType.Isin;
                case SymbolType.FCode:
                default:
                    return SymbolDialectType.Factiva;
            }
        }
    }
}