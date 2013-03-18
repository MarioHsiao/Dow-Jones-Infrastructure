// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeZoneManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Provides the list of registered time zones.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;
using Microsoft.Win32;

namespace DowJones.Formatters.Globalization.TimeZone
{
    /// <summary>
    /// Provides the list of registered time zones.
    /// </summary>
    public static class TimeZoneManager
    {
        /// <summary>
        /// The registry key to use when accessing the time zone database.
        /// </summary>
        internal const string WIN32_NT_TIME_ZONE_DATABASE_KEY = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones";

        internal const string WIN32_WINDOWS_TIME_ZONE_DATABASE_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Time Zones\";
        
        /// <summary>
        /// An assumed standard name of the GMT time zone.
        /// </summary>
        private const string GmtZoneName = "GMT Standard Time";
        
        /// <summary>
        /// The Greenwich Mean Time time zone.
        /// </summary>
        private static readonly UITimeZone gmtTimeZone;

        /// <summary>
        /// The collection of supported registered time zones.
        /// </summary>
        private static readonly TimeZoneCollection supportedTimeZones;

        private static readonly ILog Log = LogManager.GetLogger(typeof(TimeZoneManager));

        /// <summary>
        /// The collection of registered time zones.
        /// </summary>
        private static readonly TimeZoneCollection timeZones;

        private static readonly List<UITimeZone> SortedTimeZones = new List<UITimeZone>();

        /// <summary>
        /// Initializes static members of the <see cref="TimeZoneManager"/> class.
        /// </summary>
        static TimeZoneManager()
        {
            // get a reference to the key that contains all of the timezone information
            // on the current machine
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    using (var key = Registry.LocalMachine.OpenSubKey(WIN32_NT_TIME_ZONE_DATABASE_KEY))
                    {
                        timeZones = new TimeZoneCollection(key);
                        if (timeZones.Count > 0)
                        {
                            supportedTimeZones = new TimeZoneCollection();
                            foreach (UITimeZone zone in timeZones)
                            {
                                var item = TimeZoneMapper.Instance.GetTimeZoneItemByStandardName(zone.StandardName);
                                if (item == null)
                                {
                                    if (Log.IsDebugEnabled)
                                    {
                                        Log.DebugFormat("Unable to find TimeZoneItem: {0}", zone.StandardName);
                                    }

                                    continue;
                                }
                                    
                                zone.FactivaCode = item.Code;
                                zone.AlternateName = item.AlternateName;
                                supportedTimeZones.Add(zone);
                            }
                        }
                    }

                    break;
                case PlatformID.Win32Windows:
                    using (var key = Registry.LocalMachine.OpenSubKey(WIN32_WINDOWS_TIME_ZONE_DATABASE_KEY))
                    {
                        timeZones = new TimeZoneCollection(key);
                        if (timeZones.Count > 0)
                        {
                            supportedTimeZones = new TimeZoneCollection();
                            foreach (UITimeZone zone in timeZones)
                            {
                                var item = TimeZoneMapper.Instance.GetTimeZoneItemByStandardName(zone.StandardName);
                                if (item == null)
                                {
                                    if (Log.IsDebugEnabled)
                                    {
                                        Log.DebugFormat("Unable to find TimeZoneItem: {0}", zone.StandardName);
                                    }

                                    continue;
                                }

                                zone.FactivaCode = item.Code;
                                zone.AlternateName = item.AlternateName;
                                supportedTimeZones.Add(zone);
                            }
                        }
                    }

                    break;
                default:
                    throw new PlatformNotSupportedException();
            }

            // populate the sorted list of timezone
            foreach (var item in TimeZoneMapper.Instance.TimeZoneItemList)
            {
                UITimeZone temp = null; 
                try
                {
                    temp = (UITimeZone)supportedTimeZones[item.Code];
                }
                catch (Exception ex)
                {
                    if (Log.IsDebugEnabled)
                    {
                        Log.DebugFormat("Unable to find UITimeZone by code: {0} Exception: {1}", item.Code, ex);
                    }
                }

                if (temp != null && !SortedTimeZones.Contains(temp))
                {
                    SortedTimeZones.Add(temp);
                }
            }

            // Get the Greenwich time zone
            try
            {
                gmtTimeZone = (UITimeZone)timeZones[GmtZoneName];
            }
            catch (KeyNotFoundException)
            {
                gmtTimeZone = null;
            }

            if (gmtTimeZone != null)
            {
                return;
            }

            // Try to find the zone sequentially
            foreach (var zone in timeZones.Cast<UITimeZone>().Where(zone => (zone.StandardName.StartsWith("GMT") || zone.ToString().Contains("Greenwich"))))
            {
                gmtTimeZone = zone;
                break;
            }
        }

        /// <summary>
        /// Gets the collection of registered time zones.
        /// </summary>
        public static TimeZoneCollection TimeZones
        {
            [DebuggerStepThrough]
            get { return timeZones; }
        }

        /// <summary>
        /// Gets the collection of registered time zones.
        /// </summary>
        public static TimeZoneCollection SupportedTimeZones
        {
            [DebuggerStepThrough]
            get { return supportedTimeZones; }
        }

        public static List<UITimeZone> SortedSupportedTimeZones
        {
            get { return SortedTimeZones; }
        }

        /// <summary>
        /// Gets the Greenwich Mean Time time zone.
        /// </summary>
        public static System.TimeZone GmtTimeZone
        {
            [DebuggerStepThrough]
            get { return gmtTimeZone; }
        }
    }
}