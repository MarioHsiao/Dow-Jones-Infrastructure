// -----------------------------------------------------------------------
// <copyright file="SymbologyManager.cs" company="">
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using DowJones.Managers.MarketWatch.Core;
using DowJones.Properties;
using InstrumentType =  DowJones.MarketWatch.Dylan.Core.Symbology.Instrument;

namespace DowJones.Managers.MarketWatch.Symbology
{
    /// <summary>
    /// </summary>
    public class SymbologyManager : AbstractMarketWatchService<ISymbology>
    {
        private static readonly BasicHttpBinding BasicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
        private static readonly EndpointAddress EndpointAddress = new EndpointAddress(Settings.Default.DylanSymbologyEndpointAddress);
        private const string Dialect = "http://system.dowjones.com/dowjones/fcode";

        /// <summary>
        /// Gets the instrument.
        /// </summary>
        /// <param name="fcodes">The fcodes.</param>
        /// <returns></returns>
        public List<InstrumentType> GetInstruments(IEnumerable<string> fcodes)
        {
            var client = new SymbologyClient(BasicHttpBinding, EndpointAddress);
            var request = new NormalizeRequest
                              {
                                  Dialect = Dialect,
                                  Symbols = fcodes.ToArray(),
                              };

            NormalizeResponse normalized = null;

            AddEntitlementToken(client, 
                ()=>
                    {
                        normalized = client.Normalize(request);
                    });

            if (normalized != null
                && (normalized.NormalizeResults != null)
                && (normalized.NormalizeResults.Length > 0))
            {
                return normalized.NormalizeResults.Select(normalizeResult => normalizeResult.Instrument).ToList();
            }
            return new List<InstrumentType>();
        }
    }
}