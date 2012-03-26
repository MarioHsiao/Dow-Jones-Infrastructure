// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketIndexIntradayResultAssembler.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Thunderball.Library.Charting;
using DowJones.Utilities.Formatters;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Web.Mvc.UI.Models.Company;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.Common
{
    public class MarketIndexIntradayResultAssembler : IAssembler<MarketIndexIntradayResult, ChartDataResponse>
    {
        private readonly DateTimeFormatter dateTimeFormatter;

        public MarketIndexIntradayResultAssembler(DateTimeFormatter formatter)
        {
            dateTimeFormatter = formatter;
        }

        public MarketIndexIntradayResult Convert(ChartDataResponse source)
        {
            var bdpc = new BasicDataPointCollection();
            var marketResult = new MarketIndexIntradayResult();
            foreach (var kvp in source.Data)
            {
                var session = kvp.Value.Sessions[0];
                var currentDate = marketResult.Start = session.Start;
                foreach (var dp in session.Trades)
                {
                    if (dp.HasValue)
                    {
                        bdpc.Add(new BasicDataPoint
                                     {
                                         DataPoint = new DoubleNumberStock(dp.Value),
                                         Date = DateTimeFormatter.ConvertToUtc(currentDate),
                                         DateDisplay = dateTimeFormatter.FormatDateTime(currentDate),
                                     });
                    }
                    currentDate = currentDate.AddMinutes(15);
                }

                marketResult.Code = kvp.Key;
                marketResult.High = new DoubleNumberStock(session.High.Price, 2);
                marketResult.Low = new DoubleNumberStock(session.Low.Price, 2);
                marketResult.Name = kvp.Value.Name;
                marketResult.PreviousClose = new DoubleNumberStock(double.Parse(session.PreviousClose.ToString()), 2);
                marketResult.DataPoints = bdpc;

                marketResult.Start = DateTimeFormatter.ConvertToUtc(marketResult.Start);
                marketResult.StartDescripter = dateTimeFormatter.FormatDate(marketResult.Start);
                marketResult.Provider = new Provider
                                            {
                                                Name = Properties.Settings.Default.MarketDataProvider, 
                                                ExternalUrl = Properties.Settings.Default.MarketDataProviderUrl
                                            };


            }

            return marketResult;
        }
    }
}
