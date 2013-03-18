using System;
using DowJones.Infrastructure;
using DowJones.Mapping;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Dates = Factiva.Gateway.Messages.Search.V2_0.Dates;
using DateFormat = Factiva.Gateway.Messages.Search.V2_0.DateFormat;

namespace DowJones.Managers.Search
{
    public class DateRangeToSearchDatesMapper : TypeMapper<DateRange, Dates>
    {
        public override Dates Map(DateRange source)
        {
            return Map(source, PreferenceDateFormat.MDY);
        }

        public static Dates Map(DateRange source, PreferenceDateFormat dateFormat)
        {
            var range = new Factiva.Gateway.Messages.Search.V2_0.DateRange
                                {
                                    From = Parse(source.Start.GetValueOrDefault(DateTime.MinValue), dateFormat),
                                    To = Parse(source.End.GetValueOrDefault(DateTime.Now), dateFormat),
                                };

            return new Dates
            {
                Format = Map(dateFormat),
                Range = range,
                __formatSpecified = (dateFormat != PreferenceDateFormat.ISO)
                
            };
        }

        private static DateFormat Map(PreferenceDateFormat dateFormat)
        {
            switch (dateFormat)
            {
                case PreferenceDateFormat.MDY:
                    return DateFormat.MMDDCCYY;
                case PreferenceDateFormat.DMY:
                    return DateFormat.DDMMCCYY;
                default:
                    return DateFormat.MMDDCCYY;
            }
        }

        private static string Parse(DateTime source, PreferenceDateFormat dateFormat)
        {
            string format;

            switch (dateFormat)
            {
                case PreferenceDateFormat.MDY:
                    format = "{1:00}/{0:00}/{2:0000}";
                    break;
                case PreferenceDateFormat.DMY:
                    format = "{0:00}/{1:00}/{2:0000}";
                    break;
                default:
                    format = "{2:0000}{1:00}{0:00}";
                    break;
            }

            return String.Format(format,
                                 source.Day,
                                 source.Month,
                                 source.Year);
        }
    }
}