// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwitterDateTime.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   Represents the possible known date formats that Twitter reports.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization; 
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;

namespace DowJones.Infrastructure.Models.SocialMedia
{
    /// <summary>
    /// Represents a normalized date from the Twitter API.
    /// </summary>
    [Serializable]
    [DataContract]
    public class TwitterDateTime
    {
        #region Constants and Fields

        /// <summary>
        /// The _map.
        /// </summary>
        private static readonly IDictionary<string, string> Map = new Dictionary<string, string>();

        /// <summary>
        /// The _reader writer lock.
        /// </summary>
        private static readonly ReaderWriterLockSlim ReaderWriterLock = new ReaderWriterLockSlim();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterDateTime"/> class.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        public TwitterDateTime(DateTime dateTime, TwitterDateFormat format)
        {
            this.Format = format;
            this.DateTime = dateTime;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the actual date time.
        /// </summary>
        /// <value>The date time.</value>
        public virtual DateTime DateTime { get; private set; }

        /// <summary>
        ///   Gets the Twitter-based date format.
        /// </summary>
        /// <value>The format.</value>
        public virtual TwitterDateFormat Format { get; private set; }

        /// <summary>
        ///   The source content used to de-serialize the model entity instance.
        ///   Can be XML or JSON, depending on the endpoint used.
        /// </summary>
        [DataMember]
        public virtual string RawSource { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts from date time.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        /// The convert from date time.
        /// </returns>
        public static string ConvertFromDateTime(DateTime input, TwitterDateFormat format)
        {
            EnsureDateFormatsAreMapped();
            var name = Enum.GetName(typeof(TwitterDateFormat), format);
            GetReadLockOnMap();
            var value = Map[name];
            ReleaseReadLockOnMap();

            value = value.Replace(" zzzzz", " +0000");

            var converted = input.ToString(value, CultureInfo.InvariantCulture);
            return converted;
        }

        /// <summary>
        /// Converts to date time.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(string input)
        {
            EnsureDateFormatsAreMapped();
            GetReadLockOnMap();
            var formats = Map.Values;
            ReleaseReadLockOnMap();
            foreach (var format in formats)
            {
                DateTime date;
                if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date))

                {
                    return date;
                }
            }

            return default(DateTime);
        }

        /// <summary>
        /// Converts to twitter date time.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static TwitterDateTime ConvertToTwitterDateTime(string input)
        {
            EnsureDateFormatsAreMapped();
            GetReadLockOnMap();
            try
            {
                foreach (var format in Map)
                {
                    DateTime date;
                    if (!DateTime.TryParseExact(input, format.Value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date))
                    {
                        continue;
                    }

                    var kind = Enum.Parse(typeof(TwitterDateFormat), format.Key, true);
                    return new TwitterDateTime(date, (TwitterDateFormat)kind);
                }

                return default(TwitterDateTime);
            }
            finally
            {
                ReleaseReadLockOnMap();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ConvertFromDateTime(this.DateTime, this.Format);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The ensure date formats are mapped.
        /// </summary>
        private static void EnsureDateFormatsAreMapped()
        {
            var type = typeof(TwitterDateFormat);
            var names = Enum.GetNames(type);
            GetReadLockOnMap();
            try
            {
                foreach (var name in names.Where(name => !Map.ContainsKey(name)))
                {
                    GetWriteLockOnMap();
                    try
                    {
                        var fi = typeof(TwitterDateFormat).GetField(name);
                        var attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        var format = (DescriptionAttribute)attributes[0];

                        Map.Add(name, format.Description);
                    }
                    finally
                    {
                        ReleaseWriteLockOnMap();
                    }
                }
            }
            finally
            {
                ReleaseReadLockOnMap();
            }
        }

        /// <summary>
        /// The get read lock on map.
        /// </summary>
        private static void GetReadLockOnMap()
        {
            ReaderWriterLock.EnterUpgradeableReadLock();
        }

        /// <summary>
        /// The get write lock on map.
        /// </summary>
        private static void GetWriteLockOnMap()
        {
            ReaderWriterLock.EnterWriteLock();
        }

        /// <summary>
        /// The release read lock on map.
        /// </summary>
        private static void ReleaseReadLockOnMap()
        {
            ReaderWriterLock.ExitUpgradeableReadLock();
        }

        /// <summary>
        /// The release write lock on map.
        /// </summary>
        private static void ReleaseWriteLockOnMap()
        {
            ReaderWriterLock.ExitWriteLock();
        }

        #endregion
    }

    /// <summary>
    /// Represents the possible known date formats that Twitter reports.
    /// </summary>
    public enum TwitterDateFormat
    {
        /// <summary>
        ///   RestApi option.
        /// </summary>
        [Description("ddd MMM dd HH:mm:ss zzzzz yyyy")]
        RestApi,

        /// <summary>
        ///   SuliaRestApi option.
        /// </summary>
        [Description("ddd MMM dd HH:mm:ss yyyy")]
        SuliaRestApi,

        /// <summary>
        ///  SearchApi  option.
        /// </summary>
        [Description("ddd, dd MMM yyyy HH:mm:ss zzzzz")]
        SearchApi,

        /// <summary>
        ///   Atom option.
        /// </summary>
        [Description("yyyy-MM-ddTHH:mm:ssZ")]
        Atom,

        /// <summary>
        ///   XmlHashesAndRss option.
        /// </summary>
        [Description("yyyy-MM-ddTHH:mm:sszzzzzz")]
        XmlHashesAndRss,

        /// <summary>
        ///   TrendsCurrent option.
        /// </summary>
        [Description("ddd MMM dd HH:mm:ss zzzzz yyyy")]
        TrendsCurrent,

        /// <summary>
        ///   TrendsDaily option.
        /// </summary>
        [Description("yyyy-MM-dd HH:mm")]
        TrendsDaily,

        /// <summary>
        ///   TrendsWeekly option.
        /// </summary>
        [Description("yyyy-MM-dd")]
        TrendsWeekly
    }   
}