using System.Collections.Generic;
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
        /// Last two Year. Corresponds to last 367 days.
        /// </summary>
        [TimeSlice(-734)] // done for leap year
        [AssignedToken("dateRangeLast2Years")]
        LastTwoYears,

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
                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastDay).ToString(), 
                                            SearchDateRange.LastDay.GetAssignedToken().ToTokenTranslation()),

                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastWeek).ToString(), 
                                            SearchDateRange.LastWeek.GetAssignedToken().ToTokenTranslation()),

                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastMonth).ToString(), 
                                            SearchDateRange.LastMonth.GetAssignedToken().ToTokenTranslation()),

                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastThreeMonths).ToString(), 
                                            SearchDateRange.LastThreeMonths.GetAssignedToken().ToTokenTranslation()),

                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastSixMonths).ToString(), 
                                            SearchDateRange.LastSixMonths.GetAssignedToken().ToTokenTranslation()),

                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastYear).ToString(), 
                                            SearchDateRange.LastYear.GetAssignedToken().ToTokenTranslation()),

                                        new KeyValuePair<string, string>(((int)SearchDateRange.LastTwoYears).ToString(), 
                                            SearchDateRange.LastTwoYears.GetAssignedToken().ToTokenTranslation())
                                    };

            if (addAllDate)
            {
                dateRange = dateRange.Concat(new[]
                                       {
                                          new KeyValuePair<string, string>(((int)SearchDateRange.All).ToString(), 
                                            SearchDateRange.All.GetAssignedToken().ToTokenTranslation()), 
                                });
            }

            if (addCustomDate)
            {
                dateRange = dateRange.Concat(new[]
                                       {
                                          new KeyValuePair<string, string>(((int)SearchDateRange.Custom).ToString(), 
                                            SearchDateRange.Custom.GetAssignedToken().ToTokenTranslation()), 
                                });
            }

            return dateRange;
        }
    }
}