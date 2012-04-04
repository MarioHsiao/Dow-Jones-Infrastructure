using System.Collections.Generic;
using DowJones.Managers.Core;

namespace DowJones.Managers.Charting.MarketData
{
    public class MarketDataServiceRequest : IRequest
    {
        public IEnumerable<string> Symbols;

        public SymbolType SymbolType = SymbolType.FCode;

        public TimePeriod TimePeriod = TimePeriod.OneDay;

        public Frequency Frequency = Frequency.FifteenMinutes;
    }
}