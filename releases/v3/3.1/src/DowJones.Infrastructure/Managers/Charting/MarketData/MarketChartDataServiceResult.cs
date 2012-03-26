using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using DowJones.Exceptions;
using DowJones.Managers.Core;
using DowJones.Properties;

namespace DowJones.Managers.Charting.MarketData
{
    public class MarketChartDataServiceResult : AbstractServiceResult<MarketChartDataServicePartResult<MarketChartDataPackage>, MarketChartDataPackage>, IRequest
    {
        /// <summary>
        /// Gets the chart.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="timePeriod">The time period.</param>
        /// <param name="frequency">The frequency.</param>
        /// <returns>
        /// A dictionary of chart data responses
        /// </returns>
        private MarketChartDataServicePartResult<MarketChartDataPackage> ProcessChart(string identifier, string symbol, TimePeriod timePeriod = TimePeriod.OneDay, Frequency frequency = Frequency.FifteenMinutes)
        {
            var partResult = new MarketChartDataServicePartResult<MarketChartDataPackage>
                                 {
                                     Identifier = identifier
                                 };

            ProcessServicePartResult<MarketChartDataPackage>(
                MethodBase.GetCurrentMethod(),
                partResult,
                () =>
                    {
                        var chartRequests = new[]
                                                {
                                                    new Thunderball.Library.GetChartRequest
                                                        {
                                                            Time = MarketDataManager.GetXmlEnumName<TimePeriod>(timePeriod.ToString()),
                                                            Freq = MarketDataManager.GetXmlEnumName<Frequency>(frequency.ToString()),
                                                            Symbol = new[] {symbol},
                                                            EntitlementToken = Settings.Default.ThunderBallEntitlementToken,
                                                        }
                                                };


                        var basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                        var endpointAddress = new EndpointAddress(Settings.Default.ThunderBallEndpointAddress);
                        var chartServiceClient = new ChartServiceClient(basicHttpBinding, endpointAddress);
                        var response = chartServiceClient.GetChart(chartRequests);
                        if (response.Count > 0)
                        {
                            var source = response.Values.FirstOrDefault();
                            partResult.Package = (from kvp in source.Data let session = kvp.Value.Sessions[0] select GetPackage(kvp.Key, kvp.Value.Name, Convert.ToBoolean(kvp.Value.IsIndex), source.BarSize, session)).FirstOrDefault();
                        }
                        else
                        {
                            partResult.ReturnCode = DowJonesUtilitiesException.ThunderballService_EmptyResponse;
                            partResult.StatusMessage = Resources.GetErrorMessage(partResult.ReturnCode.ToString());

                        }
                    });
            return partResult;
        }

        internal MarketChartDataPackage GetPackage(string symbol, string name, bool isIndex, int barSize, Thunderball.Library.Charting.Session session)
        {
            var package = new MarketChartDataPackage
                              {
                                  Symbol = symbol, 
                                  Name = name, 
                                  IsIndex = isIndex,
                                  BarSize = barSize,
                                  Session = session
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
            PartResults = GetParts(request.Symbols, request.TimePeriod, request.Frequency);
        }

        private IEnumerable<MarketChartDataServicePartResult<MarketChartDataPackage>> GetParts(IEnumerable<string> symbols, TimePeriod timePeriod = TimePeriod.OneDay, Frequency frequency = Frequency.FifteenMinutes)
        {
            var index = 0;
            if (symbols.Count() == 1)
            {
                return new List<MarketChartDataServicePartResult<MarketChartDataPackage>> { ProcessChart(index.ToString() , symbols.First(), timePeriod, frequency) };
            }

            var tasks = (from symbol in symbols
                         let identifier = Interlocked.Increment(ref index).ToString()
                         select TaskFactory.StartNew(
                             () => ProcessChart(identifier, symbol, timePeriod, frequency), TaskCreationOptions.None)).ToList();

            Task.WaitAll(tasks.ToArray());
            var orderedTasks = tasks.OrderBy(task => Int32.Parse(task.Result.Identifier));
            return orderedTasks.Select(task => task.Result).ToList();
        }
    }
}