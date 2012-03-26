using System;
using System.Linq;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Formatters.Globalization.TimeZone;
using DowJones.Models.Charting;
using DowJones.Models.Charting.MarketData;
using DowJones.Models.Company;
using DowJones.Preferences;
using DowJones.Properties;

namespace DowJones.Assemblers.Charting.MarketData
{
    public class AbstractMarketDataIntrumentResultAssembler
    {
        private const string EasternStandarTimePreferenceString = "on, -05:00|1, on";
        protected readonly DateTimeFormatter PrefBasedDateTimeFormatter;
        protected readonly DateTimeFormatter EstBasedDateTimeFormatter;
        protected readonly DateTimeFormatter GmtBasedFormatter;
        protected readonly IPreferences Preferences;

        protected AbstractMarketDataIntrumentResultAssembler(IPreferences preferences)
        {
            Preferences = preferences;
            PrefBasedDateTimeFormatter = new DateTimeFormatter(Preferences);
            EstBasedDateTimeFormatter = new DateTimeFormatter(Preferences.InterfaceLanguage, EasternStandarTimePreferenceString); 
            GmtBasedFormatter = new DateTimeFormatter(Preferences.InterfaceLanguage, TimeZoneManager.GmtTimeZone);
        }

        private static DateTime GetEasternTime(DateTime gmtTime)
        {
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var tempDate = DateTimeFormatter.ConvertToUtc(gmtTime);
            return tempDate.Add(easternZone.GetUtcOffset(tempDate).Negate());
        }

        internal MarketDataInstrumentIntradayResult GetMarketDataInstrumentIntradayResult(string code, string name, bool isIndex, Thunderball.Library.Charting.Session session, int barSize = 15, long returnCode = 0, string statusMessage = null)
        {
            var marketResult = new MarketDataInstrumentIntradayResult {ReturnCode = returnCode, StatusMessage = statusMessage};
            if (returnCode != 0)
            {
                return marketResult;
            }

            var dataPoints = new BasicDataPointCollection();
            var currentDate = marketResult.Start = session.Start;
            marketResult.End = session.Stop;

            marketResult.Stop = session.Stop; 
            double? lastValue = null;
            var firstValue = session.Trades.Where(trade => trade.HasValue).ToList().FirstOrDefault();

            var index = 0;
            var lastIndex = 0;
            if (session.PreviousClose != null)
            {
                foreach (var trade in session.Trades)
                {
                    if (trade.HasValue) 
                    {
                        dataPoints.Add(new BasicDataPoint
                                     {
                                         DataPoint = new DoubleNumberStock(trade.Value),
                                         Date = DateTimeFormatter.ConvertToUtc(currentDate),
                                         DateDisplay = GmtBasedFormatter.FormatDateTime(currentDate),
                                     });
                        lastValue = trade.Value;
                        lastIndex = index;
                    }
                    else if (lastValue.HasValue)
                    {
                        dataPoints.Add(new BasicDataPoint
                                     {
                                         DataPoint = new DoubleNumberStock(lastValue.Value),
                                         Date = DateTimeFormatter.ConvertToUtc(currentDate),
                                         DateDisplay = GmtBasedFormatter.FormatDateTime(currentDate),
                                     });
                    }
                    else if (firstValue.HasValue)
                    {
                        dataPoints.Add(new BasicDataPoint
                        {
                            DataPoint = new DoubleNumberStock(firstValue.Value),
                            Date = DateTimeFormatter.ConvertToUtc(currentDate),
                            DateDisplay = GmtBasedFormatter.FormatDateTime(currentDate),
                        });
                    }
                    else
                    {
                        dataPoints.Add(new BasicDataPoint
                        {
                            DataPoint = null,
                            Date = DateTimeFormatter.ConvertToUtc(currentDate),
                            DateDisplay = GmtBasedFormatter.FormatDateTime(currentDate),
                        });
                    }

                    currentDate = currentDate.AddMinutes(barSize);
                    index++;
                }


                // set islast to the last datapoint
                var lastDataPoint = dataPoints[lastIndex];
                lastDataPoint.IsLast = true;
                if (lastDataPoint.Date != null)
                {
                    // update the gmt based one
                    marketResult.LastUpdated = (DateTime)lastDataPoint.Date;
                    marketResult.LastUpdatedDescripter = GmtBasedFormatter.FormatLongDateTime(marketResult.LastUpdated);

                    // update the gmt based one
                    marketResult.AdjustedLastUpdated = GetEasternTime((DateTime)lastDataPoint.Date);
                    marketResult.AdjustedLastUpdatedDescripter = EstBasedDateTimeFormatter.FormatLongDateTime(marketResult.AdjustedLastUpdated);
                }
                else
                {
                    marketResult.LastUpdatedDescripter = marketResult.AdjustedLastUpdatedDescripter = string.Empty;
                }


                // pad out dates for drawing the charts correctly
                var tempEndDate = marketResult.End.AddHours(-1);
                while (currentDate <= tempEndDate)
                {
                    dataPoints.Add(new BasicDataPoint
                    {
                        DataPoint = null,
                        Date = DateTimeFormatter.ConvertToUtc(currentDate),
                        DateDisplay = GmtBasedFormatter.FormatDateTime(currentDate),
                    });
                    currentDate = currentDate.AddMinutes(barSize);
                }

                marketResult.Symbol = code;
                marketResult.Name = name;
                marketResult.IsIndex = isIndex;
                marketResult.High = new DoubleNumberStock(session.High.Price);
                marketResult.Low = new DoubleNumberStock(session.Low.Price);
                marketResult.PreviousClose = new DoubleNumberStock(double.Parse(session.PreviousClose.ToString()));
                marketResult.DataPoints = dataPoints;

                // update the start date
                marketResult.Start = DateTimeFormatter.ConvertToUtc(marketResult.Start);
                marketResult.StartDescripter = GmtBasedFormatter.FormatLongDateTime(marketResult.Start);

                // update the adjusted start date
                marketResult.AdjustedStart = GetEasternTime(DateTimeFormatter.ConvertToUtc(marketResult.Start));
                marketResult.AdjustedStartDescripter = EstBasedDateTimeFormatter.FormatLongDateTime(marketResult.AdjustedStart);

                // update the stop date
                marketResult.Stop = DateTimeFormatter.ConvertToUtc(marketResult.Stop);
                marketResult.StopDescripter = GmtBasedFormatter.FormatLongDateTime(marketResult.Stop);

                // update the adjusted stop date
                marketResult.AdjustedStop = GetEasternTime(DateTimeFormatter.ConvertToUtc(marketResult.Stop));
                marketResult.AdjustedStopDescripter = EstBasedDateTimeFormatter.FormatLongDateTime(marketResult.AdjustedStop);

                marketResult.Provider = new Provider
                                            {
                                                Name = Settings.Default.MarketDataProvider,
                                                ExternalUrl = Settings.Default.MarketDataProviderUrl
                                            };

                if (lastValue.HasValue)
                {
                    marketResult.Last = new DoubleNumberStock(lastValue.Value);
                    marketResult.PercentChange = new PercentStock((double)((lastValue - session.PreviousClose.Value) / session.PreviousClose.Value) * 100);
                }
                else
                {
                    marketResult.Last = marketResult.PreviousClose;
                }
            }
            return marketResult;
        }
    }
}