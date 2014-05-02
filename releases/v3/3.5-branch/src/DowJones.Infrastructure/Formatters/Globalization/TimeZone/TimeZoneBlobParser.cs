// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeZonePreference.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace DowJones.Formatters.Globalization.TimeZone
{
    public class TimeZoneFields
    {
        public System.TimeZone TimeZone { get; set; }
        public bool AdjustToDaylightSavingsTime { get; set; }
        public bool ConvertToLocalTime { get; set; }
        public string TimeZoneOffset { get; set; }
    }

    public class TimeZoneBlobParser
    {

        public static TimeZoneFields Parse(string time)
        {
            var timeZone = new TimeZoneBuilder(time);
            var timeZoneOffset = timeZone.UITimeZone.FactivaCode.Split('|')[0];
            return new TimeZoneFields
                       {
                           TimeZone = timeZone.TimeZone,
                           AdjustToDaylightSavingsTime = timeZone.AdjustToDaylightSavingsTime,
                           ConvertToLocalTime = timeZone.ConvertToLocalTime,
                           TimeZoneOffset = timeZoneOffset
                       };
        }
    }
}
