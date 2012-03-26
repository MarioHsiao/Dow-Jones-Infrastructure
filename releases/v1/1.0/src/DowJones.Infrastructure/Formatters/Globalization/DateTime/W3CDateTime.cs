using System;
using System.Text.RegularExpressions;

namespace DowJones.Utilities.Formatters.Globalization
{
    public struct W3CDateTime
    {
        private const string Rfc822DateFormat =
            @"^((Mon|Tue|Wed|Thu|Fri|Sat|Sun), *)?(?<day>\d\d?) +" +
            @"(?<month>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) +" +
            @"(?<year>\d\d(\d\d)?) +" +
            @"(?<hour>\d\d):(?<min>\d\d)(:(?<sec>\d\d))? +" +
            @"(?<ofs>([+\-]?\d\d\d\d)|UT|GMT|EST|EDT|CST|CDT|MST|MDT|PST|PDT)$";

        private const string W3CDateFormat =
            @"^(?<year>\d\d\d\d)" +
            @"(-(?<month>\d\d)(-(?<day>\d\d)(T(?<hour>\d\d):(?<min>\d\d)(:(?<sec>\d\d)(?<ms>\.\d+)?)?" +
            @"(?<ofs>(Z|[+\-]\d\d:\d\d)))?)?)?$";

        private static readonly string[] MonthNames = new[]
                                                          {
                                                              "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                                                              "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
                                                          };

        private readonly DateTime _dtime;
        private readonly TimeSpan _ofs;


        public W3CDateTime(DateTime dt, TimeSpan off)
        {
            _ofs = off;
            _dtime = dt;
        }

        public DateTime UtcTime
        {
            get { return _dtime; }
        }

        public DateTime DateTime
        {
            get { return _dtime + _ofs; }
        }

        public TimeSpan UtcOffset
        {
            get { return _ofs; }
        }

        public static W3CDateTime Parse(string s)
        {
            var combinedFormat = string.Format(@"(?<rfc822>{0})|(?<w3c>{1})", Rfc822DateFormat, W3CDateFormat);

            // try to parse it
            var reDate = new Regex(combinedFormat);
            var m = reDate.Match(s);
            if (!m.Success)
            {
                // Didn't match either expression. Throw an exception.
                throw new FormatException("String is not a valid date time stamp.");
            }
            try
            {
                var isRfc822 = m.Groups["rfc822"].Success;
                var year = int.Parse(m.Groups["year"].Value);
                // handle 2-digit and 3-digit years
                if (year < 1000)
                {
                    if (year < 50) year = year + 2000;
                    else year = year + 1999;
                }

                int month;
                if (isRfc822)
                    month = ParseRfc822Month(m.Groups["month"].Value);
                else
                    month = (m.Groups["month"].Success) ? int.Parse(m.Groups["month"].Value) : 1;

                var day = m.Groups["day"].Success ? int.Parse(m.Groups["day"].Value) : 1;
                var hour = m.Groups["hour"].Success ? int.Parse(m.Groups["hour"].Value) : 0;
                var min = m.Groups["min"].Success ? int.Parse(m.Groups["min"].Value) : 0;
                var sec = m.Groups["sec"].Success ? int.Parse(m.Groups["sec"].Value) : 0;
                var ms = m.Groups["ms"].Success ? (int) Math.Round((1000*double.Parse(m.Groups["ms"].Value))) : 0;

                var ofs = TimeSpan.Zero;
                if (m.Groups["ofs"].Success)
                {
                    ofs = isRfc822 ? ParseRfc822Offset(m.Groups["ofs"].Value) : ParseW3COffset(m.Groups["ofs"].Value);
                }
                // datetime is stored in UTC
                return new W3CDateTime(new DateTime(year, month, day, hour, min, sec, ms) - ofs, ofs);
            }
            catch (Exception ex)
            {
                throw new FormatException("String is not a valid date time stamp.", ex);
            }
        }

        private static int ParseRfc822Month(string monthName)
        {
            for (var i = 0; i < 12; i++)
            {
                if (monthName == MonthNames[i])
                {
                    return i + 1;
                }
            }
            throw new ApplicationException("Invalid month name");
        }

        private static TimeSpan ParseRfc822Offset(string s)
        {
            if (s == string.Empty)
                return TimeSpan.Zero;
            var hours = 0;
            switch (s)
            {
                case "UT":
                case "GMT":
                    break;
                case "EDT":
                    hours = -4;
                    break;
                case "EST":
                case "CDT":
                    hours = -5;
                    break;
                case "CST":
                case "MDT":
                    hours = -6;
                    break;
                case "MST":
                case "PDT":
                    hours = -7;
                    break;
                case "PST":
                    hours = -8;
                    break;
                default:
                    if (s[0] == '+')
                    {
                        var sfmt = s.Substring(1, 2) + ":" + s.Substring(3, 2);
                        return TimeSpan.Parse(sfmt);
                    }
                    return TimeSpan.Parse(s.Insert(s.Length - 2, ":"));
            }
            return TimeSpan.FromHours(hours);
        }

        private static TimeSpan ParseW3COffset(string s)
        {
            if (s == string.Empty || s == "Z")
                return TimeSpan.Zero;
            return TimeSpan.Parse(s[0] == '+' ? s.Substring(1) : s);
        }

        // Format is "R" (RFC822) or "W" (W3C)
        public string ToString(string format)
        {
            switch (format)
            {
                case "R":
                    return (_dtime + _ofs).ToString("ddd, dd MMM yyyy HH:mm:ss ") +
                           FormatOffset(_ofs, "");
                case "W":
                    return (_dtime + _ofs).ToString("yyyy-MM-ddTHH:mm:ss") +
                           FormatOffset(_ofs, ":");
                default:
                    throw new ArgumentException("Unrecognized date format requested.");
            }
        }

        private static string FormatOffset(TimeSpan ofs, string separator)
        {
            var s = string.Empty;
            if (ofs >= TimeSpan.Zero)
                s = "+";
            return s + ofs.Hours.ToString("00") + separator + ofs.Minutes.ToString("00");
        }
    }
}