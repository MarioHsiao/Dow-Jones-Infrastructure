using System;
using System.Linq;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Formatters.Globalization.TimeZone;
using DowJones.Managers.MarketWatch.Instrument;
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

        private static string NormalizeTimeZoneStandardName(string timezoneName)
        {
            switch (timezoneName)
            {
                case "Malay Peninsula Standard Time":
                    return "Singapore Standard Time";
                default:
                    return timezoneName;
            }
        }

        private static string GenerateAbbreviation(string timeZoneStandardName)
        {
            var timeZoneElements = timeZoneStandardName.Split(' ');
            return timeZoneElements.Aggregate(String.Empty, (current, element) => current + element[0]);
        }

        internal MarketDataInstrumentIntradayResult GetMarketDataInstrumentIntradayResult(string code, string name, bool isIndex, Thunderball.Library.Charting.Session session, Match match, string requestId = null, int barSize = 15, long returnCode = 0, string statusMessage = null)
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
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(NormalizeTimeZoneStandardName(match.TimeZoneInfo.TimeZone));
            }
            
            var dataPoints = new BasicDataPointCollection();
            
            var currentDate = marketResult.Start = (session == null ? null : (DateTime?)session.Start);
            double? lastValue = null;
            double? firstValue = null;
            if (session != null)
            {
                marketResult.End = session.Stop;
                marketResult.Stop = session.Stop;
                firstValue = session.Trades.Where(trade => trade.HasValue).ToList().FirstOrDefault();
            }
            
            var index = 0;
            var lastIndex = 0;
            if (session != null && session.PreviousClose != null)
            {
                foreach (var trade in session.Trades)
                {
                    if (trade.HasValue)
                    {
                        dataPoints.Add(new BasicDataPoint
                                     {
                                         DataPoint = new DoubleNumberStock(trade.Value),
                                         Date = DateTimeFormatter.ConvertToUtc((DateTime)currentDate),
                                         DateDisplay = GmtBasedFormatter.FormatDateTime((DateTime) currentDate),
                                     });
                        lastValue = trade.Value;
                        lastIndex = index;
                    }
                    else if (lastValue.HasValue)
                    {
                        dataPoints.Add(new BasicDataPoint
                                     {
                                         DataPoint = new DoubleNumberStock(lastValue.Value),
                                         Date = DateTimeFormatter.ConvertToUtc((DateTime) currentDate),
                                         DateDisplay = GmtBasedFormatter.FormatDateTime((DateTime) currentDate),
                                     });
                    }
                    else if (firstValue.HasValue)
                    {
                        dataPoints.Add(new BasicDataPoint
                        {
                            DataPoint = new DoubleNumberStock(firstValue.Value),
                            Date = DateTimeFormatter.ConvertToUtc((DateTime) currentDate),
                            DateDisplay = GmtBasedFormatter.FormatDateTime((DateTime) currentDate),
                        });
                    }
                    else
                    {
                        dataPoints.Add(new BasicDataPoint
                        {
                            DataPoint = null,
                            Date = DateTimeFormatter.ConvertToUtc((DateTime) currentDate),
                            DateDisplay = GmtBasedFormatter.FormatDateTime((DateTime) currentDate),
                        });
                    }

                    currentDate = ((DateTime) currentDate).AddMinutes(barSize);
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
                    marketResult.AdjustedLastUpdated = ConvertToGmt((DateTime)lastDataPoint.Date, timeZone);
                    var temp = (DateTime) marketResult.AdjustedLastUpdated;
                    marketResult.AdjustedLastUpdatedTimeZoneName = timeZone.IsDaylightSavingTime(temp) ? timeZone.DaylightName : timeZone.StandardName;
                    marketResult.AdjustedLastUpdatedTimeZoneAbbr = timeZone.IsDaylightSavingTime(temp) ? GenerateAbbreviation(timeZone.DaylightName) : GenerateAbbreviation(timeZone.StandardName);
                    marketResult.AdjustedLastUpdatedDescripter = GreenwichMeanTimeBasedDateTimeFormatter.FormatLongDateTime(temp.AddHours(utcOffsetHours).AddMinutes(utcOffsetMinutes));
                }
                else
                {
                    marketResult.LastUpdatedDescripter = marketResult.AdjustedLastUpdatedDescripter = string.Empty;
                }

                // pad out dates for drawing the charts correctly
                var tempEndDate = ((DateTime)marketResult.End).AddHours(4);
                while (currentDate <= tempEndDate)
                {
                    dataPoints.Add(new BasicDataPoint
                    {
                        DataPoint = null,
                        Date = DateTimeFormatter.ConvertToUtc((DateTime) currentDate),
                        DateDisplay = GmtBasedFormatter.FormatDateTime((DateTime) currentDate),
                    });
                    currentDate = ((DateTime)currentDate).AddMinutes(barSize);
                }

                if (lastValue.HasValue)
                {
                    marketResult.Last = new DoubleNumberStock(lastValue.Value);
                    marketResult.PercentChange = new PercentStock((double)((lastValue - session.PreviousClose.Value) / session.PreviousClose.Value) * 100);
                    
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

                marketResult.IsIndex = isIndex;
                marketResult.DataPoints = dataPoints;
                marketResult.PreviousClose = new DoubleNumberStock(double.Parse(session.PreviousClose.ToString()));

                // update the start date
                marketResult.Start = DateTimeFormatter.ConvertToUtc((DateTime)marketResult.Start);
                marketResult.StartDescripter = GmtBasedFormatter.FormatLongDateTime((DateTime)marketResult.Start);

                // update the stop date
                marketResult.Stop = DateTimeFormatter.ConvertToUtc(marketResult.Stop);
                marketResult.StopDescripter = GmtBasedFormatter.FormatLongDateTime(marketResult.Stop);

                // update the adjusted start date
                marketResult.AdjustedStart = ConvertToGmt(DateTimeFormatter.ConvertToUtc((DateTime)marketResult.Start), timeZone);
                marketResult.AdjustedStartUpdatedTimeZoneName = timeZone.IsDaylightSavingTime(marketResult.AdjustedStart) ? timeZone.DaylightName : timeZone.StandardName;
                marketResult.AdjustedStartUpdatedTimeZoneAbbr = timeZone.IsDaylightSavingTime(marketResult.AdjustedStart) ? GenerateAbbreviation(timeZone.DaylightName) : GenerateAbbreviation(timeZone.StandardName);
                marketResult.AdjustedStartDescripter = GreenwichMeanTimeBasedDateTimeFormatter.FormatLongDateTime(marketResult.AdjustedStart.AddHours(utcOffsetHours).AddMinutes(utcOffsetMinutes));

                // update the adjusted stop date
                marketResult.AdjustedStop = ConvertToGmt(DateTimeFormatter.ConvertToUtc(marketResult.Stop), timeZone);
                marketResult.AdjustedStopDescripter = GreenwichMeanTimeBasedDateTimeFormatter.FormatLongDateTime(marketResult.AdjustedStop.AddHours(utcOffsetHours).AddMinutes(utcOffsetMinutes));
            }

            if (match != null)
            {
                if (session == null)
                {
                    // update with quote response.
                    marketResult.High =  new DoubleNumberStock(match.Trading.High.Value);
                    marketResult.Low = new DoubleNumberStock(match.Trading.Low.Value);
                    marketResult.Last = new DoubleNumberStock(match.Trading.Last.Price.Value);
                }

                if (match.Trading.ChangePercent.HasValue)
                {
                    marketResult.PercentChange = new PercentStock(match.Trading.ChangePercent.GetValueOrDefault());
                }

                marketResult.RequestedId = requestId;
                marketResult.Symbol = match.Instrument.Ticker;
                marketResult.Isin = match.Instrument.Isin;
                marketResult.Sedol = match.Instrument.Sedol;
                marketResult.Cusip = match.Instrument.Cusip;
                marketResult.Currency = match.Trading.Open.Iso;
                marketResult.FCode = (requestId != null && requestId.StartsWith(string.Concat(SymbolDialectType.Factiva.ToString(), ":"))) ? requestId.Substring(requestId.IndexOf(":") + 1) : null;
                marketResult.Name = match.Instrument.CommonName;
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
                    var temp = (DateTime)marketResult.AdjustedLastUpdated;
                    marketResult.AdjustedLastUpdatedTimeZoneName = timeZone.IsDaylightSavingTime(temp) ? timeZone.DaylightName : timeZone.StandardName;
                    marketResult.AdjustedLastUpdatedTimeZoneAbbr = timeZone.IsDaylightSavingTime(temp) ? GenerateAbbreviation(timeZone.DaylightName) : GenerateAbbreviation(timeZone.StandardName);
                    marketResult.AdjustedLastUpdatedDescripter = GreenwichMeanTimeBasedDateTimeFormatter.FormatLongDateTime(temp.AddHours(utcOffsetHours).AddMinutes(utcOffsetMinutes));
                }
                marketResult.Exchange = new Exchange
                {
                    Code = match.Instrument.Exchange.Ticker,
                    Descriptor = match.Instrument.Exchange.CommonName,
                };

                if (marketResult.AdjustedLastUpdated.HasValue)
                {
                    marketResult.Exchange.TimeZoneDescriptor = timeZone.IsDaylightSavingTime((DateTime)marketResult.AdjustedLastUpdated) ? timeZone.DaylightName : timeZone.StandardName;
                    marketResult.Exchange.TimeZoneAbbreviation = timeZone.IsDaylightSavingTime((DateTime)marketResult.AdjustedLastUpdated) ? GenerateAbbreviation(timeZone.DaylightName) : GenerateAbbreviation(timeZone.StandardName);
                }
                else
                {
                    marketResult.Exchange.TimeZoneDescriptor = timeZone.IsDaylightSavingTime(DateTime.Now) ? timeZone.DaylightName : timeZone.StandardName;
                    marketResult.Exchange.TimeZoneAbbreviation = timeZone.IsDaylightSavingTime(DateTime.Now) ? GenerateAbbreviation(timeZone.DaylightName) : GenerateAbbreviation(timeZone.StandardName);
                }
            }

            marketResult.Provider = new Provider
            {
                Name = Settings.Default.MarketDataProvider,
                ExternalUrl = Settings.Default.MarketDataProviderUrl
            };
            return marketResult;
        }
    }
}