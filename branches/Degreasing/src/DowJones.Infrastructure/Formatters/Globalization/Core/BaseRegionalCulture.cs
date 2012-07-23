using System.Globalization;

namespace DowJones.Formatters.Globalization.Core
{
    public abstract class BaseRegionalCulture : ISupportedRegionalCulture
    {
       

        public string Name
        {
            get { return string.Format("{0}-{1}", TwoLetterISOLanguageName, RegionCode); }
        }

        /// <summary>
        /// Gets the short date pattern.
        /// </summary>
        /// <value>The short date pattern.</value>
        protected virtual string shortDatePattern
        {
            get { return "dd-MMM-yyyy"; }
        }

        /// <summary>
        /// Gets the long date pattern.
        /// </summary>
        /// <value>The long date pattern.</value>
        protected virtual string longDatePattern
        {
            get { return "dd MMMM yyyy"; }
        }

        /// <summary>
        /// Gets the standard date pattern.
        /// </summary>
        /// <value>The standard date pattern.</value>
        protected virtual string standardDatePattern
        {
            get { return "d MMMM yyyy"; }
        }

        /// <summary>
        /// Gets the twelve hour clock GMT pattern.
        /// </summary>
        /// <value>The twelve hour clock GMT pattern.</value>
        protected virtual string timeTwelveHourGMTClockPattern
        {
            get { return "h:mm tttt 'GMT'"; }
        }

        /// <summary>
        /// Gets the twenty four hour clock GMT pattern.
        /// </summary>
        /// <value>The twenty four hour clock GMT pattern.</value>
        protected virtual string timeTwentyFourHourGMTClockPattern
        {
            get { return "HH:mm 'GMT'"; }
        }

        /// <summary>
        /// Gets the twelve hour time pattern.
        /// </summary>
        /// <value>The twelve hour time pattern.</value>
        protected virtual string timeTwelveHourClockPattern
        {
            get { return "h:mm tt"; }
        }

        /// <summary>
        /// Gets the twenty four hour time pattern.
        /// </summary>
        /// <value>The twenty four hour time pattern.</value>
        protected virtual string timeTwentyFourHourClockPattern
        {
            get { return "HH:mm"; }
        }

        protected virtual string standardMonthYearPattern
        {
            get { return "MMM, yyyy"; }
        }
               
        #region ISupportedRegionalCulture Members

        /// <summary>
        /// Gets the standard Month Year pattern.
        /// </summary>
        /// <value>The standard date pattern.</value>
        public string StandardMonthYearPattern
        {
            get { return standardMonthYearPattern; }
        }

        /// <summary>
        /// Gets the standard date pattern.
        /// </summary>
        /// <value>The standard date pattern.</value>
        public virtual string StandardDatePattern
        {
            get { return standardDatePattern; }
        }

        /// <summary>
        /// Gets the short date pattern.
        /// </summary>
        /// <value>The short date pattern.</value>
        public virtual string ShortDatePattern
        {
            get { return shortDatePattern; }
        }

        /// <summary>
        /// Gets the long date pattern.
        /// </summary>
        /// <value>The long date pattern.</value>
        public virtual string LongDatePattern
        {
            get { return longDatePattern; }
        }

        public virtual string TimeTwelveHourClockPattern
        {
            get { return timeTwelveHourClockPattern; }
        }

        public virtual string TimeTwentyFourHourClockPattern
        {
            get { return timeTwentyFourHourClockPattern; }
        }

        public virtual string TimeTwelveHourGMTClockPattern
        {
            get { return timeTwelveHourGMTClockPattern; }
        }

        public virtual string TimeTwentyFourHourGMTClockPattern
        {
            get { return timeTwentyFourHourGMTClockPattern; }
        }

        /// <summary>
        /// Gets the standard date/time GMT pattern.
        /// </summary>
        /// <value>The standard date/time GMT pattern.</value>
        public virtual string DateTimeTwelveFourHourGMTStandardDateTimePattern
        {
            get { return string.Format("{0} {1}", StandardDatePattern, TimeTwelveHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the standard date/time GMT pattern.
        /// </summary>
        /// <value>The standard date/time GMT pattern.</value>
        public virtual string DateTimeTwentyFourHourGMTStandardDateTimePattern
        {
            get { return string.Format("{0} {1}", StandardDatePattern, TimeTwentyFourHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the standard date time twelve four hour pattern.
        /// </summary>
        /// <value>The standard date time twelve four hour pattern.</value>
        public virtual string DateTimeTwelveFourHourStandardDateTimePattern
        {
            get { return string.Format("{0} {1}", StandardDatePattern, TimeTwelveHourClockPattern); }
        }

        /// <summary>
        /// Gets the standard date time twenty four hour pattern.
        /// </summary>
        /// <value>The standard date time twenty four hour pattern.</value>
        public virtual string DateTimeTwentyFourHourStandardDateTimePattern
        {
            get { return string.Format("{0} {1}", StandardDatePattern, TimeTwentyFourHourClockPattern); }
        }

        /// <summary>
        /// Gets the standard date/time GMT pattern.
        /// </summary>
        /// <value>The standard date/time GMT pattern.</value>
        public virtual string DateTimeTwelveFourHourGMTStandardHeadlineDateTimePattern
        {
            get { return string.Format("{1}, {0}", StandardDatePattern, TimeTwelveHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the standard date/time GMT pattern.
        /// </summary>
        /// <value>The standard date/time GMT pattern.</value>
        public virtual string DateTimeTwentyFourHourGMTStandardHeadlineDateTimePattern
        {
            get { return string.Format("{1}, {0}", StandardDatePattern, TimeTwentyFourHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the standard date time twelve four hour pattern.
        /// </summary>
        /// <value>The standard date time twelve four hour pattern.</value>
        public virtual string DateTimeTwelveFourHourStandardHeadlineDateTimePattern
        {
            get { return string.Format("{1}, {0}", StandardDatePattern, TimeTwelveHourClockPattern); }
        }

        /// <summary>
        /// Gets the standard date time twenty four hour pattern.
        /// </summary>
        /// <value>The standard date time twenty four hour pattern.</value>
        public virtual string DateTimeTwentyFourHourStandardHeadlineDateTimePattern
        {
            get { return string.Format("{1}, {0}", StandardDatePattern, TimeTwentyFourHourClockPattern); }
        }

        /// <summary>
        /// Gets the Long date/time GMT pattern.
        /// </summary>
        /// <value>The Long date/time GMT pattern.</value>
        public virtual string DateTimeTwelveFourHourGMTLongDateTimePattern
        {
            get { return string.Format("{0} {1}", LongDatePattern, TimeTwelveHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the Long date/time GMT pattern.
        /// </summary>
        /// <value>The Long date/time GMT pattern.</value>
        public virtual string DateTimeTwentyFourHourGMTLongDateTimePattern
        {
            get { return string.Format("{0} {1}", LongDatePattern, TimeTwentyFourHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the Long date time twelve four hour pattern.
        /// </summary>
        /// <value>The Long date time twelve four hour pattern.</value>
        public virtual string DateTimeTwelveFourHourLongDateTimePattern
        {
            get { return string.Format("{0} {1}", LongDatePattern, TimeTwelveHourClockPattern); }
        }

        /// <summary>
        /// Gets the Long date time twenty four hour pattern.
        /// </summary>
        /// <value>The Long date time twenty four hour pattern.</value>
        public virtual string DateTimeTwentyFourHourLongDateTimePattern
        {
            get { return string.Format("{0} {1}", LongDatePattern, TimeTwentyFourHourClockPattern); }
        }

        /// <summary>
        /// Gets the Short date/time GMT pattern.
        /// </summary>
        /// <value>The Short date/time GMT pattern.</value>
        public virtual string DateTimeTwelveFourHourGMTShortDateTimePattern
        {
            get { return string.Format("{0} {1}", ShortDatePattern, TimeTwelveHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the Short date/time GMT pattern.
        /// </summary>
        /// <value>The Short date/time GMT pattern.</value>
        public virtual string DateTimeTwentyFourHourGMTShortDateTimePattern
        {
            get { return string.Format("{0} {1}", ShortDatePattern, TimeTwentyFourHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the Short date time twelve four hour pattern.
        /// </summary>
        /// <value>The Short date time twelve four hour pattern.</value>
        public virtual string DateTimeTwelveFourHourShortDateTimePattern
        {
            get { return string.Format("{0} {1}", ShortDatePattern, TimeTwelveHourClockPattern); }
        }

        /// <summary>
        /// Gets the Short date time twenty four hour pattern.
        /// </summary>
        /// <value>The Short date time twenty four hour pattern.</value>
        public virtual string DateTimeTwentyFourHourShortDateTimePattern
        {
            get { return string.Format("{0} {1}", ShortDatePattern, TimeTwentyFourHourClockPattern); }
        }

        /// <summary>
        /// Creates the culture info.
        /// </summary>
        /// <returns></returns>
        public CultureInfo CreateCultureInfo()
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture(Name);
            cultureInfo.DateTimeFormat.ShortTimePattern = timeTwentyFourHourClockPattern;
            return cultureInfo;
        }

        /// <summary>
        /// Gets the region code.
        /// </summary>
        /// <value>The region code.</value>
        public abstract string RegionCode { get; }

        /// <summary>
        /// Gets the name of the two letter ISO language.
        /// </summary>
        /// <value>The name of the two letter ISO language.</value>
        public abstract string TwoLetterISOLanguageName { get; }

        /// <summary>
        /// Gets the internal language code.
        /// </summary>
        /// <value>The internal language code.</value>
        public abstract string InternalLanguageCode { get; }

        /// <summary>
        /// Gets the name of the language.
        /// </summary>
        /// <value>The name of the language.</value>
        public abstract string LanguageName { get; }

        #endregion
    }
}