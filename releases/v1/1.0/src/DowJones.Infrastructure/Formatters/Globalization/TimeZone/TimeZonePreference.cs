// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeZonePreference.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace DowJones.Utilities.Formatters.Globalization.TimeZone
{
    internal class TimeZoneBuilder
    {
        private bool adjustToDaylightSavingsTime;
        private bool convertToLocalTime;
        private UITimeZone uiTimeZone = (UITimeZone)TimeZoneManager.GmtTimeZone;
        private System.TimeZone timeZone = TimeZoneManager.GmtTimeZone;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeZoneBuilder"/> class.
        /// </summary>
        public TimeZoneBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeZoneBuilder"/> class.
        /// </summary>
        /// <param name="serializedPreference">The serialized preference.</param>
        public TimeZoneBuilder(string serializedPreference)
        {
            Parse(serializedPreference);
        }

        public System.TimeZone TimeZone
        {
            get
            {
                return timeZone;
            }

            set
            {
                if (!(value is UITimeZone))
                {
                    return;
                }

                timeZone = value;
                uiTimeZone = (UITimeZone)value;
            }
        }

        public UITimeZone UITimeZone
        {
            get { return uiTimeZone; }
        }

        public bool AdjustToDaylightSavingsTime
        {
            get { return adjustToDaylightSavingsTime; }
            set { adjustToDaylightSavingsTime = value; }
        }

        public bool ConvertToLocalTime
        {
            get { return convertToLocalTime; }
            set { convertToLocalTime = value; }
        }

        /// <summary>
        /// Parses the specified serialized.
        /// </summary>
        /// <param name="serializedPreference">The serialized.</param>
        private void Parse(string serializedPreference)
        {
            var strA = serializedPreference.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string timeZoneCode = null;
            switch (strA.Length)
            {
                case 1:
                    convertToLocalTime = false;
                    timeZoneCode = strA[0].Trim();
                    adjustToDaylightSavingsTime = false;
                    break;
                case 2:
                    if (strA[0].Trim().ToLower() == "on")
                    {
                        convertToLocalTime = true;
                        timeZoneCode = strA[1].Trim();
                        adjustToDaylightSavingsTime = false;
                    }
                    else
                    {
                        convertToLocalTime = false;
                        timeZoneCode = strA[0].Trim();
                        adjustToDaylightSavingsTime = (strA[1].Trim().ToLower()).Equals("on");
                    }

                    break;
                default:
                    if (strA.Length > 2)
                    {
                        convertToLocalTime = strA[0].Trim().ToLower().Equals("on");
                        timeZoneCode = strA[1].Trim();
                        adjustToDaylightSavingsTime = strA[2].Trim().ToLower().Equals("on");
                    }

                    break;
            }

            if (!string.IsNullOrEmpty(timeZoneCode) && !string.IsNullOrEmpty(timeZoneCode.Trim()))
            {
                if (convertToLocalTime)
                {
                    TimeZone = UITimeZone.GetUITimeZoneUsingFactivaCode(timeZoneCode);
                    return;
                }
            }

            TimeZone = TimeZoneManager.GmtTimeZone;
        }
    }
}
