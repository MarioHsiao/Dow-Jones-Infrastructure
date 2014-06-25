using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DowJones.Attributes;
using DowJones.Converters;
using DowJones.Extensions;
using DowJones.Search.Attributes;
using Newtonsoft.Json;

namespace DowJones.Search
{
    [JsonConverter(typeof(GeneralJsonEnumConverter))]
    public enum SearchDateRange
    {
        /// <summary>
        /// Three Months. Corresponds to last 97 days.
        /// </summary>
        [TimeSlice(-91)]
        [AssignedToken("dateRangeLast3Months")]
        LastThreeMonths = 0,
        /// <summary>
        /// Last Day. Corresponds to last 2 days.
        /// </summary>
        [TimeSlice(-2)]
        [AssignedToken("dateRangeLastDay")]
        LastDay,
        /// <summary>
        /// Last Week. Corresponds to last week.
        /// </summary>
        [TimeSlice(-9)]
        [AssignedToken("dateRangeLastWeek")]
        LastWeek,
        /// <summary>
        /// Last Month. Corresponds to last month.
        /// </summary>
        [TimeSlice(-33)]
        [AssignedToken("dateRangeLastMonth")]
        LastMonth,
        /// <summary>
        /// Last Six Year. Corresponds to last 183 days.
        /// </summary>
        [TimeSlice(-183)]
        [AssignedToken("dateRangeLast6Months")]
        LastSixMonths,
        /// <summary>
        /// Last Year. Corresponds to last 367 days.
        /// </summary>
        [TimeSlice(-367)] // done for leap year
        [AssignedToken("dateRangeLastYear")]
        LastYear,
        /// <summary>
        /// Last two Year. Corresponds to last 734 days.
        /// </summary>
        [TimeSlice(-734)] // done for leap year
        [AssignedToken("dateRangeLast2Years")]
        LastTwoYears,

        /// <summary>
        /// Last two Year. Corresponds to last 1835 days.
        /// </summary>
        [TimeSlice(-1835)] // done for leap year 367 * 5 days
        [AssignedToken("dateRangeLast5Years")]
        LastFiveYears,

        /// <summary>
        /// All date range
        /// </summary>
        [AssignedToken("dateRangeAllDates")]
        All,

        /// <summary>
        /// Custom date range as per user date format (DMY or MDY or ISO) 
        /// </summary>
        [AssignedToken("customDateRange")]
        Custom,
    }

    public class DateRangeHelper
    {
        public static IEnumerable<KeyValuePair<string, string>> GetDateRange(bool addAllDate = false, bool addCustomDate = false)
        {

            IEnumerable<KeyValuePair<string, string>> dateRange = new[]
                                    {
                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastDay).ToString(CultureInfo.InvariantCulture), 
                                            SearchDateRange.LastDay.GetAssignedToken().ToTokenTranslation()),

                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastWeek).ToString(CultureInfo.InvariantCulture), 
                                            SearchDateRange.LastWeek.GetAssignedToken().ToTokenTranslation()),

                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastMonth).ToString(CultureInfo.InvariantCulture), 
                                            SearchDateRange.LastMonth.GetAssignedToken().ToTokenTranslation()),

                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastThreeMonths).ToString(CultureInfo.InvariantCulture), 
                                            SearchDateRange.LastThreeMonths.GetAssignedToken().ToTokenTranslation()),

                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastSixMonths).ToString(CultureInfo.InvariantCulture), 
                                            SearchDateRange.LastSixMonths.GetAssignedToken().ToTokenTranslation()),

                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastYear).ToString(CultureInfo.InvariantCulture), 
                                            SearchDateRange.LastYear.GetAssignedToken().ToTokenTranslation()),

                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastTwoYears).ToString(CultureInfo.InvariantCulture), 
                                            SearchDateRange.LastTwoYears.GetAssignedToken().ToTokenTranslation()),

                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastFiveYears).ToString(CultureInfo.InvariantCulture), 
                                            SearchDateRange.LastFiveYears.GetAssignedToken().ToTokenTranslation())
                                    };

            if (addAllDate)
            {
                dateRange = dateRange.Concat(new[]
                                       {
                                          new KeyValuePair<string, string>(((int)SearchDateRange.All).ToString(CultureInfo.InvariantCulture), 
                                            SearchDateRange.All.GetAssignedToken().ToTokenTranslation())
                                });
            }

            if (addCustomDate)
            {
                dateRange = dateRange.Concat(new[]
                                       {
                                          new KeyValuePair<string, string>(((int)SearchDateRange.Custom).ToString(CultureInfo.InvariantCulture), 
                                            SearchDateRange.Custom.GetAssignedToken().ToTokenTranslation())
                                       });
            }

            return dateRange;
        }
    }
}