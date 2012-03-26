using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace DowJones.Formatters.Globalization.TimeZone
{
    /// <summary>
    /// Represents a time zone used for the user interface.
    /// </summary>
    [Serializable]
    public class UITimeZone : System.TimeZone, IComparable<System.TimeZone>, IComparable, IDeserializationCallback
    {
        #region Current UI time zone

        /// <summary>
        /// Returns a <see cref="UITimeZone"/> instance representing
        /// an equivalent to the specified time zone.
        /// </summary>
        /// <param name="timeZone">The time zone to convert.</param>
        public static UITimeZone GetUITimeZone(System.TimeZone timeZone)
        {
            if (timeZone is UITimeZone)
                return (UITimeZone) timeZone;
            try
            {
                return (UITimeZone) TimeZoneManager.TimeZones[timeZone.StandardName];
            }
            catch (KeyNotFoundException)
            {
                return (UITimeZone) TimeZoneManager.GmtTimeZone;
            }
        }

        /// <summary>
        /// Returns a <see cref="UITimeZone"/> instance representing
        /// an equivalent to the specified time zone.
        /// </summary>
        /// <param name="standardName">Standard time zone name.</param>
        /// <returns></returns>
        public static UITimeZone GetUITimeZone(string standardName)
        {
            try
            {
                return (UITimeZone) TimeZoneManager.TimeZones[standardName];
            }
            catch (KeyNotFoundException)
            {
                return (UITimeZone) TimeZoneManager.GmtTimeZone;
            }
        }

        /// <summary>
        /// Returns a <see cref="UITimeZone"/> instance representing
        /// an equivalent to the specified time zone.
        /// </summary>
        /// <param name="timeZoneCode">The timezone code.</param>
        /// <returns></returns>
        public static UITimeZone GetUITimeZoneUsingFactivaCode(string timeZoneCode)
        {
            var item = TimeZoneMapper.Instance.GetTimeZoneItemByFactivaCode(timeZoneCode);
            if (item != null)
            {
                try
                {
                    return (UITimeZone) TimeZoneManager.SupportedTimeZones[item.StandardName];
                }
                catch (KeyNotFoundException)
                {
                    return (UITimeZone) TimeZoneManager.GmtTimeZone;
                }
            }
            return (UITimeZone) TimeZoneManager.GmtTimeZone;
        }

        #endregion

        // TODO: Implement your own logic of retrieving and updating the current UI time zone for a specific user.

        /// <summary>
        /// The regular expression to use when parsing
        /// the display name of a time zone.
        /// </summary>
        private static readonly Regex displayNameRegex =
            new Regex(@"\(GMT(?:[\+\-]\d\d\:\d\d)?\)\s(?<Dscr>.*)",
                      RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// The actual UTC bias specification.
        /// </summary>
        private readonly BiasSettings _actualSettings;

        /// <summary>
        /// The daylight saving time zone name.
        /// </summary>
        private readonly string _daylightName;

        /// <summary>
        /// Representative cities belonging to the time zone
        /// or other characteristic description of the zone.
        /// </summary>
        private readonly string _description;

        /// <summary>
        /// The <see cref="String"/> used when
        /// displaying the time zone to the user.
        /// </summary>
        private readonly string _displayName;

        /// <summary>
        /// The first year in the dynamic DST list
        /// </summary>
        private readonly int _firstDaylightEntry;

        /// <summary>
        /// The system index of the time zone.
        /// </summary>
        private readonly uint _index;

        /// <summary>
        /// Determines whether the time zone uses different dates
        /// for the daylight saving period in different years.
        /// </summary>
        private readonly bool _isDaylightDynamic;

        /// <summary>
        /// The last year in the dynamic DST list
        /// </summary>
        private readonly int _lastDaylightEntry;

        /// <summary>
        /// The standard time zone name.
        /// </summary>
        private readonly string _standardName;

        /// <summary>
        /// The collection of previously calculated daylight changes.
        /// </summary>
        [NonSerialized] private Dictionary<int, DaylightTime> _cachedDaylightChanges;

        private string _factivaCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="UITimeZone"/> class.
        /// </summary>
        /// <param name="name">The name of the time zone
        /// as recorded in the Windows registry.</param>
        /// <param name="data">The time zone's registry data.</param>
        internal UITimeZone(string name, RegistryKey data)
        {
            _cachedDaylightChanges = new Dictionary<int, DaylightTime>();

            // Convert the signed number obtained from the registry
            // to the unsigned value of the system index.
            var indexValue = (int?) data.GetValue("Index");
            if (indexValue.HasValue)
            {
                _index = (((uint) (indexValue.Value >> 16)) << 16) |
                         ((uint) (indexValue.Value & 0xFFFF));
            }

            // Get the display name of the time zone.
            _displayName = (string) data.GetValue("Display");
            var m = displayNameRegex.Match(_displayName);
            _description = m.Groups["Dscr"].Value;

            // Get other descriptive data of the time zone
            _standardName = (string) data.GetValue("Std");
            _daylightName = (string) data.GetValue("Dlt");
            _actualSettings = new BiasSettings(
                (byte[]) data.GetValue("TZI"));

            // Check whether the time zone uses dynamic DST
            var subKeyNames = data.GetSubKeyNames();
            foreach (var subKeyName in subKeyNames.Where(subKeyName => subKeyName == "Dynamic DST"))
            {
                _isDaylightDynamic = true;
                using (var subKey = data.OpenSubKey(subKeyName))
                {
                    if (subKey != null)
                    {
                        _firstDaylightEntry = (int) subKey.GetValue("FirstEntry");
                        _lastDaylightEntry = (int) subKey.GetValue("LastEntry");
                    }
                }
                if (name != null && _standardName != name) // it should not be
                {
                    _standardName = name; // is used when accessing the registry
                }
                return;
            }
            _isDaylightDynamic = false;
            _firstDaylightEntry = int.MinValue;
            _lastDaylightEntry = int.MaxValue;
        }


        /// <summary>
        /// Gets the system index of the time zone.
        /// </summary>
        internal uint Index
        {
            [DebuggerStepThrough]
            get { return _index; }
        }

        /// <summary>
        /// Gets the standard time zone name.
        /// </summary>
        public override string StandardName
        {
            [DebuggerStepThrough]
            get { return _standardName; }
        }

        /// <summary>
        /// Gets the daylight saving time zone name.
        /// </summary>
        public override string DaylightName
        {
            get
            {
                return string.IsNullOrEmpty(_daylightName) ? _standardName : _daylightName;
            }
        }

        public string FactivaCode
        {
            get { return _factivaCode; }
            set { if (string.IsNullOrEmpty(_factivaCode)) _factivaCode = value; }
        }

        #region IComparable Members

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A 32-bit integer that indicates the relative order
        /// of the entities being compared.</returns>
        /// <exception cref="ArgumentException"><paramref name="other"/>
        /// is not the same type as this instance.</exception>
        int IComparable.CompareTo(object other)
        {
            if (other == null) return 1;
            if (!GetType().IsInstanceOfType(other))
                throw new ArgumentException(null, "other");
            var otherZone = (UITimeZone) other;
            var thisBias = _actualSettings.ZoneBias;
            var otherBias = otherZone._actualSettings.ZoneBias;
            return thisBias == otherBias ? StringComparer.InvariantCultureIgnoreCase.Compare(_description, otherZone._description) : thisBias.CompareTo(otherBias);
        }

        #endregion

        #region IComparable<TimeZone> Members

        int IComparable<System.TimeZone>.CompareTo(System.TimeZone other)
        {
            return ((IComparable) this).CompareTo(other);
        }

        #endregion

        #region IDeserializationCallback Members

        /// <summary>
        /// Initializes the non-serialized members.
        /// </summary>
        void IDeserializationCallback.OnDeserialization(object sender)
        {
            _cachedDaylightChanges = new Dictionary<int, DaylightTime>();
        }

        #endregion

        /// <summary>
        /// Returns the daylight saving time period for a particular year.
        /// </summary>
        /// <param name="year">The year to which
        /// the daylight saving time period applies.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="year"/> is less than 1 or greater than 9999.</exception>
        public override DaylightTime GetDaylightChanges(int year)
        {
            if ((year < 1) || (year > 9999))
                throw new ArgumentOutOfRangeException("year");

            lock (_cachedDaylightChanges)
            {
                DaylightTime result;
                if (_cachedDaylightChanges.TryGetValue(year, out result))
                    return result;
                var settings = GetBiasSettings(year);
                result = settings.GetDaylightChanges(year);
                _cachedDaylightChanges.Add(year, result);
                return result;
            }
        }


        /// <summary>
        /// Returns the coordinated universal time (UTC) offset
        /// for the specified local time.
        /// </summary>
        /// <param name="time">The local date and time.</param>
        public override TimeSpan GetUtcOffset(System.DateTime time)
        {
            return GetUtcOffset(time, true);
        }


        /// <summary>
        /// Returns the coordinated universal time (UTC) offset
        /// for the specified local time.
        /// </summary>
        /// <param name="time">The local date and time.</param>
        /// <param name="adjustToDaylightSavingsTime">if set to <c>true</c> [adjust to daylight savings time].</param>
        /// <returns></returns>
        private TimeSpan GetUtcOffset(System.DateTime time, bool adjustToDaylightSavingsTime)
        {
            if (time.Kind == DateTimeKind.Utc)
                return TimeSpan.Zero;

            var year = time.Year;
            var daylightTimes = GetDaylightChanges(year);
            var settings = GetBiasSettings(year);

            if (adjustToDaylightSavingsTime && IsDaylightSavingTime(time, daylightTimes))
                return settings.ZoneBias + daylightTimes.Delta;
            return settings.ZoneBias;
        }

        /// <summary>
        /// Returns the coordinated universal time (UTC) offset
        /// for the specified universal time.
        /// </summary>
        /// <param name="time">The UTC date and time.</param>
        protected TimeSpan GetUtcOffsetFromUniversalTime(System.DateTime time)
        {
            var year = time.Year;
            var daylightTimes = GetDaylightChanges(year);
            var settings = GetBiasSettings(year);

            var result = settings.ZoneBias;
            if ((daylightTimes == null) || (daylightTimes.Delta.Ticks == 0))
                return result;

            var time3 = daylightTimes.Start - result;
            var time4 = (daylightTimes.End - result) - daylightTimes.Delta;

            bool flag;
            if (time3 > time4)
                flag = (time < time4) || (time >= time3);
            else
                flag = (time >= time3) && (time < time4);
            return flag ? result + daylightTimes.Delta : result;
        }

        /// <summary>
        /// Returns the coordinated universal time (UTC) offset
        /// for the specified universal time.
        /// </summary>
        /// <param name="time">The UTC date and time.</param>
        /// <param name="adjustToDaylightSavingsTime">if set to <c>true</c> [adjust to daylight savings time].</param>
        /// <returns></returns>
        private TimeSpan GetUtcOffsetFromUniversalTime(System.DateTime time, bool adjustToDaylightSavingsTime)
        {
            var compareTime = System.DateTime.MinValue;
            switch (time.Kind)
            {
                case DateTimeKind.Local:
                    compareTime = time;
                    break;
                case DateTimeKind.Unspecified:
                case DateTimeKind.Utc:
                    compareTime = new System.DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, time.Millisecond, DateTimeKind.Local);
                    break;
            }

            var year = compareTime.Year;
            var daylightTimes = GetDaylightChanges(year);
            var settings = GetBiasSettings(year);

            var result = settings.ZoneBias;
            if (!adjustToDaylightSavingsTime || (daylightTimes == null) || (daylightTimes.Delta.Ticks == 0))
            {
                return result;
            }

            var time3 = daylightTimes.Start - result;
            var time4 = (daylightTimes.End - result) - daylightTimes.Delta;

            bool flag;
            if (time3 > time4)
            {
                flag = (compareTime < time4) || (compareTime >= time3);
            }
            else
            {
                flag = (compareTime >= time3) && (compareTime < time4);
            }

            return flag ? result + daylightTimes.Delta : result;
        }

        /// <summary>
        /// Returns the local time that corresponds to
        /// a specified coordinated universal time (UTC).
        /// </summary>
        /// <param name="time">A UTC time.</param>
        public override System.DateTime ToLocalTime(System.DateTime time)
        {
            return ToLocalTime(time, true);
        }

        /// <summary>
        /// Toes the localized time zone time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="adjustToDaylightSavingsTime">if set to <c>true</c> [adjust to daylight savings time].</param>
        /// <returns></returns>
        public System.DateTime ToLocalTime(System.DateTime time, bool adjustToDaylightSavingsTime)
        {
            if (time.Kind == DateTimeKind.Local)
            {
                return time;
            }

            var ticks = time.Ticks + GetUtcOffsetFromUniversalTime(time, adjustToDaylightSavingsTime).Ticks;
            var max = System.DateTime.MaxValue.Ticks;
            if (ticks > max)
            {
                return new System.DateTime(max, DateTimeKind.Local);
            }

            return ticks < 0 ? new System.DateTime(0, DateTimeKind.Local) : new System.DateTime(ticks, DateTimeKind.Local);
        }

        /// <summary>
        /// Returns the time bias specification for the specified year.
        /// </summary>
        /// <param name="year">The year for which the bias applies.</param>
        private BiasSettings GetBiasSettings(int year)
        {
            // Check whether the time zone uses dynamic DST
            if (!_isDaylightDynamic) return _actualSettings;

            if (year < _firstDaylightEntry)
                year = _firstDaylightEntry;
            else if (year > _lastDaylightEntry)
                year = _lastDaylightEntry;

            // Get the collection of dynamic DST data
            Dictionary<int, BiasSettings> years;
            lock (_standardName)
            {
                // TODO: Use caching to increase performance
                //string key = standardName + " Dynamic DST data";
                //years = (Dictionary<int, BiasSettings>)Application.Cache[key];
                years = new Dictionary<int, BiasSettings>();
                //Application.Cache.Add(key, years);
            }

            // Get the DST data for the specified year
            BiasSettings result;
            lock (years)
            {
                if (years.TryGetValue(year, out result))
                    return result;

                var keyName = string.Empty;
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Win32NT:
                        keyName = TimeZoneManager.WIN32_NT_TIME_ZONE_DATABASE_KEY;
                        break;
                    case PlatformID.Win32Windows:
                        keyName = TimeZoneManager.WIN32_NT_TIME_ZONE_DATABASE_KEY;
                        break;
                }
                if (!string.IsNullOrEmpty(keyName))
                {
                    keyName += string.Format(@"\{0}\Dynamic DST", _standardName);
                    using (var key = Registry.LocalMachine.OpenSubKey(keyName))
                    {
                        if (key != null)
                        {
                            var rawData = (byte[]) key.GetValue(year.ToString());
                            result = (rawData == null) ? _actualSettings : new BiasSettings(rawData);
                        }
                    }
                    years.Add(year, result);
                }
            }
            if (result != null)
            {
                return result;
            }
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Returns a <see cref="String"/> representing the time zone.
        /// </summary>
        public override string ToString()
        {
            return _displayName;
        }

        #region Nested type: BiasSettings

        /// <summary>
        /// Provides the UTC offset specification.
        /// </summary>
        [Serializable]
        private class BiasSettings
        {
            /// <summary>
            /// The time zone specification data.
            /// </summary>
            private TimeZoneInfo _data;

            /// <summary>
            /// Initializes a new instance of the <see cref="BiasSettings"/> class.
            /// </summary>
            /// <param name="rawData">The data to use for initialization.</param>
            internal BiasSettings(ICollection<byte> rawData)
            {
                _data = GetTimeZoneInfo(rawData);
            }

            /// <summary>
            /// Gets the basic bias of the time zone.
            /// </summary>
            public TimeSpan ZoneBias
            {
                [DebuggerStepThrough]
                get { return new TimeSpan(0, -_data.Bias, 0); }
            }

            /// <summary>
            /// Gets the Daylight Saving Time bias.
            /// </summary>
            private TimeSpan DaylightBias
            {
                [DebuggerStepThrough]
                get { return new TimeSpan(0, -_data.DaylightBias, 0); }
            }

            /// <summary>
            /// Returns the daylight saving time period for a particular year.
            /// </summary>
            /// <param name="year">The year to which
            /// the daylight saving time period applies.</param>
            public DaylightTime GetDaylightChanges(int year)
            {
                var start = GetParticularDay(year, _data.DaylightDate);
                var end = GetParticularDay(year, _data.StandardDate);
                if ((start == System.DateTime.MinValue) || (end == System.DateTime.MinValue))
                {
                    return new DaylightTime(System.DateTime.MinValue,
                                            System.DateTime.MinValue, TimeSpan.Zero);
                }
                return new DaylightTime(start, end, DaylightBias);
            }

            #region Conversion Routines

            /// <summary>
            /// Returns the particular date in the specified year
            /// on which the daylight saving time period applies.
            /// </summary>
            private static System.DateTime GetParticularDay(int year, DaylightLimit rule)
            {
                if (!rule.Enabled) return System.DateTime.MinValue;

                // Get the first occurence of the requested weekday
                var result = new System.DateTime(year, rule.Month, 1,
                                          rule.Hour, rule.Minute, rule.Second,
                                          rule.Milliseconds, DateTimeKind.Local);
                var diff = rule.DayOfWeek - (int) result.DayOfWeek;
                if (diff < 0) diff += 7;
                result = result.AddDays(diff);

                // Get the day when the daylight time change applies
                result = result.AddDays((rule.Day - 1)*7);
                return result.Month == rule.Month ? result : result.AddDays(-7.0);
            }

            /// <summary>
            /// Interprets raw byte data as a .NET structure.
            /// </summary>
            /// <param name="rawData">The data to interpret.</param>
            private static TimeZoneInfo GetTimeZoneInfo(ICollection<byte> rawData)
            {
                var result = new TimeZoneInfo();
                if (rawData.Count != Marshal.SizeOf(result))
                    throw new ArgumentException(null, "rawData");

                var handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
                try
                {
                    result = (TimeZoneInfo) Marshal.PtrToStructure(
                        handle.AddrOfPinnedObject(), typeof (TimeZoneInfo));
                    return result;
                }
                finally
                {
                    handle.Free();
                }
            }

            #region Nested type: DaylightLimit

            /// <summary>
            /// Represents the standard Windows SYSTEMTIME structure which
            /// is used when defining limits of daylight saving time periods.
            /// </summary>
            [Serializable, StructLayout(LayoutKind.Sequential)]
            private struct DaylightLimit
            {
                public readonly UInt16 Year;
                public readonly UInt16 Month;
                public readonly UInt16 DayOfWeek;
                public readonly UInt16 Day;
                public readonly UInt16 Hour;
                public readonly UInt16 Minute;
                public readonly UInt16 Second;
                public readonly UInt16 Milliseconds;

                /// <summary>
                /// Indicates whether the daylight time limit
                /// represents an applicable DST change.
                /// </summary>
                public bool Enabled
                {
                    [DebuggerStepThrough]
                    get { return Month > 0; }
                }
            }

            #endregion

//end DaylightLimit

            #region Nested type: TimeZoneInfo

            /// <summary>
            /// The layout of the TZI value in the Windows registry.
            /// </summary>
            [Serializable, StructLayout(LayoutKind.Sequential)]
            private struct TimeZoneInfo
            {
                public readonly int Bias;
                public readonly int StandardBias;
                public readonly int DaylightBias;
                public readonly DaylightLimit StandardDate;
                public readonly DaylightLimit DaylightDate;
            }

            #endregion

//end TimeZoneInfo

            #endregion
        }

        #endregion

//end BiasSettings
    }

//end UITimeZone
}

//end Jnw.Samples