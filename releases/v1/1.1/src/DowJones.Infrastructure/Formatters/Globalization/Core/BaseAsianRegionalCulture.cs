namespace DowJones.Utilities.Formatters.Globalization.Core
{
    public abstract class BaseAsianRegionalCulture : BaseRegionalCulture
    {
        private const string BASIC_ASIAN_DATE_PATTERN = "yyyy 年 M 月 d 日";
        private const string BASIC_ASIAN_MONTH_YEAR_PATTERN = "M 月, yyyy 年";

        /// <summary>
        /// Gets the short date pattern.
        /// </summary>
        /// <value>The short date pattern.</value>
        protected override string shortDatePattern
        {
            get { return BASIC_ASIAN_DATE_PATTERN; }
        }

        /// <summary>
        /// Gets the long date pattern.
        /// </summary>
        /// <value>The long date pattern.</value>
        protected override string longDatePattern
        {
            get { return BASIC_ASIAN_DATE_PATTERN; }
        }

        /// <summary>
        /// Gets the standard date pattern.
        /// </summary>
        /// <value>The standard date pattern.</value>
        protected override string standardDatePattern
        {
            get { return BASIC_ASIAN_DATE_PATTERN; }
        }

        protected override string standardMonthYearPattern
        {
            get
            {
                return BASIC_ASIAN_MONTH_YEAR_PATTERN;
            }
        }

        /// <summary>
        /// Gets the standard date/time GMT pattern.
        /// </summary>
        /// <value>The standard date/time GMT pattern.</value>
        public override string DateTimeTwelveFourHourGMTStandardDateTimePattern
        {
            get { return string.Format("{0} {1}", StandardDatePattern, TimeTwelveHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the standard date/time GMT pattern.
        /// </summary>
        /// <value>The standard date/time GMT pattern.</value>
        public override string DateTimeTwentyFourHourGMTStandardDateTimePattern
        {
            get { return string.Format("{0} {1}", StandardDatePattern, TimeTwentyFourHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the standard date time twelve four hour pattern.
        /// </summary>
        /// <value>The standard date time twelve four hour pattern.</value>
        public override string DateTimeTwelveFourHourStandardDateTimePattern
        {
            get { return string.Format("{0} {1}", StandardDatePattern, TimeTwelveHourClockPattern); }
        }

        /// <summary>
        /// Gets the standard date time twenty four hour pattern.
        /// </summary>
        /// <value>The standard date time twenty four hour pattern.</value>
        public override string DateTimeTwentyFourHourStandardDateTimePattern
        {
            get { return string.Format("{0} {1}", StandardDatePattern, TimeTwentyFourHourClockPattern); }
        }

        /// <summary>
        /// Gets the Long date/time GMT pattern.
        /// </summary>
        /// <value>The Long date/time GMT pattern.</value>
        public override string DateTimeTwelveFourHourGMTLongDateTimePattern
        {
            get { return string.Format("{0} {1}", LongDatePattern, TimeTwelveHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the Long date/time GMT pattern.
        /// </summary>
        /// <value>The Long date/time GMT pattern.</value>
        public override string DateTimeTwentyFourHourGMTLongDateTimePattern
        {
            get { return string.Format("{0} {1}", LongDatePattern, TimeTwentyFourHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the Long date time twelve four hour pattern.
        /// </summary>
        /// <value>The Long date time twelve four hour pattern.</value>
        public override string DateTimeTwelveFourHourLongDateTimePattern
        {
            get { return string.Format("{0} {1}", LongDatePattern, TimeTwelveHourClockPattern); }
        }

        /// <summary>
        /// Gets the Long date time twenty four hour pattern.
        /// </summary>
        /// <value>The Long date time twenty four hour pattern.</value>
        public override string DateTimeTwentyFourHourLongDateTimePattern
        {
            get { return string.Format("{0} {1}", LongDatePattern, TimeTwentyFourHourClockPattern); }
        }

        /// <summary>
        /// Gets the standard date/time GMT pattern.
        /// </summary>
        /// <value>The standard date/time GMT pattern.</value>
        public override string DateTimeTwelveFourHourGMTStandardHeadlineDateTimePattern
        {
            get { return string.Format("{0} {1}", StandardDatePattern, TimeTwelveHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the standard date/time GMT pattern.
        /// </summary>
        /// <value>The standard date/time GMT pattern.</value>
        public override string DateTimeTwentyFourHourGMTStandardHeadlineDateTimePattern
        {
            get { return string.Format("{0} {1}", StandardDatePattern, TimeTwentyFourHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the standard date time twelve four hour pattern.
        /// </summary>
        /// <value>The standard date time twelve four hour pattern.</value>
        public override string DateTimeTwelveFourHourStandardHeadlineDateTimePattern
        {
            get { return string.Format("{0} {1}", StandardDatePattern, TimeTwelveHourClockPattern); }
        }

        /// <summary>
        /// Gets the standard date time twenty four hour pattern.
        /// </summary>
        /// <value>The standard date time twenty four hour pattern.</value>
        public override string DateTimeTwentyFourHourStandardHeadlineDateTimePattern
        {
            get { return string.Format("{0} {1}", StandardDatePattern, TimeTwentyFourHourClockPattern); }
        }


        /// <summary>
        /// Gets the Short date/time GMT pattern.
        /// </summary>
        /// <value>The Short date/time GMT pattern.</value>
        public override string DateTimeTwelveFourHourGMTShortDateTimePattern
        {
            get { return string.Format("{0} {1}", ShortDatePattern, TimeTwelveHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the Short date/time GMT pattern.
        /// </summary>
        /// <value>The Short date/time GMT pattern.</value>
        public override string DateTimeTwentyFourHourGMTShortDateTimePattern
        {
            get { return string.Format("{0} {1}", ShortDatePattern, TimeTwentyFourHourGMTClockPattern); }
        }

        /// <summary>
        /// Gets the Short date time twelve four hour pattern.
        /// </summary>
        /// <value>The Short date time twelve four hour pattern.</value>
        public override string DateTimeTwelveFourHourShortDateTimePattern
        {
            get { return string.Format("{0} {1}", ShortDatePattern, TimeTwelveHourClockPattern); }
        }

        /// <summary>
        /// Gets the Short date time twenty four hour pattern.
        /// </summary>
        /// <value>The Short date time twenty four hour pattern.</value>
        public override string DateTimeTwentyFourHourShortDateTimePattern
        {
            get { return string.Format("{0} {1}", ShortDatePattern, TimeTwentyFourHourClockPattern); }
        }
    }
}