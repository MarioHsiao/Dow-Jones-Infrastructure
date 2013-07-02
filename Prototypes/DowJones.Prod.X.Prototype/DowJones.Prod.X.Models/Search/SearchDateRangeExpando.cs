namespace DowJones.Prod.X.Models.Search
{
    public static class SearchDateRangeExpando
    {
        public static string Convert(this SearchDateRange dateRange)
        {
            switch (dateRange)
            {
                case SearchDateRange.LastDay:
                    return "-2";
                case SearchDateRange.LastWeek:
                    return "-9";
                case SearchDateRange.LastMonth:
                    return "-33";
                case SearchDateRange.LastSixMonths:
                    return "-183";
                case SearchDateRange.LastYear:
                    return "-367";
                case SearchDateRange.LastTwoYears:
                    return "-734";
                    /*case SearchDateRange.All:
                  case SearchDateRange.LastThreeMonths:
                  case SearchDateRange.SinceDays:
                  case SearchDateRange.Unspecified:*/
                default:
                    return "-91";
            }
        }
    }
}