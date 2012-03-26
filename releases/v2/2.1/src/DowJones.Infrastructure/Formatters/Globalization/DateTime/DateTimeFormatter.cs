// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeFormatter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Runtime.Serialization;
using DowJones.DependencyInjection;
using DowJones.Formatters.Globalization.Core;
using DowJones.Formatters.Globalization.Cultures;
using DowJones.Formatters.Globalization.TimeZone;
using DowJones.Preferences;

namespace DowJones.Formatters.Globalization.DateTime
{
    /// <summary>
    /// The clock type.
    /// </summary>
    [DataContract(Name = "clockType", Namespace = "")]
    public enum ClockType
    {
        /// <summary>
        /// The twelve hours.
        /// </summary>
        [EnumMember(Value = "TwelveHours")]
        TwelveHours, 

        /// <summary>
        /// The twenty four hours.
        /// </summary>
        [EnumMember(Value = "TwentyFourHours")]
        TwentyFourHours, 
    }

    /// <summary>
    /// Class used for formatting Dates and Times
    /// Uses the Abstract Factory Pattern for resolving cultures.
    /// </summary>
    public class DateTimeFormatter : IFormatter<Date>
    {
        private const string UnableToParseInputString = "Unable to parse Date input string";

        /// <summary>
        /// The _clock type.
        /// </summary>
        private ClockType clockType = ClockType.TwelveHours;

        // private readonly UITimeZone m_GMT_UITimeZone = (UITimeZone) TimeZoneManager.GmtTimeZone;

        /// <summary>
        /// The _culture info.
        /// </summary>
        private CultureInfo cultureInfo;

        /// <summary>
        /// The _internal interface language code.
        /// </summary>
        private string internalInterfaceLanguageCode = "en";

        /// <summary>
        /// The _preference.
        /// </summary>
        private string preference = string.Empty;

        /// <summary>
        /// The _regional culture.
        /// </summary>
        private BaseRegionalCulture regionalCulture;

        /// <summary>
        /// The _time zone builder.
        /// </summary>
        private TimeZoneBuilder timeZoneBuilder = new TimeZoneBuilder();

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeFormatter"/> class.
        /// </summary>
        /// <param name="interfaceLanguage">
        /// The interface Language.
        /// </param>
        public DateTimeFormatter(string interfaceLanguage)
        {
            InterfaceLanguageCode = interfaceLanguage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeFormatter"/> class.
        /// </summary>
        /// <param name="interfaceLanguage">
        /// The interface Language.
        /// </param>
        /// <param name="timeZone">
        /// The time Zone.
        /// </param>
        public DateTimeFormatter(string interfaceLanguage, System.TimeZone timeZone) : this(interfaceLanguage)
        {
            timeZoneBuilder.TimeZone = timeZone;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeFormatter"/> class.
        /// </summary>
        /// <param name="interfaceLanguage">
        /// The interface Language.
        /// </param>
        /// <param name="preference">
        /// The preference.
        /// </param>
        public DateTimeFormatter(string interfaceLanguage, string preference) : this(interfaceLanguage)
        {
            if (preference == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(preference) || !string.IsNullOrEmpty(preference.Trim()))
            {
                ParseTimeZonePreference(preference);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeFormatter"/> class.
        /// </summary>
        /// <param name="interfaceLanguage">
        /// The interface language.
        /// </param>
        /// <param name="preference">
        /// The preference.
        /// </param>
        /// <param name="clock">
        /// The clock.
        /// </param>
        public DateTimeFormatter(string interfaceLanguage, string preference, ClockType clock)
            : this(interfaceLanguage, preference)
        {
            clockType = clock;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeFormatter"/> class.
        /// </summary>
        /// <param name="preferences">The preferences.</param>
        [Inject("Disambiguating multiple constructors")]
        public DateTimeFormatter(IPreferences preferences)
            : this(preferences.InterfaceLanguage, preferences.TimeZone)
        {
            clockType = preferences.ClockType;
        }

        /// <summary>
        /// Gets the time zone preference.
        /// </summary>
        /// <value>The time zone preference.</value>
        public UITimeZone CurrentTimeZone
        {
            get { return timeZoneBuilder.UITimeZone; }
        }

        /// <summary>
        /// Gets or sets the type of the clock.
        /// </summary>
        /// <value>The type of the clock.</value>
        public ClockType ClockType
        {
            get { return clockType; }
            set { clockType = value; }
        }

        /// <summary>
        /// Gets the original preference.
        /// </summary>
        /// <value>The original preference.</value>
        public string Preference
        {
            get { return preference; }
        }

        /// <summary>
        /// Gets or sets InterfaceLanguageCode.
        /// </summary>
        public string InterfaceLanguageCode
        {
            get
            {
                return internalInterfaceLanguageCode;
            }

            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
                {
                    return;
                }

                SetRegionalCulture(value);
                internalInterfaceLanguageCode = regionalCulture.InternalLanguageCode;
                cultureInfo = regionalCulture.CreateCultureInfo();
            }
        }

        /// <summary>
        /// Gets or sets RegionalCulture.
        /// </summary>
        public BaseRegionalCulture RegionalCulture
        {
            get { return regionalCulture; }
            set { regionalCulture = value; }
        }

        /// <summary>
        /// Gets or sets CultureInfo.
        /// </summary>
        public CultureInfo CultureInfo
        {
            get { return cultureInfo; }
            set { cultureInfo = value; }
        }

        #region IFormatter<Date> Members

        /// <summary>
        /// The is format-able.
        /// </summary>
        /// <param name="formattableObject">The format-able object.</param>
        /// <returns>
        ///   <c>true</c> if the specified format-able object is format-able; otherwise, <c>false</c>.
        /// </returns>
        public bool IsFormattable(object formattableObject)
        {
            return formattableObject is Date;
        }

        /// <summary>
        /// The format.
        /// </summary>
        /// <param name="d">
        /// The date object.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public void Format(Date d)
        {
            var date = d;
            if (date == null)
            {
                throw new ArgumentException("You are only able to format objects of Type Date");
            }

            date.text = new Text { Value = Format(date.value) };
        }

        #endregion

        /// <summary>
        /// Parses the specified serialized.
        /// </summary>
        /// <param name="pref">
        /// The preference.
        /// </param>
        public void ParseTimeZonePreference(string pref)
        {
            preference = pref;
            timeZoneBuilder = new TimeZoneBuilder(pref);
        }

        /// <summary>
        /// Sets the regional culture.
        /// </summary>
        /// <param name="language">
        /// The language.
        /// </param>
        private void SetRegionalCulture(string language)
        {
            switch (language.ToLower())
            {
                default:
                    regionalCulture = new EnglishUnitedStatesRegionalCulture();
                    break;
                case "fr":
                    regionalCulture = new FrenchFranceRegionalCulture();
                    break;
                case "de":
                    regionalCulture = new GermanGermanyRegionalCulture();
                    break;
                case "es":
                    regionalCulture = new SpanishSpainRegionalCulture();
                    break;
                case "it":
                    regionalCulture = new ItalianItalyRegionalCulture();
                    break;
                case "zhtw":
                    regionalCulture = new ChineseTaiwainRegionalCulture();
                    break;
                case "zhcn":
                    regionalCulture = new ChineseChinaRegionalCulture();
                    break;
                case "ja":
                    regionalCulture = new JapaneesJapanRegionalCulture();
                    break;
                case "ru":
                    regionalCulture = new RussianRussiaRegionalCulture();
                    break;
            }
        }

        /// <summary>
        /// Formats the specified dt.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The format.
        /// </returns>
        public string Format(System.DateTime dt)
        {
            return FormatStandardDate(dt, false);
        }

        /// <summary>
        /// Formats the time.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The format time.
        /// </returns>
        public string FormatTime(System.DateTime dt)
        {
            return CoreFormatTime(dt);
        }

        /// <summary>
        /// Formats the date.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The format date.
        /// </returns>
        public string FormatDate(string s)
        {
            System.DateTime dateTime;
            if (ParseDate(s, out dateTime))
            {
                return Format(dateTime);
            }

            throw new ArgumentException(UnableToParseInputString);
        }

        /// <summary>
        /// Formats the date.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// The format date.
        /// </returns>
        public string FormatDate(System.DateTime dateTime)
        {
            return Format(dateTime);
        }

        /// <summary>
        /// Formats the long date.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The format long date.
        /// </returns>
        public string FormatLongDate(string s)
        {
            System.DateTime dateTime;
            if (ParseDate(s, out dateTime))
            {
                return FormatLongDate(dateTime);
            }

            throw new ArgumentException(UnableToParseInputString);
        }

        /// <summary>
        /// Formats the long date.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The format long date.
        /// </returns>
        public string FormatLongDate(System.DateTime dt)
        {
            // returned it back to original logic
            /*var temp = timeZoneBuilder.ConvertToLocalTime
                ? timeZoneBuilder.UITimeZone.ToLocalTime( dt, timeZoneBuilder.AdjustToDaylightSavingsTime )
                : dt;*/

            return dt.ToString(regionalCulture.LongDatePattern, cultureInfo);
        }

        /// <summary>
        /// Formats the short date.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The format short date.
        /// </returns>
        public string FormatShortDate(System.DateTime dt)
        {
            // returned it back to original logic
            /*var temp = timeZoneBuilder.ConvertToLocalTime
                ? timeZoneBuilder.UITimeZone.ToLocalTime(dt, timeZoneBuilder.AdjustToDaylightSavingsTime)
                : dt;*/

            return dt.ToString(regionalCulture.ShortDatePattern, cultureInfo);
        }

        /// <summary>
        /// Formats the short date.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The format short date.
        /// </returns>
        public string FormatShortDate(string s)
        {
            System.DateTime dateTime;
            if (ParseDate(s, out dateTime))
                return FormatShortDate(dateTime);
            throw new ArgumentException(UnableToParseInputString);
        }

        /// <summary>
        /// Formats the Standard date.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The format standard date.
        /// </returns>
        public string FormatStandardDate(System.DateTime dt)
        {
            return FormatStandardDate(dt, false);
        }

        /// <summary>
        /// Formats the Standard date.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <param name="usePreference">
        /// if set to <c>true</c> [use preference].
        /// </param>
        /// <returns>
        /// The format standard date.
        /// </returns>
        public string FormatStandardDate(System.DateTime dt, bool usePreference)
        {
            var temp = ConvertToUtc(dt);
            if (timeZoneBuilder.ConvertToLocalTime && usePreference)
            {
                temp = timeZoneBuilder.UITimeZone.ToLocalTime(temp, timeZoneBuilder.AdjustToDaylightSavingsTime);
                return temp.ToString(regionalCulture.StandardDatePattern, cultureInfo);
            }

            return temp.ToString(regionalCulture.StandardDatePattern, cultureInfo);
        }

        /// <summary>
        /// Formats the Standard date.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The format standard date.
        /// </returns>
        public string FormatStandardDate(string s)
        {
            System.DateTime dateTime;
            if (ParseDate(s, out dateTime))
                return FormatStandardDate(dateTime, false);
            throw new ArgumentException(UnableToParseInputString);
        }


        /// <summary>
        /// Formats the Standard date.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="usePreference">
        /// if set to <c>true</c> [use preference].
        /// </param>
        /// <returns>
        /// The format standard date.
        /// </returns>
        public string FormatStandardDate(string s, bool usePreference)
        {
            System.DateTime dateTime;
            if (ParseDate(s,out dateTime))
            {
                return FormatStandardDate(dateTime, usePreference);
            }

            throw new ArgumentException(UnableToParseInputString);
        }


        /// <summary>
        /// Formats the time.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The format time.
        /// </returns>
        public string FormatTime(string s)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(s.Trim()))
            {
                return null;
            }

            System.DateTime dateTime;

            // This is done to guarantee that this is a universal date time.
            if (!s.ToUpper().EndsWith("Z"))
                s = string.Concat(s.ToUpper(), "Z");

            if (System.DateTime.TryParse(s, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AdjustToUniversal | DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AssumeUniversal, out dateTime))
                return CoreFormatTime(dateTime);
            throw new ArgumentException(UnableToParseInputString);
        }


        /// <summary>
        /// Formats the specified year/month/day.
        /// </summary>
        /// <param name="year">
        /// The year.
        /// </param>
        /// <param name="month">
        /// The month.
        /// </param>
        /// <param name="day">
        /// The day.
        /// </param>
        /// <returns>
        /// The format.
        /// </returns>
        public string Format(int year, int month, int day)
        {
            var dateTime = new System.DateTime(year, month, day);
            return Format(dateTime);
        }

        /// <summary>
        /// Formats the time.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The core format time.
        /// </returns>
        private string CoreFormatTime(System.DateTime dt)
        {
            var temp = ConvertToUtc(dt);
            if (!timeZoneBuilder.ConvertToLocalTime)
            {
                return temp.ToString(MapToTimeGMTClockPattern(), cultureInfo);
            }

            temp = timeZoneBuilder.UITimeZone.ToLocalTime(temp, timeZoneBuilder.AdjustToDaylightSavingsTime);
            return temp.ToString(MapToTimeClockPattern(), cultureInfo);
        }

        /// <summary>
        /// Merges the specified date to a UTC date/time.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <returns>
        /// </returns>
        public static System.DateTime Merge(System.DateTime date, System.DateTime time)
        {
            return ConvertToUtc(date, time);
        }

        /// <summary>
        /// The convert to utc.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <returns>
        /// </returns>
        private static System.DateTime ConvertToUtc(System.DateTime date, System.DateTime time)
        {
            // Guarantee Both Parts are Local/UnSpecified
            if (date.Kind == DateTimeKind.Utc)
            {
                date = date.ToLocalTime();
            }

            if (time.Kind == DateTimeKind.Utc)
            {
                time = time.ToLocalTime();
            }

            // Set up a base that is out of date-light savings time. 1/3/01
            var baseDateTime = new System.DateTime(1, 1, 3, time.Hour, time.Minute, time.Second, time.Millisecond, DateTimeKind.Local);

            // Check to see if the system is set to a time-zone that is using Daylight
            if (System.DateTime.Now.IsDaylightSavingTime())
            {
                // Subtract the delta.
                var daylightTimes = System.TimeZone.CurrentTimeZone.GetDaylightChanges(baseDateTime.Year);
                baseDateTime = baseDateTime.Subtract(daylightTimes.Delta);
            }

            // Generate a good UTC time 
            var temp = baseDateTime.ToUniversalTime();
            temp = new System.DateTime(temp.Ticks, DateTimeKind.Utc);

            // output the correct combined UTC
            var nDate = new System.DateTime(date.Year, date.Month, date.Day, temp.Hour, temp.Minute, temp.Second, temp.Millisecond, DateTimeKind.Utc);
            return nDate;
        }

        /// <summary>
        /// The parse date.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The parse date.
        /// </returns>
        public static bool ParseDate(string s, out System.DateTime date)
        {
            // This is done to guarantee that this is a universal date time.
            return System.DateTime.TryParse(s, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AdjustToUniversal | DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AssumeUniversal, out date);
        }

        /// <summary>
        /// The convert to utc.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// </returns>
        public static System.DateTime ConvertToUtc(System.DateTime date)
        {
            return date.Kind == DateTimeKind.Utc ? date : new System.DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, DateTimeKind.Utc);
        }

        /// <summary>
        /// The parse time.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <returns>
        /// The parse time.
        /// </returns>
        public static bool ParseTime(string s, out System.DateTime time)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(s.Trim()))
            {
                time = System.DateTime.MinValue;
                return false;
            }

            if (!s.ToUpper().EndsWith("Z"))
                s = string.Concat(s.ToUpper(), "Z");
            return ParseDate(s, out time);
        }

        /// <summary>
        /// The merge.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static System.DateTime Merge(string date, string time)
        {
            var isSuccessfulDateParsing = false;

            var isSuccessfulTimeParsing = false;

            System.DateTime datePortion;
            if (ParseDate(date, out datePortion))
            {
                isSuccessfulDateParsing = true;
            }

            System.DateTime timePortion;
            if (ParseTime(time, out timePortion))
            {
                isSuccessfulTimeParsing = true;
            }

            if (isSuccessfulTimeParsing && isSuccessfulDateParsing)
            {
                return new System.DateTime(datePortion.Year, datePortion.Month, datePortion.Day, timePortion.Hour, timePortion.Minute, timePortion.Second, DateTimeKind.Utc);
            }

            if (isSuccessfulDateParsing)
            {
                return new System.DateTime(datePortion.Year, datePortion.Month, datePortion.Day, datePortion.Hour, datePortion.Minute, datePortion.Second, DateTimeKind.Utc);
            }

            throw new ArgumentException(UnableToParseInputString);
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The to string.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "\nPreferenceValue:{0}\nTimeZone:{1}\nAdjustToDaylightSavingsTime:{2}\nConvertToLocalTime:{3}\nInterfaceLanguage:{4}\nClockType:{5}", 
                preference, 
                timeZoneBuilder.UITimeZone, 
                timeZoneBuilder.AdjustToDaylightSavingsTime, 
                timeZoneBuilder.ConvertToLocalTime, 
                internalInterfaceLanguageCode, 
                clockType);
        }

        #region << Clock Time/DateTime Mappers >>

        /// <summary>
        /// The map to time clock pattern.
        /// </summary>
        /// <returns>
        /// The map to time clock pattern.
        /// </returns>
        private string MapToTimeClockPattern()
        {
            switch (clockType)
            {
                case ClockType.TwentyFourHours:
                    return regionalCulture.TimeTwentyFourHourClockPattern;
                default:
                    return regionalCulture.TimeTwelveHourClockPattern;
            }
        }

        /// <summary>
        /// The map to time gmt clock pattern.
        /// </summary>
        /// <returns>
        /// The map to time gmt clock pattern.
        /// </returns>
        private string MapToTimeGMTClockPattern()
        {
            switch (clockType)
            {
                case ClockType.TwentyFourHours:
                    return regionalCulture.TimeTwentyFourHourGMTClockPattern;
                default:
                    return regionalCulture.TimeTwelveHourGMTClockPattern;
            }
        }

        /// <summary>
        /// The map to standard date time clock pattern.
        /// </summary>
        /// <returns>
        /// The map to standard date time clock pattern.
        /// </returns>
        private string MapToStandardDateTimeClockPattern()
        {
            switch (clockType)
            {
                case ClockType.TwentyFourHours:
                    return regionalCulture.DateTimeTwentyFourHourStandardDateTimePattern;
                default:
                    return regionalCulture.DateTimeTwelveFourHourStandardDateTimePattern;
            }
        }

        /// <summary>
        /// The map to standard date time gmt clock pattern.
        /// </summary>
        /// <returns>
        /// The map to standard date time gmt clock pattern.
        /// </returns>
        private string MapToStandardDateTimeGMTClockPattern()
        {
            switch (clockType)
            {
                case ClockType.TwentyFourHours:
                    return regionalCulture.DateTimeTwentyFourHourGMTStandardDateTimePattern;
                default:
                    return regionalCulture.DateTimeTwelveFourHourGMTStandardDateTimePattern;
            }
        }

        /// <summary>
        /// The map to standard headline date time clock pattern.
        /// </summary>
        /// <returns>
        /// The map to standard headline date time clock pattern.
        /// </returns>
        private string MapToStandardHeadlineDateTimeClockPattern()
        {
            switch (clockType)
            {
                case ClockType.TwentyFourHours:
                    return regionalCulture.DateTimeTwentyFourHourStandardHeadlineDateTimePattern;
                default:
                    return regionalCulture.DateTimeTwelveFourHourStandardHeadlineDateTimePattern;
            }
        }

        /// <summary>
        /// The map to standard headline date time gmt clock pattern.
        /// </summary>
        /// <returns>
        /// The map to standard headline date time gmt clock pattern.
        /// </returns>
        private string MapToStandardHeadlineDateTimeGMTClockPattern()
        {
            switch (clockType)
            {
                case ClockType.TwentyFourHours:
                    return regionalCulture.DateTimeTwentyFourHourGMTStandardHeadlineDateTimePattern;
                default:
                    return regionalCulture.DateTimeTwelveFourHourGMTStandardHeadlineDateTimePattern;
            }
        }

        /// <summary>
        /// The map to long date time clock pattern.
        /// </summary>
        /// <returns>
        /// The map to long date time clock pattern.
        /// </returns>
        private string MapToLongDateTimeClockPattern()
        {
            switch (clockType)
            {
                case ClockType.TwentyFourHours:
                    return regionalCulture.DateTimeTwentyFourHourLongDateTimePattern;
                default:
                    return regionalCulture.DateTimeTwelveFourHourLongDateTimePattern;
            }
        }

        /// <summary>
        /// The map to long date time gmt clock pattern.
        /// </summary>
        /// <returns>
        /// The map to long date time gmt clock pattern.
        /// </returns>
        private string MapToLongDateTimeGMTClockPattern()
        {
            switch (clockType)
            {
                case ClockType.TwentyFourHours:
                    return regionalCulture.DateTimeTwentyFourHourGMTLongDateTimePattern;
                default:
                    return regionalCulture.DateTimeTwelveFourHourGMTLongDateTimePattern;
            }
        }

        /// <summary>
        /// The map to short date time clock pattern.
        /// </summary>
        /// <returns>
        /// The map to short date time clock pattern.
        /// </returns>
        private string MapToShortDateTimeClockPattern()
        {
            switch (clockType)
            {
                case ClockType.TwentyFourHours:
                    return regionalCulture.DateTimeTwentyFourHourShortDateTimePattern;
                default:
                    return regionalCulture.DateTimeTwelveFourHourShortDateTimePattern;
            }
        }

        /// <summary>
        /// The map to short date time gmt clock pattern.
        /// </summary>
        /// <returns>
        /// The map to short date time gmt clock pattern.
        /// </returns>
        private string MapToShortDateTimeGMTClockPattern()
        {
            switch (clockType)
            {
                case ClockType.TwentyFourHours:
                    return regionalCulture.DateTimeTwentyFourHourGMTShortDateTimePattern;
                default:
                    return regionalCulture.DateTimeTwelveFourHourGMTShortDateTimePattern;
            }
        }

        #endregion

        #region << FormatShortDateTime >>

        /// <summary>
        /// Cores the format Short date time.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The core format short date time.
        /// </returns>
        private string CoreFormatShortDateTime(System.DateTime dt)
        {
            var temp = ConvertToUtc(dt);
            if (timeZoneBuilder.ConvertToLocalTime)
            {
                temp = timeZoneBuilder.UITimeZone.ToLocalTime(temp, timeZoneBuilder.AdjustToDaylightSavingsTime);
                return temp.ToString(MapToShortDateTimeClockPattern(), cultureInfo);
            }

            return temp.ToString(MapToShortDateTimeGMTClockPattern(), cultureInfo);
        }

        /// <summary>
        /// Formats the Short date time.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The format short date time.
        /// </returns>
        public string FormatShortDateTime(string s)
        {
            System.DateTime dt;
            return ParseDate(s, out dt) ? FormatDateTime(dt) : CoreFormatShortDateTime(System.DateTime.MinValue);
        }

        /// <summary>
        /// Formats the Short date time.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <returns>
        /// The format short date time.
        /// </returns>
        public string FormatShortDateTime(string date, string time)
        {
            var dt = Merge(date, time);
            return CoreFormatShortDateTime(dt);
        }

        /// <summary>
        /// Formats the Short date time.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// The format short date time.
        /// </returns>
        public string FormatShortDateTime(System.DateTime dateTime)
        {
            return CoreFormatShortDateTime(dateTime);
        }

        #endregion

        #region << FormatLongDateTime >> 

        /// <summary>
        /// Cores the format long date time.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The core format long date time.
        /// </returns>
        private string CoreFormatLongDateTime(System.DateTime dt)
        {
            var temp = ConvertToUtc(dt);
            if (timeZoneBuilder.ConvertToLocalTime)
            {
                temp = timeZoneBuilder.UITimeZone.ToLocalTime(temp, timeZoneBuilder.AdjustToDaylightSavingsTime);
                return temp.ToString(MapToLongDateTimeClockPattern(), cultureInfo);
            }

            return temp.ToString(MapToLongDateTimeGMTClockPattern(), cultureInfo);
        }

        /// <summary>
        /// Formats the long date time.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The format long date time.
        /// </returns>
        public string FormatLongDateTime(string s)
        {
            System.DateTime dt;
            return ParseDate(s, out dt) ? FormatDateTime(dt) : CoreFormatLongDateTime(System.DateTime.MinValue);
        }

        /// <summary>
        /// Formats the long date time.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <returns>
        /// The format long date time.
        /// </returns>
        public string FormatLongDateTime(string date, string time)
        {
            var dt = Merge(date, time);
            return CoreFormatLongDateTime(dt);
        }

        /// <summary>
        /// Formats the long date time.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// The format long date time.
        /// </returns>
        public string FormatLongDateTime(System.DateTime dateTime)
        {
            return CoreFormatLongDateTime(dateTime);
        }

        #endregion

        #region << FormatStandardHeadlineDateTime >>

        /// <summary>
        /// Formats the standard date time.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The format standard headline date time.
        /// </returns>
        public string FormatStandardHeadlineDateTime(string s)
        {
            System.DateTime dt;
            return ParseDate(s, out dt) ? FormatDateTime(dt) : CoreStandardHeadlineFormatDateTime(System.DateTime.MinValue);
        }

        /// <summary>
        /// Formats the standard date time.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <returns>
        /// The format standard headline date time.
        /// </returns>
        public string FormatStandardHeadlineDateTime(string date, string time)
        {
            var dt = Merge(date, time);
            return FormatStandardHeadlineDateTime(dt);
        }

        /// <summary>
        /// Formats the standard date time.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <returns>
        /// The format standard headline date time.
        /// </returns>
        public string FormatStandardHeadlineDateTime(System.DateTime date, System.DateTime time)
        {
            var dt = Merge(date, time);
            return FormatStandardHeadlineDateTime(dt);
        }
        
        /// <summary>
        /// Formats the standard date time.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// The format standard headline date time.
        /// </returns>
        public string FormatStandardHeadlineDateTime(System.DateTime dateTime)
        {
            return CoreStandardHeadlineFormatDateTime(dateTime);
        }

        #endregion

        #region << FormatStandardDateTime >>

        /// <summary>
        /// Formats the standard date time.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The format standard date time.
        /// </returns>
        public string FormatStandardDateTime(string s)
        {
            System.DateTime dt;
            return ParseDate(s, out dt) ? FormatDateTime(dt) : CoreFormatDateTime(System.DateTime.MinValue);
        }

        /// <summary>
        /// Formats the standard date time.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <param name="time">
        /// The time.
        /// </param>
        /// <returns>
        /// The format standard date time.
        /// </returns>
        public string FormatStandardDateTime(string date, string time)
        {
            var dt = Merge(date, time);
            return FormatDateTime(dt);
        }

        /// <summary>
        /// Formats the standard date time.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// The format standard date time.
        /// </returns>
        public string FormatStandardDateTime(System.DateTime dateTime)
        {
            return CoreFormatDateTime(dateTime);
        }

        #endregion

        #region << FormatDateTime >>

        /// <summary>
        /// Formats the time.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The core format date time.
        /// </returns>
        private string CoreFormatDateTime(System.DateTime dt)
        {
            var temp = ConvertToUtc(dt);
            if (timeZoneBuilder.ConvertToLocalTime)
            {
                temp = timeZoneBuilder.UITimeZone.ToLocalTime(temp, timeZoneBuilder.AdjustToDaylightSavingsTime);
                return temp.ToString(MapToStandardDateTimeClockPattern(), cultureInfo);
            }

            return temp.ToString(MapToStandardDateTimeGMTClockPattern(), cultureInfo);
        }
        
        /// <summary>
        /// Formats the time.
        /// </summary>
        /// <param name="dt">
        /// The dt.
        /// </param>
        /// <returns>
        /// The core standard headline format date time.
        /// </returns>
        private string CoreStandardHeadlineFormatDateTime(System.DateTime dt)
        {
            var temp = ConvertToUtc(dt);
            if (timeZoneBuilder.ConvertToLocalTime)
            {
                temp = timeZoneBuilder.UITimeZone.ToLocalTime(temp, timeZoneBuilder.AdjustToDaylightSavingsTime);
                return temp.ToString(MapToStandardHeadlineDateTimeClockPattern(), cultureInfo);
            }

            return temp.ToString(MapToStandardHeadlineDateTimeGMTClockPattern(), cultureInfo);
        }

        /// <summary>
        /// Formats the date time.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The format date time.
        /// </returns>
        public string FormatDateTime(string s)
        {
            System.DateTime dt;
            return ParseDate(s, out dt) ? FormatDateTime(dt) : CoreFormatDateTime(System.DateTime.MinValue);
        }

        /// <summary>
        /// Formats the date time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="time">The time.</param>
        /// <returns>
        /// The format date time.
        /// </returns>
        public string FormatDateTime(string date, string time)
        {
            var dt = Merge(date, time);
            return FormatDateTime(dt);
        }
        
        /// <summary>
        /// Formates the date time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// The format date time.
        /// </returns>
        public string FormatDateTime(System.DateTime dateTime)
        {
            return CoreFormatDateTime(dateTime);
        }

        #endregion
    }
}