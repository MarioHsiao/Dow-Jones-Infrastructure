using DowJones.Infrastructure;

namespace DowJones.Search.Filters
{
    public class DateRangeQueryFilter : IQueryFilter
    {
        public DateRange DateRange { get; set; }


        public DateRangeQueryFilter()
        {
            DateRange = new DateRange(null, null);
        }

        public DateRangeQueryFilter(DateRange dateRange)
        {
            DateRange = dateRange;
        }

        public DateRangeQueryFilter(string dateRange)
        {
            DateRange = DateRange.Deserialize(dateRange);
        }
    }
}
