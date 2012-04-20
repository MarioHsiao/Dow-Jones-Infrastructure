using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using DowJones.Exceptions;
using DowJones.Managers.Core;
using DowJones.Managers.MarketWatch.Instrument;
using DowJones.MarketWatch.Dylan.Core.Financialdata;
using DowJones.Properties;

namespace DowJones.Managers.Charting.MarketData
{
    [DataContract(Namespace = "")]
    public class MarketChartDataServiceResult : AbstractServiceResult<MarketChartDataServicePartResult<MarketChartDataPackage>, MarketChartDataPackage>, IRequest
    {
        private static readonly BasicHttpBinding BasicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
        private static readonly EndpointAddress ThunderballEndpointAddress = new EndpointAddress(Settings.Default.ThunderBallEndpointAddress);

        /// <summary>
        /// Gets the chart.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="symbolType">Type of the symbol.</param>
        /// <param name="timePeriod">The time period.</param>
        /// <param name="frequency">The frequency.</param>
        /// <returns>
        /// A dictionary of chart data responses
        /// </returns>
        private MarketChartDataServicePartResult<MarketChartDataPackage> ProcessChart(string identifier, string symbol, SymbolType symbolType, TimePeriod timePeriod = TimePeriod.OneDay, Frequency frequency = Frequency.FifteenMinutes)
        {
            var partResult = new MarketChartDataServicePartResult<MarketChartDataPackage>
                                 {
                                     Identifier = identifier
                                 };

            Match tempMatch = null;
            var tempSymbol = symbol;
            var requestId = string.Empty;

            var instrumentManger = new InstrumentManager();
            var instrumentResponse = instrumentManger.GetInstruments(new[] { symbol }, Mapper.Map<SymbolDialectType>(symbolType));

            if (instrumentResponse != null &&
                instrumentResponse.Count >= 1 &&
                instrumentResponse[0].Matches != null &&
                instrumentResponse[0].Matches.Count() >= 1)
            {
                var tempResponse = instrumentResponse[0];
                tempMatch = tempResponse.Matches[0];
                requestId = tempResponse.RequestId;

                // update the symbol.
                tempSymbol = string.Concat(tempMatch.Instrument.Exchange.CountryCode.ToLowerInvariant(), ":", tempMatch.Instrument.Ticker);
            }
            else
            {
                partResult.ReturnCode = DowJonesUtilitiesException.Dylan_EmptyMatchResponse;
                partResult.StatusMessage = Resources.GetErrorMessage(partResult.ReturnCode.ToString());
            }

            // Continue if Return code is still 0
            if (partResult.ReturnCode == 0)
            {
                ProcessServicePartResult<MarketChartDataPackage>(
                    MethodBase.GetCurrentMethod(),
                    partResult,
                    () =>
                        {
                            var chartRequests = new[]
                                                    {
                                                        new Thunderball.Library.GetChartRequest
                                                            {
                                                                Time = MarketDataChartingManager.GetXmlEnumName<TimePeriod>(timePeriod.ToString()),
                                                                Freq = MarketDataChartingManager.GetXmlEnumName<Frequency>(frequency.ToString()),
                                                                Symbol = new[] {tempSymbol},
                                                                EntitlementToken = Settings.Default.ThunderBallEntitlementToken,
                                                            }
                                                    };

                            var chartServiceClient = new ChartServiceClient(BasicHttpBinding, ThunderballEndpointAddress);
                            var response = chartServiceClient.GetChart(chartRequests);
                            if (response.Count > 0)
                            {
                                var source = response.Values.FirstOrDefault();
                                partResult.Package = (from kvp in source.Data let session = kvp.Value.Sessions[0] select GetPackage(kvp.Key, kvp.Value.Name, Convert.ToBoolean(kvp.Value.IsIndex), source.BarSize, session, requestId, tempMatch)).FirstOrDefault();
                            }
                            else
                            {
                                partResult.ReturnCode = DowJonesUtilitiesException.ThunderballService_EmptyResponse;
                                partResult.StatusMessage = Resources.GetErrorMessage(partResult.ReturnCode.ToString());
                            }
                        });
            }
            return partResult;
        }

        internal MarketChartDataPackage GetPackage(string symbol, string name, bool isIndex, int barSize, Thunderball.Library.Charting.Session session, string requestId, Match match)
        {
            var package = new MarketChartDataPackage
                              {
                                  Symbol = symbol, 
                                  Name = name, 
                                  RequestId = requestId,
                                  IsIndex = isIndex,
                                  BarSize = barSize,
                                  Session = session,
                                  Match = match,
                              };
            return package;
        }

       
        internal void Populate(MarketDataServiceRequest request)
        {
            ProcessServiceResult(
                MethodBase.GetCurrentMethod(),
                () =>
                    {
                        // page and module ids are not required
                        if (request == null)
                        {
                            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidGetRequest);
                        }
                  
                        GetData(request);
                    });
        }

        private void GetData(MarketDataServiceRequest request)
        {
            PartResults = GetParts(request.Symbols, request.SymbolType, request.TimePeriod, request.Frequency);
        }

        private IEnumerable<MarketChartDataServicePartResult<MarketChartDataPackage>> GetParts(IEnumerable<string> symbols, SymbolType symbolType, TimePeriod timePeriod = TimePeriod.OneDay, Frequency frequency = Frequency.FifteenMinutes)
        {
            var index = 0;
            if (symbols.Count() == 1)
            {
                return new List<MarketChartDataServicePartResult<MarketChartDataPackage>> { ProcessChart(index.ToString() , symbols.First(), symbolType, timePeriod, frequency) };
            }

            var tasks = (from symbol in symbols
                         let identifier = Interlocked.Increment(ref index).ToString()
                         select TaskFactory.StartNew(
                             () => ProcessChart(identifier, symbol, symbolType, timePeriod, frequency), TaskCreationOptions.None)).ToList();

            Task.WaitAll(tasks.ToArray());
            var orderedTasks = tasks.OrderBy(task => Int32.Parse(task.Result.Identifier));
            return orderedTasks.Select(task => task.Result).ToList();
        }
    }
}