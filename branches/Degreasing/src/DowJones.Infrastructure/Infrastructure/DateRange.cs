using System;
using System.Globalization;
using System.Text.RegularExpressions;
using DowJones.Extensions;

namespace DowJones.Infrastructure
{
    public class DateRange
    {
        public const string DateFormat = "yyyyMMdd";

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }


        public DateRange()
        {
        }

        public DateRange(DateTime? start, DateTime? end)
        {
            Start = start;
            End = end;
        }


        public TimeSpan? TimeSpan
        {
            get { return End - Start; }
        }


        public static string Serialize(DateRange dateRange, string format = DateFormat)
        {
            if (dateRange == null)
                return string.Empty;

            var start = dateRange.Start.HasValue ? dateRange.Start.Value.ToString(DateFormat) : string.Empty;
            var end = dateRange.End.HasValue ? dateRange.End.Value.ToString(DateFormat) : string.Empty;

            return string.Format("{0}-{1}", start, end);
        }

        public static string Serialize(DateTime? date, string format = DateFormat)
        {
            if (!date.HasValue)
                return string.Empty;

            return date.Value.ToString(format);
        }

        public static DateRange Deserialize(string start = null, string end = null)
        {
            var startDate = ParseDate(start);
            var endDate = ParseDate(end);

            return new DateRange(startDate, endDate);
        }

        public static DateRange Deserialize(string dateRange)
        {
            if (!dateRange.HasValue())
                return new DateRange();

            var matches = Regex.Match(dateRange, "(?<Start>[^-]*)-(?<End>.*)");
            var start = matches.Groups["Start"].Value.Trim();
            var end = matches.Groups["End"].Value.Trim();

            return Deserialize(start, end);
        }

        private static DateTime? ParseDate(string date)
        {
            DateTime parsed;

            if(!date.HasValue())
                return null;

            if (DateTime.TryParseExact(date, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out parsed))
                return parsed;

            if (DateTime.TryParse(date, out parsed))
                return parsed;

            return null;
        }
    }
}