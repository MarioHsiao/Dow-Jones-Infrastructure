﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;

namespace DowJones.Extensions
{
    /// <summary>
    /// Extension methods for the DateTimeOffset data type.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Calculates the age based on today.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>The calculated age.</returns>
        public static int CalculateAge(this DateTime dateOfBirth)
        {
            return CalculateAge(dateOfBirth, DateTime.Today);
        }

        /// <summary>
        /// Calculates the age based on a passed reference date.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="referenceDate">The reference date to calculate on.</param>
        /// <returns>The calculated age.</returns>
        public static int CalculateAge(this DateTime dateOfBirth, DateTime referenceDate)
        {
            var years = referenceDate.Year - dateOfBirth.Year;
            if (referenceDate.Month < dateOfBirth.Month || (referenceDate.Month == dateOfBirth.Month && referenceDate.Day < dateOfBirth.Day))
            {
                --years;
            }

            return years;
        }

        /// <summary>
        /// Returns the number of days in the month of the provided date.
        /// </summary>
        /// <param name="date">The date-time object</param>
        /// <returns>The number of days.</returns>
        public static int GetCountDaysOfMonth(this DateTime date)
        {
            var nextMonth = date.AddMonths(1);
            return new DateTime(nextMonth.Year, nextMonth.Month, 1).AddDays(-1).Day;
        }

        /// <summary>
        /// Returns 12:00 AM of the day of the provided date
        /// </summary>
        /// <param name="dateTime">The DateTime to convert</param>
        /// <returns>12:00 AM of the day of the provided date</returns>
        public static DateTime GetDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        /// <summary>
        /// Returns the first day of the month of the provided date.
        /// </summary>
        /// <param name="date">The date-time object</param>
        /// <returns>The first day of the month</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Returns the first day of the month of the provided date.
        /// </summary>
        /// <param name="date">The date-time object</param>
        /// <param name="dayOfWeek">The desired day of week.</param>
        /// <returns>The first day of the month</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetFirstDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek)
            {
                dt = dt.AddDays(1);
            }

            return dt;
        }

        /// <summary>
        /// Returns the last day of the month of the provided date.
        /// </summary>
        /// <param name="date">The date-time object</param>
        /// <returns>The last day of the month.</returns>
        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, GetCountDaysOfMonth(date));
        }

        /// <summary>
        /// Returns the last day of the month of the provided date.
        /// </summary>
        /// <param name="date">The date-time object</param>
        /// <param name="dayOfWeek">The desired day of week.</param>
        /// <returns>The date time</returns>
        public static DateTime GetLastDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetLastDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek)
            {
                dt = dt.AddDays(-1);
            }

            return dt;
        }

        /// <summary>
        /// Indicates whether the date is today.
        /// </summary>
        /// <param name="dt">The date time object.</param>
        /// <returns>
        /// <c>true</c> if the specified date is today; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsToday(this DateTime dt)
        {
            return dt.Date == DateTime.Today;
        }

        /// <summary>
        /// Sets the time on the specified DateTime value.
        /// </summary>
        /// <param name="date">The base date.</param>
        /// <param name="hours">The hours to be set.</param>
        /// <param name="minutes">The minutes to be set.</param>
        /// <param name="seconds">The seconds to be set.</param>
        /// <returns>The DateTime including the new time value</returns>
        public static DateTime SetTime(this DateTime date, int hours, int minutes, int seconds)
        {
            return date.SetTime(new TimeSpan(hours, minutes, seconds));
        }

        /// <summary>
        /// Sets the time on the specified DateTime value.
        /// </summary>
        /// <param name="date">The base date.</param>
        /// <param name="time">The TimeSpan to be applied.</param>
        /// <returns>
        /// The DateTime including the new time value
        /// </returns>
        public static DateTime SetTime(this DateTime date, TimeSpan time)
        {
            return date.Date.Add(time);
        }

        /// <summary>
        /// Converts a DateTime into a DateTimeOffset using the local system time zone.
        /// </summary>
        /// <param name="localDateTime">The local DateTime.</param>
        /// <returns>The converted DateTimeOffset</returns>
        public static DateTimeOffset ToDateTimeOffset(this DateTime localDateTime)
        {
            return localDateTime.ToDateTimeOffset(null);
        }

        /// <summary>
        /// Converts a DateTime into a DateTimeOffset using the specified time zone.
        /// </summary>
        /// <param name="localDateTime">The local DateTime.</param>
        /// <param name="localTimeZone">The local time zone.</param>
        /// <returns>The converted DateTimeOffset</returns>
        public static DateTimeOffset ToDateTimeOffset(this DateTime localDateTime, TimeZoneInfo localTimeZone)
        {
            localTimeZone = localTimeZone ?? TimeZoneInfo.Local;

            if (localDateTime.Kind != DateTimeKind.Unspecified)
            {
                localDateTime = new DateTime(localDateTime.Ticks, DateTimeKind.Unspecified);
            }

            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, localTimeZone);
        }

        /// <summary>
        /// Gets the first day of the week using the current culture.
        /// </summary>
        /// <param name="date">The date-time object.</param>
        /// <returns>The first day of the week</returns>
        public static DateTime GetFirstDayOfWeek(this DateTime date)
        {
            return date.GetFirstDayOfWeek(null);
        }

        /// <summary>
        /// Gets the first day of the week using the specified culture.
        /// </summary>
        /// <param name="date">The date-time object.</param>
        /// <param name="cultureInfo">The culture to determine the first weekday of a week.</param>
        /// <returns>The first day of the week</returns>
        public static DateTime GetFirstDayOfWeek(this DateTime date, CultureInfo cultureInfo)
        {
            cultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;

            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            while (date.DayOfWeek != firstDayOfWeek)
            {
                date = date.AddDays(-1);
            }

            return date;
        }

        /// <summary>
        /// Gets the last day of the week using the current culture.
        /// </summary>
        /// <param name="date">The date-time object.</param>
        /// <returns>The first day of the week</returns>
        public static DateTime GetLastDayOfWeek(this DateTime date)
        {
            return date.GetLastDayOfWeek(null);
        }

        /// <summary>
        /// Gets the last day of the week using the specified culture.
        /// </summary>
        /// <param name="date">The date-time object.</param>
        /// <param name="cultureInfo">The culture to determine the first weekday of a week.</param>
        /// <returns>The first day of the week</returns>
        public static DateTime GetLastDayOfWeek(this DateTime date, CultureInfo cultureInfo)
        {
            return date.GetFirstDayOfWeek(cultureInfo).AddDays(6);
        }

        /// <summary>
        /// Gets the next occurence of the specified weekday within the current week using the current culture.
        /// </summary>
        /// <param name="date">The base date.</param>
        /// <param name="weekday">The desired weekday.</param>
        /// <returns>The calculated date.</returns>
        /// <example><code>
        /// var thisWeeksMonday = DateTime.Now.GetWeekday(DayOfWeek.Monday);
        /// </code></example>
        public static DateTime GetWeeksWeekday(this DateTime date, DayOfWeek weekday)
        {
            return date.GetWeeksWeekday(weekday, null);
        }

        /// <summary>
        /// Gets the next occurence of the specified weekday within the current week using the specified culture.
        /// </summary>
        /// <param name="date">The base date.</param>
        /// <param name="weekday">The desired weekday.</param>
        /// <param name="cultureInfo">The culture to determine the first weekday of a week.</param>
        /// <returns>The calculated date.</returns>
        /// <example><code>
        /// var thisWeeksMonday = DateTime.Now.GetWeekday(DayOfWeek.Monday);
        /// </code></example>
        public static DateTime GetWeeksWeekday(this DateTime date, DayOfWeek weekday, CultureInfo cultureInfo)
        {
            var firstDayOfWeek = date.GetFirstDayOfWeek(cultureInfo);
            return firstDayOfWeek.GetNextWeekday(weekday);
        }

        /// <summary>
        /// Gets the next occurence of the specified weekday.
        /// </summary>
        /// <param name="date">The base date.</param>
        /// <param name="weekday">The desired weekday.</param>
        /// <returns>The calculated date.</returns>
        /// <example><code>
        /// var lastMonday = DateTime.Now.GetNextWeekday(DayOfWeek.Monday);
        /// </code></example>
        public static DateTime GetNextWeekday(this DateTime date, DayOfWeek weekday)
        {
            while (date.DayOfWeek != weekday)
            {
                date = date.AddDays(1);
            }

            return date;
        }

        /// <summary>
        /// Gets the previous occurence of the specified weekday.
        /// </summary>
        /// <param name="date">The base date.</param>
        /// <param name="weekday">The desired weekday.</param>
        /// <returns>The calculated date.</returns>
        /// <example><code>
        /// var lastMonday = DateTime.Now.GetPreviousWeekday(DayOfWeek.Monday);
        /// </code></example>
        public static DateTime GetPreviousWeekday(this DateTime date, DayOfWeek weekday)
        {
            while (date.DayOfWeek != weekday)
            {
                date = date.AddDays(-1);
            }

            return date;
        }

        /// <summary>
        /// Toes the relative date string.
        /// </summary>
        /// <param name="date">The date-time object.</param>
        /// <returns>A string object.</returns>
        public static string ToRelativeDateString(this DateTime date)
        {
            return GetRelativeDateValue(date, DateTime.Now);
        }

        /// <summary>
        /// Toes the relative date string UTC.
        /// </summary>
        /// <param name="date">The date-time object.</param>
        /// <returns>A string object.</returns>
        public static string ToRelativeDateStringUtc(this DateTime date)
        {
            return GetRelativeDateValue(date, DateTime.UtcNow);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="date">The date-time object.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public static string ToString(this DateTime? date)
        {
            return date.ToString(null, DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="date">The date-time object.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public static string ToString(this DateTime? date, string format)
        {
            return date.ToString(format, DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="date">The date-time object.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public static string ToString(this DateTime? date, IFormatProvider provider)
        {
            return date.ToString(null, provider);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="date">The date-time object.</param>
        /// <param name="format">The format.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public static string ToString(this DateTime? date, string format, IFormatProvider provider)
        {
            return date.HasValue ? date.Value.ToString(format, provider) : string.Empty;
        }

        private static string GetRelativeDateValue(DateTime date, DateTime comparedTo)
        {
            var diff = comparedTo.Subtract(date);
            if (diff.TotalDays >= 365)
            {
                return string.Concat("on ", date.ToString("MMMM d, yyyy"));
            }

            if (diff.TotalDays >= 7)
            {
                return string.Concat("on ", date.ToString("MMMM d"));
            }

            if (diff.TotalDays > 1)
            {
                return string.Format("{0:N0} days ago", diff.TotalDays);
            }

            if (diff.TotalDays == 1)
            {
                return "yesterday";
            }

            if (diff.TotalHours >= 2)
            {
                return string.Format("{0:N0} hours ago", diff.TotalHours);
            }

            if (diff.TotalMinutes >= 60)
            {
                return "more than an hour ago";
            }

            if (diff.TotalMinutes >= 5)
            {
                return string.Format("{0:N0} minutes ago", diff.TotalMinutes);
            }

            return diff.TotalMinutes >= 1 ? "a few minutes ago" : "less than a minute ago";
        }

        public static string ToElapsedTime(this DateTime d)
        {
            // 1.
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(d);

            // 2.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0 || dayDiff >= 31)
            {
                return null;
            }

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return "{0} second{1} ago".FormatWith(secDiff, secDiff < 2 ? "": "s");
                }
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return "1 minute ago";
                }
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("{0} minutes ago",
                        Math.Floor((double)secDiff / 60));
                }
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    return "1 hour ago";
                }
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format("{0} hours ago",
                        Math.Floor((double)secDiff / 3600));
                }
            }
            // 6.
            // Handle previous days.
            if (dayDiff == 1)
            {
                return "yesterday";
            }
            if (dayDiff < 7)
            {
                return string.Format("{0} days ago",
                dayDiff);
            }
            if (dayDiff < 31)
            {
                return string.Format("{0} weeks ago",
                Math.Ceiling((double)dayDiff / 7));
            }
            return null;
        }

    }
}