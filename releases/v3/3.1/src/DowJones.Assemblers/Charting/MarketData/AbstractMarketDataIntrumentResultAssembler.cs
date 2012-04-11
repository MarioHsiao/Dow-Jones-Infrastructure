using System;
using System.Linq;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Formatters.Globalization.TimeZone;
using DowJones.MarketWatch.Dylan.Core.Financialdata;
using DowJones.Models.Charting;
using DowJones.Models.Charting.MarketData;
using DowJones.Models.Company;
using DowJones.Preferences;
using DowJones.Properties;
using TimeZoneInfo = System.TimeZoneInfo;

namespace DowJones.Assemblers.Charting.MarketData
{
    public class AbstractMarketDataIntrumentResultAssembler
    {
        private const string GreenwichMeanTimePreferenceString = "on, +00:00|1, on";
        private const string EasternStandardTimeId = "Eastern Standard Time";
        protected readonly DateTimeFormatter PrefBasedDateTimeFormatter;
        protected readonly DateTimeFormatter GreenwichMeanTimeBasedDateTimeFormatter;
        protected readonly TimeZoneInfo EasternTimeZone;
        protected readonly DateTimeFormatter GmtBasedFormatter;
        protected readonly IPreferences Preferences;

        protected AbstractMarketDataIntrumentResultAssembler(IPreferences preferences)
        {
            Preferences = preferences;
            PrefBasedDateTimeFormatter = new DateTimeFormatter(Preferences);
            GreenwichMeanTimeBasedDateTimeFormatter = new DateTimeFormatter(Preferences.InterfaceLanguage, GreenwichMeanTimePreferenceString);
            GmtBasedFormatter = new DateTimeFormatter(Preferences.InterfaceLanguage, TimeZoneManager.GmtTimeZone);
            EasternTimeZone = TimeZoneInfo.FindSystemTimeZoneById(EasternStandardTimeId);
        }

        private static DateTime ConvertToGmt(DateTime time, TimeZoneInfo timeZone)
        {
            var tempDate = DateTimeFormatter.ConvertToUtc(time);
            return tempDate.Add(timeZone.GetUtcOffset(tempDate).Negate());
        }

        private static string ShortTimeZoneFormat(string timeZoneStandardName)
        {
            var timeZoneElements = timeZoneStandardName.Split(' ');
            return timeZoneElements.Aggregate(String.Empty, (current, element) => current + element[0]);
        }

        internal MarketDataInstrumentIntradayResult GetMarketDataInstrumentIntradayResult(string code, string name, bool isIndex, Thunderball.Library.Charting.Session session, Match match, int barSize = 15, long returnCode = 0, string statusMessage = null)
        {
            var marketResult = new MarketDataInstrumentIntradayResult { ReturnCode = returnCode, StatusMessage = statusMessage };
            if (returnCode != 0)
            {
                return marketResult;
            }

            var utcOffsetHours = 0;
            var utcOffsetMinutes = EasternTimeZone.GetUtcOffset(DateTime.Now).TotalMinutes;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(EasternStandardTimeId);
            if (match != null && match.TimeZoneInfo != null)
            {
                utcOffsetHours = match.TimeZoneInfo.UtcOffsetHours;
                utcOffsetMinutes = match.TimeZoneInfo.UtcOffsetMinutes;
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(match.TimeZoneInfo.TimeZone);
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
                string abbr;
                if (lastDataPoint.Date != null)
                {
                    // update the gmt based one
                    marketResult.LastUpdated = (DateTime)lastDataPoint.Date;
                    marketResult.LastUpdatedDescripter = GmtBasedFormatter.FormatLongDateTime(marketResult.LastUpdated);

                    // update the gmt based one
                    marketResult.AdjustedLastUpdated = ConvertToGmt((DateTime)lastDataPoint.Date, timeZone);
                    abbr = timeZone.IsDaylightSavingTime(marketResult.AdjustedLastUpdated) ? ShortTimeZoneFormat(timeZone.DaylightName) : ShortTimeZoneFormat(timeZone.StandardName);

                    marketResult.AdjustedLastUpdatedDescripter = string.Concat(
                        GreenwichMeanTimeBasedDateTimeFormatter.FormatLongDateTime(marketResult.AdjustedLastUpdated.AddHours(utcOffsetHours).AddMinutes(utcOffsetMinutes)),
                        " ",
                        abbr);
                }
                else
                {
                    marketResult.LastUpdatedDescripter = marketResult.AdjustedLastUpdatedDescripter = string.Empty;
                }

                // pad out dates for drawing the charts correctly
                var tempEndDate = marketResult.End.AddHours(3);
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

                if (lastValue.HasValue)
                {
                    marketResult.Last = new DoubleNumberStock(lastValue.Value);
                    marketResult.PercentChange = new PercentStock((double)((lastValue - session.PreviousClose.Value) / session.PreviousClose.Value) * 100);
                    if (match != null && match.Trading.ChangePercent.HasValue)
                    {
                        marketResult.PercentChange = new PercentStock(match.Trading.ChangePercent.GetValueOrDefault());
                    }
                }
                else
                {
                    marketResult.Last = marketResult.PreviousClose;
                }

                if (match == null)
                {
                    marketResult.Symbol = code;
                    marketResult.Name = name;
                    marketResult.High = new DoubleNumberStock(session.High.Price);
                    marketResult.Low = new DoubleNumberStock(session.Low.Price);

                }
                else
                {
                    marketResult.Symbol = match.Instrument.Ticker;
                    marketResult.Isin = match.Instrument.Isin;
                    marketResult.Sedol = match.Instrument.Sedol;
                    marketResult.Cusip = match.Instrument.Cusip;
                    marketResult.Name = string.Concat(match.Instrument.CommonName, " [", match.Instrument.Exchange.Ticker, "]");
                    marketResult.High = new DoubleNumberStock(match.Trading.High.Value);
                    marketResult.Low = new DoubleNumberStock(match.Trading.Low.Value);
                    marketResult.Open = new DoubleNumberStock(match.Trading.Open.Value);

                    if (match.Trading.Last != null && match.Trading.Last.Time.HasValue)
                    {

                        // get the last value from Dylan if available
                        marketResult.Last = new DoubleNumberStock(match.Trading.Last.Price.Value);

                        // update the gmt based one
                        marketResult.LastUpdated = (DateTime)match.Trading.Last.Time;
                        marketResult.LastUpdatedDescripter = GmtBasedFormatter.FormatLongDateTime(marketResult.LastUpdated);

                        // update the gmt based one
                        marketResult.AdjustedLastUpdated = ConvertToGmt((DateTime)match.Trading.Last.Time, timeZone);
                        abbr = timeZone.IsDaylightSavingTime(marketResult.AdjustedLastUpdated) ? ShortTimeZoneFormat(timeZone.DaylightName) : ShortTimeZoneFormat(timeZone.StandardName);
                        
                        marketResult.AdjustedLastUpdatedDescripter = string.Concat(
                           GreenwichMeanTimeBasedDateTimeFormatter.FormatLongDateTime(marketResult.AdjustedLastUpdated.AddHours(utcOffsetHours).AddMinutes(utcOffsetMinutes)),
                           " ",
                           abbr);
                    }
                }

                marketResult.IsIndex = isIndex;
                marketResult.DataPoints = dataPoints;
                marketResult.PreviousClose = new DoubleNumberStock(double.Parse(session.PreviousClose.ToString()));

                // update the start date
                marketResult.Start = DateTimeFormatter.ConvertToUtc(marketResult.Start);
                marketResult.StartDescripter = GmtBasedFormatter.FormatLongDateTime(marketResult.Start);

                // update the stop date
                marketResult.Stop = DateTimeFormatter.ConvertToUtc(marketResult.Stop);
                marketResult.StopDescripter = GmtBasedFormatter.FormatLongDateTime(marketResult.Stop);

                // update the adjusted start date
                marketResult.AdjustedStart = ConvertToGmt(DateTimeFormatter.ConvertToUtc(marketResult.Start), timeZone);
                abbr = timeZone.IsDaylightSavingTime(marketResult.AdjustedStart) ? ShortTimeZoneFormat(timeZone.DaylightName) : ShortTimeZoneFormat(timeZone.StandardName);
                
                marketResult.AdjustedStartDescripter = string.Concat(
                     GreenwichMeanTimeBasedDateTimeFormatter.FormatLongDateTime(marketResult.AdjustedStart.AddHours(utcOffsetHours).AddMinutes(utcOffsetMinutes)),
                     " ",
                     abbr);

                // update the adjusted stop date
                marketResult.AdjustedStop = ConvertToGmt(DateTimeFormatter.ConvertToUtc(marketResult.Stop), timeZone);
                marketResult.AdjustedStopDescripter = string.Concat(
                       GreenwichMeanTimeBasedDateTimeFormatter.FormatLongDateTime(marketResult.AdjustedStop.AddHours(utcOffsetHours).AddMinutes(utcOffsetMinutes)),
                       " ",
                       abbr);

                marketResult.Provider = new Provider
                                            {
                                                Name = Settings.Default.MarketDataProvider,
                                                ExternalUrl = Settings.Default.MarketDataProviderUrl
                                            };
            }
            return marketResult;
        }
    }
}