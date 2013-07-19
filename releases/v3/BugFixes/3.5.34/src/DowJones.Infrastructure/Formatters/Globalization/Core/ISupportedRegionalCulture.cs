using System.Globalization;

namespace DowJones.Formatters.Globalization.Core
{
    internal interface ISupportedRegionalCulture
    {
        /// <summary>
        /// Gets the standard Month Year pattern.
        /// </summary>
        /// <value>The standard date pattern.</value>
        string StandardMonthYearPattern { get; }

        /// <summary>
        /// Gets the standard date pattern.
        /// </summary>
        /// <value>The standard date pattern.</value>
        string StandardDatePattern { get; }

        /// <summary>
        /// Gets the short date pattern.
        /// </summary>
        /// <value>The short date pattern.</value>
        string ShortDatePattern { get; }

        /// <summary>
        /// Gets the long date pattern.
        /// </summary>
        /// <value>The long date pattern.</value>
        string LongDatePattern { get; }

        /// <summary>
        /// Gets the twelve hour clock pattern.
        /// </summary>
        /// <value>The twelve hour clock pattern.</value>
        string TimeTwelveHourClockPattern { get; }

        /// <summary>
        /// Gets the twenty four hour clock pattern.
        /// </summary>
        /// <value>The twenty four hour clock pattern.</value>
        string TimeTwentyFourHourClockPattern { get; }

        /// <summary>
        /// Gets the twelve hour GMT clock pattern.
        /// </summary>
        /// <value>The twelve hour GMT clock pattern.</value>
        string TimeTwelveHourGMTClockPattern { get; }

        /// <summary>
        /// Gets the twenty four hour GMT clock pattern.
        /// </summary>
        /// <value>The twenty four hour GMT clock pattern.</value>
        string TimeTwentyFourHourGMTClockPattern { get; }

        /// <summary>
        /// Gets the standard date/time GMT pattern.
        /// </summary>
        /// <value>The standard date/time GMT pattern.</value>
        string DateTimeTwelveFourHourGMTStandardDateTimePattern { get; }

        /// <summary>
        /// Gets the standard date/time GMT pattern.
        /// </summary>
        /// <value>The standard date/time GMT pattern.</value>
        string DateTimeTwentyFourHourGMTStandardDateTimePattern { get; }

        /// <summary>
        /// Gets the standard date time twelve four hour pattern.
        /// </summary>
        /// <value>The standard date time twelve four hour pattern.</value>
        string DateTimeTwelveFourHourStandardDateTimePattern { get; }

        /// <summary>
        /// Gets the standard date time twenty four hour pattern.
        /// </summary>
        /// <value>The standard date time twenty four hour pattern.</value>
        string DateTimeTwentyFourHourStandardHeadlineDateTimePattern { get; }

        /// <summary>
        /// Gets the standard date/time GMT pattern.
        /// </summary>
        /// <value>The standard date/time GMT pattern.</value>
        string DateTimeTwelveFourHourGMTStandardHeadlineDateTimePattern { get; }

        /// <summary>
        /// Gets the standard date/time GMT pattern.
        /// </summary>
        /// <value>The standard date/time GMT pattern.</value>
        string DateTimeTwentyFourHourGMTStandardHeadlineDateTimePattern { get; }

        /// <summary>
        /// Gets the standard date time twelve four hour pattern.
        /// </summary>
        /// <value>The standard date time twelve four hour pattern.</value>
        string DateTimeTwelveFourHourStandardHeadlineDateTimePattern { get; }

        /// <summary>
        /// Gets the standard date time twenty four hour pattern.
        /// </summary>
        /// <value>The standard date time twenty four hour pattern.</value>
        string DateTimeTwentyFourHourStandardDateTimePattern { get; }

        /// <summary>
        /// Gets the Long date/time GMT pattern.
        /// </summary>
        /// <value>The Long date/time GMT pattern.</value>
        string DateTimeTwelveFourHourGMTLongDateTimePattern { get; }

        /// <summary>
        /// Gets the Long date/time GMT pattern.
        /// </summary>
        /// <value>The Long date/time GMT pattern.</value>
        string DateTimeTwentyFourHourGMTLongDateTimePattern { get; }

        /// <summary>
        /// Gets the Long date time twelve four hour pattern.
        /// </summary>
        /// <value>The Long date time twelve four hour pattern.</value>
        string DateTimeTwelveFourHourLongDateTimePattern { get; }

        /// <summary>
        /// Gets the Long date time twenty four hour pattern.
        /// </summary>
        /// <value>The Long date time twenty four hour pattern.</value>
        string DateTimeTwentyFourHourLongDateTimePattern { get; }

        /// <summary>
        /// Gets the region code.
        /// </summary>
        /// <value>The region code.</value>
        string RegionCode { get; }

        /// <summary>
        /// Gets the name of the two letter ISO language.
        /// </summary>
        /// <value>The name of the two letter ISO language.</value>
        string TwoLetterISOLanguageName { get; }

        /// <summary>
        /// Gets the internal language code.
        /// </summary>
        /// <value>The internal language code.</value>
        string InternalLanguageCode { get; }

        /// <summary>
        /// Gets the name of the language.
        /// </summary>
        /// <value>The name of the language.</value>
        string LanguageName { get; }

        CultureInfo CreateCultureInfo();
    }
}