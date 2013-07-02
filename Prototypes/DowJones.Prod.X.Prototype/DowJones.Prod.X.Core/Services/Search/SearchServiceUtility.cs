using DowJones.Prod.X.Common.Extentions;
using DowJones.Prod.X.Models.Search;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.Messages.Search.V2_0;
using DeduplicationMode = Factiva.Gateway.Messages.Search.V2_0.DeduplicationMode;
using SearchMode = Factiva.Gateway.Messages.Search.V2_0.SearchMode;

namespace DowJones.Prod.X.Core.Services.Search
{
    public static class SearchServiceUtility
    {
        public static DefaultSimpleSearchDateRange MapDateRange(this SearchDateRange searchDateRange)
        {
            switch (searchDateRange)
            {
                case SearchDateRange.LastDay:
                    return DefaultSimpleSearchDateRange.LastDay;
                case SearchDateRange.LastWeek:
                    return DefaultSimpleSearchDateRange.LastWeek;
                case SearchDateRange.LastMonth:
                    return DefaultSimpleSearchDateRange.LastMonth;
                case SearchDateRange.LastSixMonths:
                    return DefaultSimpleSearchDateRange.Last6Months;
                case SearchDateRange.LastYear:
                    return DefaultSimpleSearchDateRange.LastYear;
                case SearchDateRange.LastTwoYears:
                    return DefaultSimpleSearchDateRange.Last2Years;
                case SearchDateRange.LastThreeMonths:
                    return DefaultSimpleSearchDateRange.Last3Months;
                case SearchDateRange.All:
                    return DefaultSimpleSearchDateRange.All;
                default:
                    return DefaultSimpleSearchDateRange.Last3Months;
            }
        }

        public static PreferenceSearchDateRange MapPreferenceDateRange(this SearchDateRange searchDateRange)
        {
            return searchDateRange.ParseEnum(PreferenceSearchDateRange.LastThreeMonths);
        }

        public static DeduplicationMode MapDeduplicationLevel(this DeduplicationLevel deduplication)
        {
            return deduplication.ParseEnum(DeduplicationMode.Off);
        }

        public static ResultSortOrder MapGatewaySortOrder(this Models.Search.SortOrder searchSorting)
        {
            return searchSorting.ParseEnum(ResultSortOrder.PublicationDateReverseChronological);
        }

        public static SearchMode MapGatewaySearchMode(this Models.Search.SearchMode searchMode)
        {
            return searchMode.ParseEnum(SearchMode.Simple);
        }

        public static DeduplicationMode MapGatewayDeduplicationModel(this Models.Search.DeduplicationMode deduplicationMode )
        {
            return deduplicationMode.ParseEnum(DeduplicationMode.Off);
        }
    }
}
